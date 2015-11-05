using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IO;
using Client.UI;
using System.Collections.Generic;
using Client.Sprites;
using Client.Properties;
using MovementState = IO.Common.MovementState;
using Client.Map;
using Client.Controls;
using Client.Textures;
using IO.Message.Client;
using IO.Message.Server;
using IO.Message;
using IO.Objects;

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

        /// <summary>
        /// The main UI window. 
        /// </summary>
        UiManager mainInterface;

        /// <summary>
        /// Gets the server this client is connected to. 
        /// </summary>
        public IReceptor Server { get; private set; }

        /// <summary>
        /// Gets the current state of the game. 
        /// </summary>
        public GameStatus GameState { get; private set; } = GameStatus.Loading;


        public readonly string PlayerName;
        

        public string LoadState { get; private set; }

        Rectangle lastWindowBounds;

        /// <summary>
        /// The guy that handles objects. 
        /// </summary>
        ObjectGod ObjectManager = new ObjectGod();

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
            get { return ObjectManager.MainHero?.Position ?? IO.Common.Vector.Zero; }
        }


        public MainGame(string playerName)
            : base()
        {
            PlayerName = playerName;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        

        public void SetEngine(IEngine engine)
        {
            if (Server != null)
                throw new InvalidOperationException("Already got a server!");

            //request to join lel
        }

        public void SetReceptor(IReceptor server)
        {
            Server = server;

            Server.ObjectSeen += server_ObjectSeen;
            Server.ObjectUnseen += server_ObjectUnseen;
            Server.MainHeroChanged += server_MainHeroChanged;
            Server.MapChunkReceived += server_MapChunkReceived;
            Server.HandshakeReplied += server_ConnectionChanged;
        }

        void server_ConnectionChanged(HandshakeReplyMessage msg)
        {
            IsConnected = msg.Success;
        }

        void server_MapChunkReceived(MapReplyMessage msg)
        {
            MapManager.HandleMapReply(msg);
        }

        void server_MainHeroChanged(PlayerStatusMessage msg)
        {
            ObjectManager.SetMainHero(msg.HeroId);
        }

        void server_ObjectUnseen(IGameObject obj)
        {
            ObjectManager.RemoveObject(obj.Guid);
        }

        void server_ObjectSeen(IGameObject obj)
        {
            ObjectManager.AddObject(obj);
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

            mainInterface = new UiManager();
            mainInterface.OnActionPerformed += MainInterface_OnActionPerformed;

            this.MapManager = new MapManager(GraphicsDevice);
            MapManager.ChunkRequested += MapManager_ChunkRequested;

            this.mainInterface.Add(ObjectManager);

            ObjectManager.UnitClicked += (u) =>
            {
                mainInterface.Target = u;
            };

            Screen.Update(graphics, ObjectManager.MainHero);

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
        private void MainInterface_OnActionPerformed(ActionMessage msg)
        {
            ActionActivated(msg);
        }

        /// <summary>
        /// Changes the back buffer size in response to window resize. 
        /// </summary>
        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            var sz = Window.ClientBounds;
            if (sz != lastWindowBounds)
            {
                lastWindowBounds = sz;
                graphics.PreferredBackBufferWidth = sz.Width;
                graphics.PreferredBackBufferHeight = sz.Height;
                graphics.ApplyChanges();
            }
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureCache.LoadContent(Content);
            SpriteFactory.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            mainInterface.TargetHero = ObjectManager.MainHero;

            //update cameraInfo
            Screen.Update(graphics, ObjectManager.MainHero);

            // keyboard
            KeyManager.Update(msElapsed);
            UpdateKeys();

            // object manager
            ObjectManager.Update(msElapsed);
            ObjectManager.SendToBack();

            //user interface
            mainInterface.DoUpdate(msElapsed);

            //terrain
            MapManager.Update(CameraPosition);


            // overrides the hero position to the one we've just received. 
            // this is a problem in local games since the ObjectManager
            // queries a unit's more up-to-date position than in the update. 
            //ObjectManager.LocalHero.CustomLocation = new Vector2((float)x, (float)y);

            base.Update(gameTime);
        }

        MovementState lastMoveState;

        public void UpdateKeys()
        {
            //converts a bool to an int
            Func<bool, int> b2i = (b) => (b ? 1 : 0);

            //movement
            var dx = b2i(KeyManager.IsDown(Keybind.MoveRight)) - b2i(KeyManager.IsDown(Keybind.MoveLeft));
            var dy = b2i(KeyManager.IsDown(Keybind.MoveDown)) - b2i(KeyManager.IsDown(Keybind.MoveUp));

            var moveState = new MovementState(dx, dy);
            if(moveState != lastMoveState)
            {
                lastMoveState = moveState;
                MovementStateChanged(new MoveMessage(moveState));
            }

            //health bars
            if(KeyManager.IsActivated(Keybind.ShowHealthBars))
                Settings.Default.AlwaysShowHealthBars = !Settings.Default.AlwaysShowHealthBars;


        }


        private double smoothFrameDelay = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if(!IsConnected)
            {
                drawConnectingScreen();
                return;
            }


            if (Server != null)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                //1. draw terrain (basiceffect)
                MapManager.DrawTerrain(CameraPosition);

                //start spritebatch drawing
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.DepthRead, RasterizerState.CullNone);

                // 2. draw interface + gameobjects 
                // TODO: effects
                mainInterface.DoDraw(spriteBatch);

                //draw debug
                drawDebugStats(spriteBatch, gameTime.ElapsedGameTime.TotalMilliseconds);

                //end drawing
                spriteBatch.End();

                //draw shadow
                //Server.
            }

            base.Draw(gameTime);
        }


        private void drawConnectingScreen()
        {
            var txt = "Connecting...";
            var font = TextureCache.HugeFont;
            var pos = new IO.Common.Point(Screen.Size.X / 10);

            spriteBatch.Begin();

            font.DrawStringScreen(spriteBatch, txt, Color.DarkRed, pos, 0f, 0f);

            spriteBatch.End();
        }


        private void drawDebugStats(SpriteBatch sb, double msElapsed)
        {
            var mpUi = Screen.ScreenToUi(Mouse.GetState().Position.ToPoint());
            var mpGame = Screen.ScreenToGame(Mouse.GetState().Position.ToPoint());

            //FPS
            const double frameConst = 0.1;
            smoothFrameDelay = smoothFrameDelay * (1 - frameConst) + msElapsed * frameConst;
            var sFps =
                "FPS: " + (1000 / smoothFrameDelay).ToString("00.0");
            TextureCache.FancyFont.DrawStringScreen(spriteBatch, sFps, Color.Goldenrod, new IO.Common.Point(24, 18));

            //UI coordinates of mouse
            var sUiCoord = string.Format(
                "UI X/Y: {0} {1}", mpUi.X.ToString("0.00"), mpUi.Y.ToString("0.00"));
            TextureCache.FancyFont.DrawStringScreen(spriteBatch, sUiCoord, Color.Black, new IO.Common.Point(24, 3 * 24));

            //in-game coordinates of mouse
            var sGameCoord = string.Format(
                "Game X/Y: {0} {1}", mpGame.X.ToString("0.00"), mpGame.Y.ToString("0.00"));
            TextureCache.FancyFont.DrawStringScreen(spriteBatch, sGameCoord, Color.Black, new IO.Common.Point(24, 2 * 24));

        }

    }
}
