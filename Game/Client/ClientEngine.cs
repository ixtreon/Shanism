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

using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Shanism.Common.Interfaces.Entities;
using Shanism.Client.Drawing;
using Shanism.Common.Message.Client;

namespace Shanism.Client
{
    /// <summary>
    /// Contains all the drawing and update logic of the client. 
    /// </summary>
    class ClientEngine : IClientEngine
    {
        public static bool ShowDebugStats;


        readonly string playerName;

        readonly IGraphicsDeviceService graphics;

        readonly ContentManager ContentManager;

        readonly ConcurrentQueue<IOMessage> pendingMessages = new ConcurrentQueue<IOMessage>();


        /// <summary>
        /// Contains all systems. 
        /// </summary>
        SystemGod Game;

        IReceptor server;

        AssetList Content;

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


        public event Action<IOMessage> MessageSent;

        GraphicsDevice graphicsDevice => graphics.GraphicsDevice;

        string IShanoClient.Name => playerName;

        TextureCache IClientEngine.Textures => Content.Textures;


        public ClientEngine(string playerName, IGraphicsDeviceService graphics, ContentManager content)
        {
            ContentManager = content;
            this.playerName = playerName;
            this.graphics = graphics;
        }


        #region IClientEngine implementation

        void IClientEngine.SetServer(IReceptor server)
        {
            isConnected = false;

            if (server != null)
                server.MessageSent -= server_MessageSent;

            this.server = server;
            server.MessageSent += server_MessageSent;

            tryInitGame();
        }

        void tryInitGame()
        {
            if (Game == null
                && server != null
                && graphicsDevice != null)
            {
                Game = new SystemGod(graphicsDevice, Content, server, State);
                Game.MessageSent += sendMessage;
            }
        }

        void server_MessageSent(IOMessage msg)
        {
            if (msg != null)
                pendingMessages.Enqueue(msg);
        }

        void IClientEngine.LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphicsDevice);

            //load default content (textures,fonts,animations)
            Content = new AssetList(graphicsDevice);
            Content.LoadDefaultContent(ContentManager);

            //shaders
            shaders = new ShaderContainer(ContentManager);

            // ??? 
            graphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.CullClockwiseFace,
            };

            tryInitGame();
        }

        void IClientEngine.Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //update the local server
            if (server != null)
                server.UpdateServer(msElapsed);

            //Parse its messages
            IOMessage msg;
            while (pendingMessages.TryDequeue(out msg))
                parseMessage(msg);

            if (isConnected)
            {
                Ticker.Default.Update(msElapsed);

                //input
                KeyboardInfo.Update(msElapsed);
                MouseInfo.Update(msElapsed);

                //pan camera to the hero
                if(Game.MainHero != null)
                    Screen.MoveCamera(Game.MainHero.Position);

                //ui, objects
                Game.Update(msElapsed);

                //debug
                updateDebugStats(msElapsed);
            }
        }

        void IClientEngine.Draw(GameTime gameTime)
        {

            if (!isConnected)
            {
                drawConnectingScreen();
                return;
            }


            if (server != null)
            {
                graphicsDevice.Clear(Color.Black);
                graphicsDevice.SetRenderTarget(null);
                graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                // 1. draw terrain
                Game.DrawTerrain();

                // 2. draw gameobjects
                Game.DrawObjects();

                if (!isDesignMode)
                {
                    // 3. draw shadow layer
                    var shader = shaders.FogOfWar;
                    if (shader != null && Game.MainHero != null)
                    {
                        var destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, Screen.Size.X, Screen.Size.Y);
                        var tint = Color.White;
                        var tex = Content.Textures.Blank;

                        shader.Parameters["TexSize"].SetValue(Screen.GameSize.ToVector2());
                        shader.Parameters["SightRange"].SetValue((float)Game.MainHero.VisionRange);

                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                            SamplerState.PointClamp, DepthStencilState.DepthRead,
                            RasterizerState.CullNone, shader);

                        spriteBatch.Draw(tex, destRect, tint);

                        spriteBatch.End();
                    }

                    // 4. interface
                    Game.DrawUi();

                    //debug
                    if (ShowDebugStats)
                    {
                        spriteBatch.Begin();
                        drawDebugStats(spriteBatch);
                        spriteBatch.End();
                    }
                }
            }
        }

        void IClientEngine.SetWindowSize(Point sz)
        {
            if (sz.X == 0 || sz.Y == 0 || sz == Screen.Size)
                return;

            Screen.SetWindowSize(sz);
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

                default:
                    Game.HandleMessage(msg);
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
                Content.LoadScenarioContent(ContentManager, msg);
            }
        }

        #region Drawing Methods

        void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = Content.Fonts.LargeFont;
            var pos = new Point(Screen.Size.X / 10);

            graphicsDevice.SetRenderTarget(null);
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
            var heroPos = Game.MainHero?.Position;

            debugString = string.Join("\n", new[]
            {
                $"FPS: {curFps:00}",

                $"Mouse UI: {mpUi:0.00}",
                $"Mouse InGame: {mpGame:0.00}",
                $"Hero X/Y: {heroPos:0.00}",

                $"UI Hover: {Control.HoverControl.GetType().Name}",
                $"UI Focus: {Control.FocusControl?.GetType().Name }",

                server.GetDebugString(),
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
