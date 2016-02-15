using Client.Input;
using Client.Map;
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

        Point WindowSize = new Point(800, 480);

        /// <summary>
        /// Contains the UI, ObjectGod objects. 
        /// </summary>
        GameManager Game;

        /// <summary>
        /// Manages chunks and sends requests for new ones. 
        /// </summary>
        MapManager MapManager;

        #region MonoGame/Rendering Fields
        readonly IGraphicsDeviceService graphics;

        readonly ContentManager ContentManager;

        SpriteBatch spriteBatch;

        RenderTarget2D terrainTexture;
        RenderTarget2D objectsTexture;
        RenderTarget2D interfaceTexture;
        RenderTarget2D shadowTextureA;
        RenderTarget2D shadowTextureB;
        #endregion

        /// <summary>
        /// Gets the name of the current player. 
        /// </summary>
        public string PlayerName { get; }

        /// <summary>
        /// Gets the server this client is connected to. 
        /// </summary>
        internal IReceptor Server { get; private set; }

        /// <summary>
        /// Gets the current state of the game. 
        /// </summary>
        public GameStatus GameState { get; private set; } = GameStatus.Loading;

        /// <summary>
        /// Raised when the game has finished loading. 
        /// </summary>
        public event Action GameLoaded;


        #region IGameClient
        public event Action<MoveMessage> MovementStateChanged;
        public event Action<ActionMessage> ActionActivated;
        public event Action<ChatMessage> ChatMessageSent;
        public event Action<MapRequestMessage> MapRequested;
        public event Action HandshakeInit;

        string IShanoClient.Name
        {
            get { return PlayerName; }
        }
        #endregion

        public GraphicsDevice GraphicsDevice { get { return graphics.GraphicsDevice; } }

        public bool IsConnected { get; private set; }

        IO.Common.Vector CameraPosition
        {
            get { return Game.Objects.MainHero?.Position ?? IO.Common.Vector.Zero; }
        }


        public ClientEngine(string playerName, IGraphicsDeviceService graphics, ContentManager content)
        {
            ContentManager = content;
            PlayerName = playerName;
            this.graphics = graphics;


        }


        public void SetServer(IReceptor server)
        {
            Server = server;

            server.MessageSent += (m) =>
            {
                pendingMessages.Enqueue(m);
            };
        }

        readonly ConcurrentQueue<IOMessage> pendingMessages = new ConcurrentQueue<IOMessage>();

        void ParseMessage(IOMessage msg)
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
                    Game.DamageText.AddDamageLabel(dmgEv);
                    break;
            }
        }



        /// <summary>
        /// Listens to the MapManager requesting chunks and relays them to the server. 
        /// </summary>
        void MapManager_ChunkRequested(MapChunkId obj)
        {
            MapRequested?.Invoke(new MapRequestMessage(obj));
        }



        public void WindowSizeChanged(Rectangle bounds)
        {
            if (bounds.Area == 0)
                return;

            WindowSize = bounds.Size;

            terrainTexture?.Dispose();
            objectsTexture?.Dispose();
            shadowTextureA?.Dispose();
            shadowTextureB?.Dispose();
            interfaceTexture?.Dispose();

            //update drawtargets
            terrainTexture = new RenderTarget2D(GraphicsDevice, WindowSize.X, WindowSize.Y, false, SurfaceFormat.Bgra32SRgb, DepthFormat.None);
            objectsTexture = new RenderTarget2D(GraphicsDevice, WindowSize.X, WindowSize.Y, false, SurfaceFormat.Bgra32SRgb, DepthFormat.None);
            shadowTextureA = new RenderTarget2D(GraphicsDevice, WindowSize.X, WindowSize.Y);
            shadowTextureB = new RenderTarget2D(GraphicsDevice, WindowSize.X, WindowSize.Y);
            interfaceTexture = new RenderTarget2D(GraphicsDevice, WindowSize.X, WindowSize.Y);
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Client.Content.LoadDefaultContent(ContentManager);
            Shaders.Load(GraphicsDevice, ContentManager);

            ///old init is new loadcontent
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
            };

            //MapManager
            MapManager = new MapManager(GraphicsDevice);
            MapManager.ChunkRequested += MapManager_ChunkRequested;

            //game manager
            Game = new GameManager();
            Game.ActionPerformed += onActionPerformed;

            ////screen center
            //Screen.Update(graphics, ObjectManager.MainHero);

            GameState = GameStatus.Playing;
            GameLoaded?.Invoke();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //update the local server
            if(Server != null)
                Server.UpdateServer(msElapsed);

            //Parse its messages
            IOMessage msg;
            while (pendingMessages.TryDequeue(out msg))
                ParseMessage(msg);

            if (!IsConnected)
                return;

            //keyboard
            KeyboardInfo.Update(msElapsed);
            UpdateKeys();

            //camera
            Screen.SetCamera(WindowSize, Game.Objects.MainHero?.Position);

            //ui, objects
            Game.Update(msElapsed);

            //terrain
            MapManager.CameraPosition = CameraPosition;
            MapManager.Update(msElapsed);
        }

        MovementState lastMoveState;

        void UpdateKeys()
        {
            //movement
            var dx = Convert.ToInt32(KeyboardInfo.IsDown(GameAction.MoveRight)) - Convert.ToInt32(KeyboardInfo.IsDown(GameAction.MoveLeft));
            var dy = Convert.ToInt32(KeyboardInfo.IsDown(GameAction.MoveDown)) - Convert.ToInt32(KeyboardInfo.IsDown(GameAction.MoveUp));

            var moveState = new MovementState(dx, dy);
            if (moveState != lastMoveState)
            {
                lastMoveState = moveState;
                MovementStateChanged(new MoveMessage(moveState));
            }

            //health bars
            if (KeyboardInfo.IsActivated(GameAction.ShowHealthBars))
                ShanoSettings.Current.QuickButtonPress = !ShanoSettings.Current.QuickButtonPress;

            //reload UI
            if (KeyboardInfo.IsActivated(GameAction.ReloadUi))
                Game.ReloadUi();
        }

        /// <summary>
        /// The (averaged) time to render a frame. 
        /// </summary>
        double timeToRender = 0;

        /// <summary>
        /// Called whenever the game should draw itself.
        /// </summary>
        public void Draw(GameTime gameTime)
        {

            if (!IsConnected)
            {
                drawConnectingScreen();
                return;
            }


            if (Server != null)
            {
                //GraphicsDevice.Clear(Color.DarkBlue);

                // 1. draw terrain
                GraphicsDevice.SetRenderTarget(terrainTexture);
                GraphicsDevice.Clear(Color.Transparent);
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                MapManager.DrawTerrain();


                // 2. draw gameobjects
                StartDrawing(spriteBatch, objectsTexture,
                    clearColor: Color.Transparent,
                    drawCode: () =>
                    {
                        Game.Objects.Draw(spriteBatch);
                    });

                // 3. draw shadow layer
                //todo

                // 4. draw interface + debug
                StartDrawing(spriteBatch, interfaceTexture,
                    clearColor: Color.Transparent,
                    blend: BlendState.AlphaBlend,
                    drawCode: () =>
                    {
                        Game.Interface.Draw(spriteBatch);
                        drawDebugStats(spriteBatch, gameTime.ElapsedGameTime.TotalMilliseconds);
                    });



                // 5. reset surface, put all textures on
                StartDrawing(spriteBatch, null,
                    clearColor: Color.LightBlue,
                    blend: BlendState.NonPremultiplied,
                    drawCode: () =>
                    {
                        spriteBatch.Draw(terrainTexture, Microsoft.Xna.Framework.Vector2.Zero, null, Color.White);
                        spriteBatch.Draw(objectsTexture, Microsoft.Xna.Framework.Vector2.Zero, Color.White);
                        //spriteBatch.Draw(shadowTextureB, Vector2.Zero, Color.White);
                        spriteBatch.Draw(interfaceTexture, Microsoft.Xna.Framework.Vector2.Zero, Color.White);
                    });
            }
        }

        void StartDrawing(SpriteBatch sb, RenderTarget2D target,
            Color? clearColor = null,
            Effect shader = null,
            Action drawCode = null,
            BlendState blend = null)
        {
            GraphicsDevice.SetRenderTarget(target);
            if (clearColor.HasValue)
                GraphicsDevice.Clear(clearColor.Value);
            
            sb.Begin(SpriteSortMode.Deferred, blend ?? BlendState.NonPremultiplied,
                SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone, shader);

            drawCode?.Invoke();

            sb.End();
        }
        

        void onHandshakeReply(HandshakeReplyMessage msg)
        {
            if (!msg.Success)
            {
                IsConnected = false;
                return;
            }

            Content.LoadScenarioContent(ContentManager, msg);

            //start playing!
            IsConnected = true;
        }

        /// <summary>
        /// Listens to the interface sending actions and relays them to the server. 
        /// </summary>
        /// <param name="msg"></param>
        void onActionPerformed(ActionMessage msg)
        {
            ActionActivated(msg);
        }



        void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = Client.Content.Fonts.LargeFont;
            var pos = new Point(Screen.PixelSize.X / 10);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            font.DrawStringScreen(spriteBatch, txt, Color.DarkRed, pos, 0f, 0f);
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
                Server.GetPerfData(),

            }.Aggregate((a, b) => a + '\n' + b);

            Client.Content.Fonts.FancyFont.DrawStringScreen(sb, debugString, Color.DarkViolet,
                new Point(24, 18), 0, 0);

        }

        #region IClientEngine implementation

        public void SetCameraParams(Vector? cameraPos = null, Vector? windowSz = null)
        {
            Screen.SetCamera(null, cameraPos, windowSz);
        }

        public void ToggleUI(bool visible)
        {
            Game.Interface.Visible = visible;
        }

        public Vector GameToScreen(Vector gamePos)
        {
            return Screen.GameToScreen(gamePos);
        }

        public Vector ScreenToGame(Vector screenPos)
        {
            return Screen.ScreenToGame(screenPos);
        }

        #endregion
    }
}
