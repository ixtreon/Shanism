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
        public static bool ShowDebugStats;


        string playerName;

        readonly IGraphicsDeviceService graphics;

        readonly ContentManager ContentManager;

        readonly ConcurrentQueue<IOMessage> pendingMessages = new ConcurrentQueue<IOMessage>();


        SystemGod Systems;

        IShanoEngine server;
        IReceptor receptor;

        ContentList Content;

        SpriteBatch spriteBatch;

        ShaderContainer shaders;

        /// <summary>
        /// The average time to render a frame. 
        /// </summary>
        double timeToRender;

        string debugString;

        bool isDesignMode;


        bool isConnected;

        public ClientState State { get; } = new ClientState();

        public RenderTarget2D RenderTarget { get; set; }


        public event Action<IOMessage> MessageSent;

        GraphicsDevice graphicsDevice => graphics.GraphicsDevice;

        string IShanoClient.Name => playerName;

        TextureCache IClientEngine.Textures => Content.Textures;


        public ClientEngine(IGraphicsDeviceService graphics, ContentManager content, string playerName = "Shanist")
        {
            ContentManager = content;
            this.playerName = playerName;
            this.graphics = graphics;
        }


        #region IClientEngine implementation

        public bool TryConnect(IShanoEngine server, string playerName, out IReceptor receptor)
        {
            //set new server
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            disconnect();
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

        void disconnect()
        {
            if (receptor != null)
                receptor.MessageSent -= server_MessageSent;

            isConnected = false;
            receptor = null;
            server = null;
        }


        void initSystems()
        {
            if (Systems != null)
                Systems.MessageSent -= sendMessage;

            Systems = new SystemGod(graphicsDevice, Content, receptor, State);
            Systems.MessageSent += sendMessage;
        }

        void server_MessageSent(IOMessage msg)
        {
            if (msg != null)
                pendingMessages.Enqueue(msg);
        }

        bool IClientEngine.LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphicsDevice);

            //load default content (textures,fonts,animations)
            Content = new ContentList(graphicsDevice);
            if (!Content.LoadDefault(ContentManager))
            {
                return false;
            }

            //shaders
            shaders = new ShaderContainer(ContentManager);

            // ??? 
            graphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.CullClockwiseFace,
            };

            initSystems();
            return true;
        }

        public override void Update(int msElapsed)
        {
            //update the local server
            if (isConnected)
                server.Update(msElapsed);

            //Parse its messages
            IOMessage msg;
            while (pendingMessages.TryDequeue(out msg))
                parseMessage(msg);

            if (isConnected)
            {
                //pan camera to the hero
                if (Systems.MainHero != null)
                    Screen.MoveCamera(Systems.MainHero.Position);

                //ui, objects
                Systems.Update(msElapsed);

                //debug
                updateDebugStats(msElapsed);
            }
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

            graphicsDevice.Clear(Color.Black);
            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            // 1. draw terrain
            Systems.DrawTerrain();

            // 2. draw gameobjects
            Systems.DrawObjects();

            if (!isDesignMode)
            {
                // 3. draw shadow layer
                var shader = shaders.FogOfWar;
                if (shader != null && Systems.MainHero != null)
                {
                    var destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(Screen.Size.X * Screen.RenderSize), (int)(Screen.Size.Y * Screen.RenderSize));
                    var tint = Color.White;
                    var tex = Content.Textures.Blank;

                    shader.Parameters["TexSize"].SetValue((Screen.GameSize).ToVector2());
                    shader.Parameters["SightRange"].SetValue((float)Systems.MainHero.VisionRange);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                        SamplerState.PointClamp, DepthStencilState.DepthRead,
                        RasterizerState.CullNone, shader);

                    spriteBatch.Draw(tex, destRect, tint);

                    spriteBatch.End();
                }

                //draw the buffer to screen
                if (RenderTarget != null)
                {
                    graphicsDevice.SetRenderTarget(null);
                    using (var sb = new SpriteBatch(graphicsDevice))
                    {
                        sb.Begin();
                        var sz = Screen.Size;
                        sb.Draw(RenderTarget,
                            new Microsoft.Xna.Framework.Rectangle(0, 0, (int)sz.X, (int)sz.Y),
                            new Microsoft.Xna.Framework.Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height),
                            Color.White);
                        sb.End();
                    }
                }


                // 4. interface
                graphicsDevice.SetRenderTarget(null);
                Systems.DrawUi();
                //debug
                if (ShowDebugStats)
                {
                    spriteBatch.Begin();
                    drawDebugStats(spriteBatch);
                    spriteBatch.End();
                }
            }
        }

        void IClientEngine.SetWindowSize(Point sz)
        {
            if (sz.X == 0 || sz.Y == 0 || sz == Screen.Size)
                return;

            Screen.SetWindowSize(/*((Vector)*/sz /** Settings.Current.RenderSize).ToPoint()*/);
        }

        void IClientEngine.MoveCamera(Vector? inGameCenter, Vector? inGameSz)
            => Screen.MoveCamera(inGameCenter ?? Screen.GameCenter, inGameSz ?? Screen.GameSize);

        void IClientEngine.SetDesignMode(bool isDesignMode)
        {
            this.isDesignMode = isDesignMode;
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
                Content.LoadScenario(ContentManager, msg);
            }
        }

        #region Drawing Helpers

        void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = Content.Fonts.LargeFont;
            var pos = new Point(Screen.Size.X / 10);

            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            font.DrawString(spriteBatch, txt, Color.DarkRed, pos, 0f, 0f);
            spriteBatch.End();
        }

        #endregion

        #region Debug Stats


        void updateDebugStats(int msElapsed)
        {
            const double frameConst = 0.5;

            // FPS
            timeToRender = (1 - frameConst) * timeToRender + frameConst * msElapsed;
            var curFps = 1000 / timeToRender;

            // mouse
            var mpUi = MouseInfo.UiPosition;
            var mpGame = MouseInfo.InGamePosition;
            var heroPos = Systems.MainHero?.Position;

            debugString = string.Join("\n", new[]
            {
                $"FPS: {curFps:00}",

                $"BackBuffer Size: {RenderTarget?.Bounds.Size}",
                $"Window Size: {Screen.Size}",
                $"Mouse: {MouseInfo.ScreenPosition}",

                $"Mouse UI: {mpUi:0.00}",
                $"Mouse InGame: {mpGame:0.00}",
                $"Hero X/Y: {heroPos:0.00}",

                $"UI Hover: {Control.HoverControl.GetType().Name}",
                $"UI Focus: {Control.FocusControl?.GetType().Name }",

                receptor?.GetDebugString(),
            });
        }

        void drawDebugStats(SpriteBatch sb)
        {
            Content.Fonts.NormalFont.DrawString(sb, debugString, Color.Goldenrod,
                new Point(24, 18), 0, 0);
        }

        #endregion
    }
}
