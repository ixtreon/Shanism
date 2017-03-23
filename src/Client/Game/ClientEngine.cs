using Shanism.Client.Input;
using Shanism.Common;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common.Message.Client;


using Shanism.Client.Assets;
using Shanism.Client.UI;
using Shanism.Client.UI.Game;
using Shanism.Client.UI.Chat;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client
{
    /// <summary>
    /// Contains all the drawing and update logic of the client. 
    /// </summary>
    partial class ClientEngine : UiScreen, IClientEngine, IChatConsumer, IChatSource
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

        new GameRoot Root { get; }

        GraphicsDevice GraphicsDevice => Screen.GraphicsDevice;

        public ClientEngine(IShanoComponent game, string playerName = "Shanist")
            : this(game, new GameRoot(), playerName) { }

        public ClientEngine(IShanoComponent game, GameRoot customRoot, string playerName = "Shanist")
            : base(game, customRoot)
        {
            Root = customRoot;
            this.playerName = playerName;

            reloadSystems();

            Control.SetContext(this);
        }

        IHero MainHero => Systems.Objects.MainHero;


        #region IClientEngine implementation

        public TextureCache Textures => Content.Textures;


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

            reloadSystems();
            return true;
        }

        public void RestartScenario()
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


        void reloadSystems()
        {
            if (Systems != null)
                Systems.MessageSent -= sendMessage;

            Systems = new SystemGod(this, Root, receptor, State);
            Systems.MessageSent += sendMessage;

            Root.Init(this, this, Systems.Objects);
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
            if (MainHero != null)
                Screen.MoveCamera(MainHero.Position);

            //reload ui & stuff
            if (Keyboard.JustActivatedActions.Contains(ClientAction.ReloadUi))
                reloadUi();

            //update all systems
            Systems.Update(msElapsed);
        }

        void reloadUi()
        {
            reloadSystems();
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

            GraphicsDevice.Clear(Color.Black.ToXnaColor());
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            // 1. draw terrain
            Systems.Terrain.Draw();

            // 2. draw gameobjects
            Systems.Objects.Draw();

            if (!isDesignMode)
            {
                // 3. draw shadow layer
                var shader = Content.Shaders.FogOfWar;
                if (shader != null && MainHero != null)
                {
                    var destRect = new Microsoft.Xna.Framework.Rectangle(0, 0,
                        (int)(Screen.Size.X * Screen.RenderScale),
                        (int)(Screen.Size.Y * Screen.RenderScale));
                    var tint = Color.White;
                    var tex = Content.Textures.Blank;

                    shader.Parameters["TexSize"].SetValue(Screen.GameSize.ToVector2());
                    shader.Parameters["SightRange"].SetValue((float)MainHero.VisionRange);

                    Canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                        SamplerState.PointClamp, DepthStencilState.DepthRead,
                        RasterizerState.CullNone, shader);
                    Canvas.Draw(tex, Vector.Zero, Screen.UiSize);
                    Canvas.End();
                }

                // draw to screen if all drawing was done to a texture
                if (RenderTarget != null)
                {
                    GraphicsDevice.SetRenderTarget(null);

                    using (var sb = new SpriteBatch(GraphicsDevice))
                    {
                        var sz = Screen.Size;

                        Canvas.Begin();
                        Canvas.Draw(RenderTarget,
                            new Microsoft.Xna.Framework.Rectangle(0, 0, sz.X, sz.Y),
                            new Microsoft.Xna.Framework.Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height),
                            Microsoft.Xna.Framework.Color.White);
                        Canvas.End();
                    }
                }


                // 4. interface
                GraphicsDevice.SetRenderTarget(null);

                Canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                    SamplerState.AnisotropicWrap, DepthStencilState.DepthRead,
                    RasterizerState.CullNone);
                Root.Draw(Canvas);
                Canvas.End();

            }

            Canvas.Begin();
            DrawDebugStats();
            Canvas.End();
        }

        void IClientEngine.MoveCamera(Vector? inGameCenter, Vector? inGameSz)
            => Screen.MoveCamera(inGameCenter ?? Screen.GameCenter, inGameSz ?? Screen.GameSize);

        void IClientEngine.SetDesignMode(bool isDesignMode)
        {
            this.isDesignMode = isDesignMode;
        }

        public void SetWindowSize(Point sz)
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

                case MessageType.DamageEvent:

                    var dmgEv = (DamageEventMessage)msg;
                    var unit = Systems.Objects.TryGet(dmgEv.UnitId);
                    if (unit == null)
                        return;

                    var text = dmgEv.ValueChange.ToString("0");
                    Root.FloatingText.AddLabel(unit.Position, text, Color.Red, 
                        FloatingTextStyle.Rainbow);
                    break;

                case MessageType.PlayerStatusUpdate:
                    Systems.Objects.SetMainHero(((PlayerStatusMessage)msg).HeroId);
                    break;

                case MessageType.MapReply:
                    Systems.Terrain.ParseMessage((MapDataMessage)msg);
                    break;

                case MessageType.ServerChat:
                    ChatMessageSent?.Invoke(((ChatMessage)msg).Message);
                    break;

                default:
                    Console.WriteLine("Unhandled message: " + msg.Type);
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
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            Canvas.Begin();
            Canvas.DrawString(font, txt, Color.DarkRed, pos, 0f, 0f);
            Canvas.End();
        }

        #endregion

        /// <summary>
        /// Recreates the <see cref="RenderTarget"/> texture used to draw everything.
        /// Actually destroys the texture if <see cref="Screen.RenderScale"/> is close to 1. 
        /// </summary>
        public void RecreateBuffer()
        {
            //if rendersize == 1 we can just draw to the screen
            if (Screen.RenderScale > 0.99f)
            {
                RenderTarget = null;
                return;
            }

            //get new buffer size, see if it changed
            var bufferSize = ((Vector)Screen.Size * Screen.RenderScale).ToPoint();
            if (RenderTarget?.Bounds.Size.ToPoint() == bufferSize)
                return;

            RenderTarget = new RenderTarget2D(GraphicsDevice, bufferSize.X, bufferSize.Y);
        }

        protected override void WriteDebug(List<string> lines)
        {
            lines.Insert(2, $"Buffer Size: {RenderTarget?.Bounds.Size}");

            lines.Add(receptor?.GetDebugString());
        }
    }
}
