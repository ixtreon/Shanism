using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.GameScreens;
using Shanism.Client.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client
{
    /// <summary>
    /// The entry game class that starts the <see cref="ClientEngine"/>. 
    /// Extends the <see cref="Game"/> class.
    /// </summary>
    class ClientGame : Game, IClientInstance
    {
        GraphicsDeviceManager graphics;

        Microsoft.Xna.Framework.Rectangle _windowSize;
        bool _stopResizeRecurse;
        bool _isLoaded;


        ClientEngine gameScreen;
        MainMenu mainMenuScreen;

        GameScreen currentGameScreen;


        /// <summary>
        /// All resources loaded with the game.
        /// </summary>
        ContentList contentList => game.Content;

        /// <summary>
        /// The main game screen structure.
        /// </summary>
        Screen screen => game.Screen;

        /// <summary>
        /// The main game input structure.
        /// </summary>
        GameComponent game;


        #region IClientInstance implementation

        public IClientEngine Engine => gameScreen;

        #endregion


        public ClientGame()
        {
            Console.WriteLine("Starting up...");

            graphics = new GraphicsDeviceManager(this);
            graphics.HardwareModeSwitch = false;

            //setup MonoGame vars
            IsMouseVisible = true;

            Window.Title = "Shanism";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }


        /// <summary>
        /// Run at the beginning. 
        /// </summary>
        protected override void Initialize()
        {
            Console.WriteLine("Initializing...");

            GameHelper.SetGame(this);
            GameHelper.QuitToTitle += GameHelper_QuitToTitle;


            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };

            //IO
            game = new GameComponent(GraphicsDevice, Content);
            screen.SetWindowSize(Window.ClientBounds.Size.ToPoint());

            base.Initialize();
        }

        void setScreen(GameScreen scr)
        {
            currentGameScreen = scr;
            scr.Shown();
        }

        void GameHelper_QuitToTitle()
        {
            gameScreen.Disconnect();
            setScreen(mainMenuScreen);
        }

        void mainMenuScreen_GameStarted(IShanoEngine engine)
        {
            IReceptor receptor;
            if (!gameScreen.TryConnect(engine, "???", out receptor))
                throw new Exception("Unable to connect to the local server!");

            engine.StartPlaying(receptor);

            setScreen(gameScreen);
        }

        void recreateDrawBuffer()
        {
            var windowSize = Window.ClientBounds.Size.ToPoint();

            gameScreen.RecreateBuffer(windowSize);
        }

        void reloadGraphicsEngine()
        {
            //vsync
            IsFixedTimeStep = Settings.Current.VSync;
            graphics.SynchronizeWithVerticalRetrace = Settings.Current.VSync;

            //full screen
            graphics.IsFullScreen = Settings.Current.FullScreen;

            //draw buffer
            screen.SetRenderSize(Settings.Current.RenderSize);
            recreateDrawBuffer();

            graphics.ApplyChanges();
        }


        /// <summary>
        /// Changes the back buffer size in response to window resize. 
        /// </summary>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (_stopResizeRecurse) return;

            _stopResizeRecurse = true;

            var sz = Window.ClientBounds;
            if (sz != _windowSize && sz.Width > 0 && sz.Height > 0)
            {
                _windowSize = sz;

                //drawbuffer
                recreateDrawBuffer();
                Engine.SetWindowSize(sz.Size.ToPoint());

                //actual backbuffer
                graphics.PreferredBackBufferWidth = (int)(sz.Width);
                graphics.PreferredBackBufferHeight = (int)(sz.Height);
                graphics.ApplyChanges();
            }


            _stopResizeRecurse = false;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Console.WriteLine("Loading content...");

            //load default content (textures,fonts,animations)
            if (!contentList.LoadDefault())
            {
                //TODO: content loading failed
            }

            // game screens
            gameScreen = new ClientEngine(game);
            mainMenuScreen = new MainMenu(game);
            mainMenuScreen.GameStarted += mainMenuScreen_GameStarted;

            //reload graphics settings from settings + hook settings saved
            reloadGraphicsEngine();
            Settings.Saved += (s) => reloadGraphicsEngine();

            // set the starting screen
            setScreen(mainMenuScreen);
        }

        const int MaxFps = 30;
        readonly Counter updateCounter = new Counter(1000 / MaxFps);

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;


            //detect first run
            if (!_isLoaded)
            {
                _isLoaded = true;
                Console.WriteLine("Boom! Let's play");
            }

            //if (!IsFixedTimeStep && !updateCounter.Tick(msElapsed))
            //    return;

            //input
            game.Update(msElapsed, IsActive);

            //screen
            currentGameScreen.Update(msElapsed);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            currentGameScreen.Draw();
        }
    }
}
