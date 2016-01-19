using Client.Input;
using Client.Map;
using Client.UI;
using IO;
using IO.Message;
using IO.Message.Client;
using IO.Message.Server;
using IO.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using MovementState = IO.Common.MovementState;

namespace Client
{
    /// <summary>
    /// This is the main type for our game
    /// </summary>
    class MainGame : Game, IShanoClient
    {
        //MonoGame stuff
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        RenderTarget2D terrainTexture;
        RenderTarget2D objectsTexture;
        RenderTarget2D interfaceTexture;
        RenderTarget2D shadowTextureA;
        RenderTarget2D shadowTextureB;


        /// <summary>
        /// Gets the server this client is connected to. 
        /// </summary>
        public IReceptor Server { get; private set; }

        /// <summary>
        /// Gets the current state of the game. 
        /// </summary>
        public GameStatus GameState { get; private set; } = GameStatus.Loading;

        /// <summary>
        /// Gets the name of the current player. 
        /// </summary>
        public string PlayerName { get; }


        Rectangle WindowSize;

        /// <summary>
        /// Contains the UI, ObjectGod objects. 
        /// </summary>
        GameManager Game;

        /// <summary>
        /// Manages chunks and sends requests for new ones. 
        /// </summary>
        MapManager MapManager;

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

        string IClient.Name
        {
            get { return PlayerName; }
        }
        #endregion

        public bool IsConnected { get; private set; }

        IO.Common.Vector CameraPosition
        {
            get { return Game.Objects.MainHero?.Position ?? IO.Common.Vector.Zero; }
        }


        public MainGame(string playerName)
        {
            PlayerName = playerName;
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            Window.Title = "ShanoRPG";
        }


        public void SetServer(IReceptor server)
        {
            Server = server;

            Server.ObjectSeen += server_ObjectSeen;
            Server.ObjectUnseen += server_ObjectUnseen;
            server.MessageSent += Server_MessageSent;
        }

        void Server_MessageSent(IOMessage msg)
        {
            switch(msg.Type)
            {
                case MessageType.HandshakeReply:
                    onHandshakeReply((HandshakeReplyMessage)msg);
                    break;

                case MessageType.MapReply:
                    MapManager.HandleMapReply((MapReplyMessage)msg);
                    break;

                case MessageType.PlayerStatusUpdate:
                    onStatusChanged((PlayerStatusMessage)msg);
                    break;
            }
        }

        void onStatusChanged(PlayerStatusMessage msg)
        {
            Game.Objects.SetMainHero(msg.HeroId);
        }

        void onHandshakeReply(HandshakeReplyMessage msg)
        {
            if (!msg.Success)
            {
                IsConnected = false;
                return;
            }

            //get the scenario
            var sc = ScenarioFile.LoadBytes(msg.ScenarioData);

            //unzip
            Client.Content.UnzipContent(msg.ContentData, "Scenario", "Content");

            Content.RootDirectory = "Scenario";
            Client.Content.LoadScenario(Content, sc.Content);

            //start playing!
            IsConnected = true;
        }


        void server_ObjectUnseen(IGameObject obj)
        {
            Game.Objects.RemoveObject(obj.Guid);
        }

        void server_ObjectSeen(IGameObject obj)
        {
            Game.Objects.AddObject(obj);
        }

        /// <summary>
        /// Run at the beginning. 
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();


            GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
            };

            //no vsync
            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
            graphics.ApplyChanges();

            //game manager
            Game = new GameManager();
            Game.ActionPerformed += onActionPerformed;

            //MapManager
            this.MapManager = new MapManager(GraphicsDevice);
            MapManager.ChunkRequested += MapManager_ChunkRequested;


            ////screen center
            //Screen.Update(graphics, ObjectManager.MainHero);

            //window stuff
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window_ClientSizeChanged(null, null);


