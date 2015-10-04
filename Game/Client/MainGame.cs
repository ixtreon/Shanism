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

namespace Client
{
    /// <summary>
    /// This is the main type for our game
    /// </summary>
    public class MainGame : Game, IGameClient
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        /// <summary>
        /// The main UI window. 
        /// </summary>
        UiManager mainInterface;

        public IGameReceptor Server { get; private set; }

        public GameStatus GameState { get; private set; } = GameStatus.Loading;


        public readonly string PlayerName;

        public string LoadState { get; private set; }


        /// <summary>
        /// Raised when the game has finished loading. 
        /// </summary>
        public event Action GameLoaded;


        bool HasHero
        {
            get { return localHero != null; }
        }

        string IGameClient.Name
        {
            get { return PlayerName; }
        }

        /// <summary>
        /// Gets the local hero. 
        /// </summary>
        IHero localHero
        {
            get { return Server.MainHero; }
        }


        Rectangle lastWindowBounds;

        private ObjectManager ObjectManager;

        private MapManager MapManager;


        //hack so we can start the game without referencing monogame. duh
        public bool Running
        {
            set
            {
                if (value)
                    Run();
            }
        }

        public MainGame(string playerName)
            : base()
        {
            PlayerName = playerName;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Also accepts a method to update the local server's state. 
        public MainGame(string playerName, IGameReceptor server)
            : this(playerName)
        {
            Server = server;
        }

        public void SetServer(IGameReceptor server)
        {
            if (Server != null)
                throw new InvalidOperationException("Already got a server!");
            Server = server;
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

            this.ObjectManager = new ObjectManager();

            this.MapManager = new MapManager(Server, GraphicsDevice);

            this.mainInterface.Add(ObjectManager);

            ObjectManager.UnitClicked += (u) =>
            {
                mainInterface.Target = u;
            };

            Screen.Update(graphics, Server.CameraPosition, Server.HasHero);

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window_ClientSizeChanged(null, null);
        }

        private void MainInterface_OnActionPerformed(ActionMessage msg)
        {
            Server.RegisterAction(msg);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            var s = this.Window.ClientBounds;
            if (s != lastWindowBounds)
            {
                lastWindowBounds = s;
                graphics.PreferredBackBufferWidth = s.Width;
                graphics.PreferredBackBufferHeight = s.Height;
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
            Server.Update(msElapsed);

            if (!Server.Connected)
                return;

            mainInterface.TargetHero = localHero;

            //update cameraInfo
            Screen.Update(graphics, Server.CameraPosition, Server.HasHero);

            // keyboard
            KeyManager.Update(msElapsed);
            UpdateKeys();

            // object manager
            var objs = Server.VisibleObjects;
            ObjectManager.Update(msElapsed, objs);
            ObjectManager.SendToBack();

            //user interface
            mainInterface.DoUpdate(msElapsed);

            //terrain
            MapManager.Update();


            // overrides the hero position to the one we've just received. 
            // this is a problem in local games since the ObjectManager
            // queries a unit's more up-to-date position than in the update. 
            //ObjectManager.LocalHero.CustomLocation = new Vector2((float)x, (float)y);

            base.Update(gameTime);
        }


        public void UpdateKeys()
        {
            //converts a bool to an int
            Func<bool, int> b2i = (b) => (b ? 1 : 0);

            //movement
            var dx = b2i(KeyManager.IsDown(Keybind.MoveRight)) - b2i(KeyManager.IsDown(Keybind.MoveLeft));
            var dy = b2i(KeyManager.IsDown(Keybind.MoveDown)) - b2i(KeyManager.IsDown(Keybind.MoveUp));
            Server.MovementState = new MovementState(dx, dy);

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

            if(!Server.Connected)
            {
                drawConnectingScreen();
                return;
            }

            if(GameState == GameStatus.Loading)
            {
                GameState = GameStatus.Playing;
                if(GameLoaded != null)
                    GameLoaded();
            }

            if (Server != null)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                //1. draw terrain (basiceffect)
                MapManager.DrawTerrain();

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
            var pos = new Point(Screen.Size.X / 10);

            spriteBatch.Begin();

            font.DrawStringScreen(spriteBatch, txt, Color.DarkRed, pos, 0f, 0f);

            spriteBatch.End();
        }


        private void drawDebugStats(SpriteBatch sb, double msElapsed)
        {
            var mpUi = Screen.ScreenToUi(Mouse.GetState().Position);
            var mpGame = Screen.ScreenToGame(Mouse.GetState().Position);

            //FPS
            const double frameConst = 0.1;
            smoothFrameDelay = smoothFrameDelay * (1 - frameConst) + msElapsed * frameConst;
            var sFps =
                "FPS: " + (1000 / smoothFrameDelay).ToString("00.0");
            TextureCache.FancyFont.DrawStringScreen(spriteBatch, sFps, Color.Goldenrod, new Point(24, 18));

            //UI coordinates of mouse
            var sUiCoord = string.Format(
                "UI X/Y: {0} {1}", mpUi.X.ToString("0.00"), mpUi.Y.ToString("0.00"));
            TextureCache.FancyFont.DrawStringScreen(spriteBatch, sUiCoord, Color.Black, new Point(24, 3 * 24));

            //in-game coordinates of mouse
            var sGameCoord = string.Format(
                "Game X/Y: {0} {1}", mpGame.X.ToString("0.00"), mpGame.Y.ToString("0.00"));
            TextureCache.FancyFont.DrawStringScreen(spriteBatch, sGameCoord, Color.Black, new Point(24, 2 * 24));

        }

    }
}
