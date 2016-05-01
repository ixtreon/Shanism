using Client.Input;
using Client.Map;
using Client.Textures;
using Client.UI;
using IO;
using IO.Common;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using IO.Objects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScenarioLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Client
{
    /// <summary>
    /// Contains all the drawing and update logic of the client. 
    /// </summary>
    class ClientEngine : IClientEngine
    {
        public static bool ShowDebugStats = false;

        readonly ConcurrentQueue<IOMessage> pendingMessages = new ConcurrentQueue<IOMessage>();
        
        /// <summary>
        /// Contains the UI, ObjectGod objects. 
        /// </summary>
        GameManager Game;

        /// <summary>
        /// Manages chunks and sends requests for new ones. 
        /// </summary>
        MapManager MapManager;


        /// <summary>
        /// The average time to render a frame. 
        /// </summary>
        double timeToRender = 0;

        string playerName;

        IReceptor server;


        #region MonoGame/Rendering Fields
        readonly IGraphicsDeviceService graphics;

        readonly ContentManager ContentManager;

        SpriteBatch spriteBatch;

        readonly RenderTarget2D[] textures = new RenderTarget2D[5];


        RenderTarget2D terrainTexture => textures[0];
        RenderTarget2D objectsTexture => textures[1];
        RenderTarget2D interfaceTexture => textures[2];
        RenderTarget2D shadowTextureA => textures[3];
        RenderTarget2D shadowTextureB => textures[4];
        #endregion


        bool IsConnected { get; set; }

        GraphicsDevice graphicsDevice => graphics.GraphicsDevice;


        #region IShanoClient Implementation
        string IShanoClient.Name => playerName;

        public event Action<MoveMessage> MovementStateChanged;
        public event Action<ActionMessage> ActionActivated;
        public event Action<ChatMessage> ChatMessageSent;
        public event Action<MapRequestMessage> MapRequested;
        public event Action HandshakeInit;
        #endregion


        public ClientEngine(string playerName, IGraphicsDeviceService graphics, ContentManager content)
        {
            ContentManager = content;
            this.playerName = playerName;
            this.graphics = graphics;
        }


        #region IClientEngine implementation

        TextureCache IClientEngine.Textures => Content.Textures;

        void IClientEngine.SetServer(IReceptor server)
        {
            IsConnected = false;
            this.server = server;

            server.MessageSent += (m) =>
            {
                if (m != null)
                    pendingMessages.Enqueue(m);
            };
        }

        void IClientEngine.LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphicsDevice);

            recreateDrawingSurfaces();

            Content.LoadDefaultContent(graphicsDevice, ContentManager);

            Shaders.Load(graphicsDevice, ContentManager);

            graphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
            };

            //MapManager
            MapManager = new MapManager(graphicsDevice);
            MapManager.ChunkRequested += 
                (chunkId) => MapRequested?.Invoke(new MapRequestMessage(chunkId));

            //game manager
            Game = new GameManager();
            Game.MovementStateChanged += (ms) => MovementStateChanged(new MoveMessage(ms));
            Game.ActionPerformed += (act) => ActionActivated(act);
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

            if (!IsConnected)
                return;

            //keyboard
            KeyboardInfo.Update(msElapsed);

            //camera
            Screen.SetCamera(null, Game.Objects.MainHero?.Position);

            //ui, objects
            Game.Update(msElapsed);

            //terrain
            MapManager.Update(msElapsed);
        }

        void IClientEngine.Draw(GameTime gameTime)
        {

            if (!IsConnected)
            {
                drawConnectingScreen();
                return;
            }


            if (server != null)
            {
                // 1. draw terrain
                graphicsDevice.SetRenderTarget(terrainTexture);
                graphicsDevice.Clear(Color.Transparent);
                graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                MapManager.DrawTerrain();


                // 2. draw gameobjects
                startDrawing(spriteBatch, objectsTexture,
                    clearColor: Color.Transparent,
                    drawCode: () =>
                    {
                        Game.Objects.Draw(spriteBatch);
                    });

                // 3. draw shadow layer
                if (Shaders.effect != null)
                {
                    Shaders.effect.Parameters["TexSize"].SetValue(Screen.InGameSize.ToVector2());
                    Shaders.effect.Parameters["SightRange"].SetValue((float)(Game.Objects.MainHero?.VisionRange ?? 10000));
                    startDrawing(spriteBatch, shadowTextureA,
                        clearColor: Color.Transparent,
                        blend: BlendState.AlphaBlend,
                        shader: Shaders.effect,
                        drawCode: () =>
                        {
                            spriteBatch.Draw(Content.Textures.Blank, new Microsoft.Xna.Framework.Rectangle(0, 0, Screen.Size.X, Screen.Size.Y), Color.Red);
                        });
                }

                // 4. draw interface + debug
                startDrawing(spriteBatch, interfaceTexture,
                    clearColor: Color.Transparent,
                    blend: BlendState.AlphaBlend,
                    drawCode: () =>
                    {
                        Game.Interface.Draw(spriteBatch);
                        if(ShowDebugStats)
                            drawDebugStats(spriteBatch, gameTime.ElapsedGameTime.TotalMilliseconds);
                    });



                // 5. reset surface, put all textures on
                startDrawing(spriteBatch, null,
                    clearColor: Color.Black,
                    blend: BlendState.NonPremultiplied,
                    drawCode: () =>
                    {
                        spriteBatch.Draw(terrainTexture, Microsoft.Xna.Framework.Vector2.Zero, Color.White);
                        spriteBatch.Draw(objectsTexture, Microsoft.Xna.Framework.Vector2.Zero, Color.White);
                        spriteBatch.Draw(shadowTextureA, Microsoft.Xna.Framework.Vector2.Zero, Color.White);
                        spriteBatch.Draw(interfaceTexture, Microsoft.Xna.Framework.Vector2.Zero, Color.White);
                    });
            }
        }

        void IClientEngine.SetWindowSize(Point sz)
        {
            if (sz.X * sz.Y == 0 || sz == Screen.Size)
                return;

            Screen.SetCamera(sz);

            recreateDrawingSurfaces();
        }

        void IClientEngine.SetCameraParams(Vector? cameraPos, IEntity lockedEntity, Vector? windowSz)
            => Screen.SetCamera(null, cameraPos, lockedEntity, windowSz);

        void IClientEngine.ToggleUI(bool visible)
            => Game.Interface.IsVisible = visible;

        Vector IClientEngine.GameToScreen(Vector gamePos)
            => Screen.GameToScreen(gamePos);

        Vector IClientEngine.ScreenToGame(Vector screenPos)
            => Screen.ScreenToGame(screenPos);

        #endregion

        void recreateDrawingSurfaces()
        {
            foreach (var i in Enumerable.Range(0, textures.Length))
            {
                textures[i]?.Dispose();
                textures[i] = new RenderTarget2D(graphicsDevice, Screen.Size.X, Screen.Size.Y);
            }
        }

        void parseMessage(IOMessage msg)
        {
            MapManager.HandleMessage(msg);
            Game.Objects.HandleMessage(msg);

            switch (msg.Type)
            {
                case MessageType.HandshakeReply:
                    onHandshakeReply((HandshakeReplyMessage)msg);
                    break;

                case MessageType.DamageEvent:
                    var dmgEv = (DamageEventMessage)msg;
                    Game.FloatingText.AddDamageLabel(dmgEv);
                    break;
            }
        }

        #region Event Handlers

        void onHandshakeReply(HandshakeReplyMessage msg)
        {
            IsConnected = msg.Success;
            if (IsConnected)
            {
                //load content n start playing!
                Content.LoadScenarioContent(ContentManager, msg);
            }
        }

        #endregion

        #region Drawing Methods

        void startDrawing(SpriteBatch sb, RenderTarget2D target,
            Color? clearColor = null,
            Effect shader = null,
            Action drawCode = null,
            BlendState blend = null)
        {
            graphicsDevice.SetRenderTarget(target);
            if (clearColor.HasValue)
                graphicsDevice.Clear(clearColor.Value);

            sb.Begin(SpriteSortMode.Deferred, blend ?? BlendState.NonPremultiplied,
                SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone, shader);

            drawCode?.Invoke();

            sb.End();
        }

        void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = Client.Content.Fonts.LargeFont;
            var pos = new Point(Screen.Size.X / 10);

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            font.DrawStringPx(spriteBatch, txt, Color.DarkRed, pos, 0f, 0f);
            spriteBatch.End();
        }

        void drawDebugStats(SpriteBatch sb, double msElapsed)
        {
            const double frameConst = 0.1;

            // FPS
            timeToRender = timeToRender * (1 - frameConst) + msElapsed * frameConst;
            var sFps = "FPS: {0:00}".F(1000 / timeToRender);
            // mouse
            var mpUi = Screen.ScreenToUi(Mouse.GetState().Position.ToPoint());
            var mpGame = Screen.ScreenToGame(Mouse.GetState().Position.ToPoint());


            var debugString = new[]
            {
                "FPS: {0:00}".F(1000 / timeToRender),
                "UI X/Y: {0:0.00}".F(mpUi),
                "Hero X/Y: {0:0.00}".F(Game.Objects.MainHero?.Position),
                "Game X/Y: {0:0.00}".F(mpGame),
                "{0} objects".F(Game.Objects.Controls.Count()),
                "Hover: " + Control.HoverControl.GetType().Name,
                "Focus: " + Control.FocusControl?.GetType().Name,
                server.GetPerfData(),

            }.Aggregate((a, b) => a + '\n' + b);

            Content.Fonts.NormalFont.DrawStringPx(sb, debugString, Color.Goldenrod,
                new Point(24, 18), 0, 0);

        }

        #endregion
    }
}
