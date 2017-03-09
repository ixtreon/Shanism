using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Drawing;
using Shanism.Common.Message.Client;


using Color = Microsoft.Xna.Framework.Color;
using Shanism.Client.GameScreens;

namespace Shanism.Client
{
    /// <summary>
    /// Contains all the drawing and update logic of the client. 
    /// </summary>
    class ClientEngine : GameScreen, IClientEngine
    {
        static readonly Keybind cancelConnectKeybind = new Keybind(Microsoft.Xna.Framework.Input.Keys.Escape);

        string playerName;

        readonly ConcurrentQueue<IOMessage> pendingMessages = new ConcurrentQueue<IOMessage>();


        SystemGod Systems;

        IShanoEngine server;
        IReceptor receptor;
        
        bool isDesignMode;

        bool isConnected;

        public PlayerState State { get; } = new PlayerState();

        public RenderTarget2D RenderTarget { get; set; }


        public event Action<IOMessage> MessageSent;


        string IShanoClient.Name => playerName;


        public ClientEngine(GameComponent game, string playerName = "Shanist")
            : base(game)
        {
            this.playerName = playerName;

            initSystems();
        }


        #region IClientEngine implementation

        TextureCache IClientEngine.Textures => Content.Textures;

        public bool TryConnect(IShanoEngine server, string playerName, 
            out IReceptor receptor)
        {
            //set new server
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            Disconnect();
            this.server = server;

            //request a new receptor
            receptor = server.Connect(this);
            if (receptor == null)
                return false;

            receptor.MessageSent += server_MessageSent;
            this.receptor = receptor;

            initSystems();
            return true;
        }

        void IClientEngine.RestartScenario()
        {
            if (server != null)
            {
                server.RestartScenario();
                if (TryConnect(server, playerName, out receptor))
                    server.StartPlaying(receptor);
            }
        }

        public void Disconnect()
        {
            if (receptor != null)
            {
                receptor.Disconnect();
                receptor.MessageSent -= server_MessageSent;
                receptor = null;
            }

            isConnected = false;
            server = null;
        }


        void initSystems()
        {
            if (Systems != null)
                Systems.MessageSent -= sendMessage;

            Systems = new SystemGod(this, receptor, State);
            Systems.MessageSent += sendMessage;
        }

        void server_MessageSent(IOMessage msg)
        {
            if (msg != null)
                pendingMessages.Enqueue(msg);
        }
        
        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);

            //quit while loading
            if (!isConnected && Keyboard.IsActivated(cancelConnectKeybind))
            {
                GameHelper.Quit();
                return;
            }

            //update the local server
            if (server != null)
                server.Update(msElapsed);

            //Parse its messages
            IOMessage msg;
            while (pendingMessages.TryDequeue(out msg))
                parseMessage(msg);


            if (!isConnected)
                return;

            //pan camera to the hero
            if (Systems.MainHero != null)
                Screen.MoveCamera(Systems.MainHero.Position);

            //update all systems
            Systems.Update(msElapsed);
        }

        public override void Draw()
        {

            if (!isConnected)
            {
                drawConnectingScreen();
                return;
            }


            if (receptor == null)
                return;

            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            // 1. draw terrain
            Systems.DrawTerrain();

            // 2. draw gameobjects
            Systems.DrawObjects();

            if (!isDesignMode)
            {
                // 3. draw shadow layer
                var shader = Content.Shaders.FogOfWar;
                if (shader != null && Systems.MainHero != null)
                {
                    var destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 
                        (int)(Screen.Size.X * Screen.RenderSize), 
                        (int)(Screen.Size.Y * Screen.RenderSize));
                    var tint = Color.White;
                    var tex = Content.Textures.Blank;

                    shader.Parameters["TexSize"].SetValue((Screen.GameSize).ToVector2());
                    shader.Parameters["SightRange"].SetValue((float)Systems.MainHero.VisionRange);

                    Canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                        SamplerState.PointClamp, DepthStencilState.DepthRead,
                        RasterizerState.CullNone, shader);
                    Canvas.Draw(tex, destRect, tint);
                    Canvas.End();
                }

                // draw to screen if all drawing was done to a texture
                if (RenderTarget != null)
                {
                    GraphicsDevice.SetRenderTarget(null);

                    using (var sb = new SpriteBatch(GraphicsDevice))
                    {
                        Canvas.Begin();
                        var sz = Screen.Size;
                        Canvas.Draw(RenderTarget,
                            new Microsoft.Xna.Framework.Rectangle(0, 0, (int)sz.X, (int)sz.Y),
                            new Microsoft.Xna.Framework.Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height),
                            Color.White);
                        Canvas.End();
                    }
                }


                // 4. interface
                GraphicsDevice.SetRenderTarget(null);
                Systems.DrawUi();
            }

            base.Draw();
        }

        void IClientEngine.MoveCamera(Vector? inGameCenter, Vector? inGameSz)
            => Screen.MoveCamera(inGameCenter ?? Screen.GameCenter, inGameSz ?? Screen.GameSize);

        void IClientEngine.SetDesignMode(bool isDesignMode)
        {
            this.isDesignMode = isDesignMode;
        }

        void IClientEngine.SetWindowSize(Point sz)
        {
            if(sz.X == 0 || sz.Y == 0 || sz == Screen.Size)
                return;

            Screen.SetWindowSize(sz);
        }

        Vector IClientEngine.GameToScreen(Vector gamePos)
            => Screen.GameToScreen(gamePos);

        Vector IClientEngine.ScreenToGame(Vector screenPos)
            => Screen.ScreenToGame(screenPos);

        void sendMessage(IOMessage msg) => MessageSent?.Invoke(msg);

        void parseMessage(IOMessage msg)
        {
            switch (msg.Type)
            {
                case MessageType.HandshakeReply:
                    onHandshakeReply((HandshakeReplyMessage)msg);
                    break;

                case MessageType.Disconnected:
                    GameHelper.Quit();
                    break;

                default:
                    Systems.HandleMessage(msg);
                    break;
            }
        }

        #endregion

        void onHandshakeReply(HandshakeReplyMessage msg)
        {
            isConnected = msg.Success;
            if (isConnected)
            {
                //load content n start playing!
                Content.LoadScenario(msg);
            }
        }

        #region Drawing Helpers

        void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = Content.Fonts.LargeFont;
            var pos = new Point(Screen.Size.X / 10);

            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            Canvas.Begin();
            Canvas.DrawString(font, txt, Color.DarkRed.ToShanoColor(), pos, 0f, 0f);
            Canvas.End();
        }

        #endregion

        /// <summary>
        /// Recreates the 
        /// </summary>
        /// <param name="windowSize"></param>
        public void RecreateBuffer(Point windowSize)
        {
            //if rendersize == 1 we can just draw to the screen
            if (Screen.RenderSize > 0.99f)
            {
                RenderTarget = null;
                return;
            }

            //get new buffer size, see if it changed
            var bufferSize = ((Vector)windowSize * Screen.RenderSize).ToPoint();
            if (RenderTarget?.Bounds.Size.ToPoint() == bufferSize)
                return;

            RenderTarget = new RenderTarget2D(GraphicsDevice, bufferSize.X, bufferSize.Y);
        }

        #region Debug Stats

        protected override void writeDebugStats(List<string> lines)
        {
            lines.Insert(2, $"Buffer Size: {RenderTarget?.Bounds.Size}");

            lines.Add(receptor?.GetDebugString());
        }

        #endregion
    }
}