            GameState = GameStatus.Playing;
            if (GameLoaded != null)
                GameLoaded();
        }



        /// <summary>
        /// Listens to the MapManager requesting chunks and relays them to the server. 
        /// </summary>
        private void MapManager_ChunkRequested(IO.Common.MapChunkId obj)
        {
            MapRequested(new MapRequestMessage(obj));
        }

        /// <summary>
        /// Listens to the interface sending actions and relays them to the server. 
        /// </summary>
        /// <param name="msg"></param>
        private void onActionPerformed(ActionMessage msg)
        {
            ActionActivated(msg);
        }


        bool _noRecurse;
        /// <summary>
        /// Changes the back buffer size in response to window resize. 
        /// </summary>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (_noRecurse) return;
            _noRecurse = true;

            var sz = Window.ClientBounds;
            if (sz != WindowSize && sz.Width > 0 && sz.Height > 0)
            {
                //update graphics size
                WindowSize = sz;
                graphics.PreferredBackBufferWidth = sz.Width;
                graphics.PreferredBackBufferHeight = sz.Height;
                graphics.ApplyChanges();

                //update drawtargets
                terrainTexture = new RenderTarget2D(GraphicsDevice, sz.Width, sz.Height, false, SurfaceFormat.Bgra32SRgb, DepthFormat.None);
                objectsTexture = new RenderTarget2D(GraphicsDevice, sz.Width, sz.Height, false, SurfaceFormat.Bgra32SRgb, DepthFormat.None);
                shadowTextureA = new RenderTarget2D(GraphicsDevice, sz.Width, sz.Height);
                shadowTextureB = new RenderTarget2D(GraphicsDevice, sz.Width, sz.Height);
                interfaceTexture = new RenderTarget2D(GraphicsDevice, sz.Width, sz.Height);
            }

            _noRecurse = false;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Client.Content.LoadDefault(Content);

            Shaders.Load(GraphicsDevice, Content);

        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //update the local server
            Server.UpdateServer(msElapsed);

            if (!IsConnected)
                return;

            //keyboard
            KeyboardInfo.Update(msElapsed);
            UpdateKeys();

            //camera
            Screen.Update(graphics, Game.Objects.MainHero);

            //ui, objects
            Game.Update(msElapsed);

            //terrain
            MapManager.Update(CameraPosition);


            // overrides the hero position to the one we've just received. 
            // this is a problem in local games since the ObjectManager
            // queries a unit's more up-to-date position than in the update. 
            //ObjectManager.LocalHero.CustomLocation = new Vector2((float)x, (float)y);

            base.Update(gameTime);
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

            if (KeyboardInfo.IsActivated(new Keybind(ModifierKeys.Control | ModifierKeys.Shift, Keys.R)))
                Game.ReloadUi();
        }


        private double smoothFrameDelay = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
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
                {
                    MapManager.DrawTerrain(CameraPosition);
                }


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
                        spriteBatch.Draw(terrainTexture, Vector2.Zero, Color.White);
                        spriteBatch.Draw(objectsTexture, Vector2.Zero, Color.White);
                        //spriteBatch.Draw(shadowTextureB, Vector2.Zero, Color.White);
                        spriteBatch.Draw(interfaceTexture, Vector2.Zero, Color.White);
                    });
            }

            base.Draw(gameTime);
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



        void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = Client.Content.Fonts.LargeFont;
            var pos = new IO.Common.Point(Screen.Size.X / 10);

            spriteBatch.Begin();

            font.DrawStringScreen(spriteBatch, txt, Color.DarkRed, pos, 0f, 0f);

            spriteBatch.End();
        }


        void drawDebugStats(SpriteBatch sb, double msElapsed)
        {
            const double frameConst = 0.1;

            // FPS
            smoothFrameDelay = smoothFrameDelay * (1 - frameConst) + msElapsed * frameConst;
            var sFps = "FPS: {0:00}".F(1000 / smoothFrameDelay);
            // mouse
            var mpUi = Screen.ScreenToUi(Mouse.GetState().Position.ToPoint());
            var mpGame = Screen.ScreenToGame(Mouse.GetState().Position.ToPoint());

            var debugString =
                "FPS: {0:00}".F(1000 / smoothFrameDelay) + "\n" +
                "UI X/Y: {0:0.00}".F(mpUi) + "\n" +
                "Hero X/Y: {0:0.00}".F(Game.Objects.MainHero?.Position) + "\n" +
                "Game X/Y: {0:0.00}".F(mpGame) + "\n" + 
                "{0} objects".F(Game.Objects.Controls.Count()) + "\n" +
                "Hover: " + Control.HoverControl.GetType().Name;

            Client.Content.Fonts.FancyFont.DrawStringScreen(sb, debugString, Color.Black, 
                new IO.Common.Point(24, 18), 0, 0);

        }

    }
}
