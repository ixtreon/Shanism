using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.GameScreens;
using Shanism.Client.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client
{
    /// <summary>
    /// The entry game class that starts the <see cref="ClientEngine"/>. 
    /// Extends the <see cref="Microsoft.Xna.Framework.Game"/> class.
    /// </summary>
    class ClientGamez : Game, IClientInstance
    {
        GraphicsDeviceManager graphics;

        Rectangle _windowSize;
        bool _stopResizeRecurse;
        bool _isLoaded;


        ClientEngine inGameScreen;
        MainMenu mainMenuScreen;

        GameScreen currentGameScreen;


        #region IClientInstance implementation

        public IClientEngine GameScreen => inGameScreen;

        #endregion


        public ClientGamez()
        {
            Console.WriteLine("Starting up...");

            graphics = new GraphicsDeviceManager(this);
            graphics.HardwareModeSwitch = false;

            Window.Title = "ShanoRPG";

            // game screens
            inGameScreen = new ClientEngine(graphics, Content);

        }


        /// <summary>
        /// Run at the beginning. 
        /// </summary>
        protected override void Initialize()
        {
            Console.WriteLine("Initializing...");

            base.Initialize();

            GameHelper.SetGame(this);
            GameHelper.QuitToTitle += GameHelper_QuitToTitle;


            //setup MonoGame vars
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.None };

            Screen.SetWindowSize(Window.ClientBounds.Size.ToPoint());


            // game screens
            mainMenuScreen = new MainMenu(graphics.GraphicsDevice);
            mainMenuScreen.GameStarted += mainMenuScreen_GameStarted;

            setScreen(mainMenuScreen);


            //reload graphics settings from settings + hook settings saved
            reloadGraphicsEngine();
            Settings.Saved += (s) => reloadGraphicsEngine();

        }

        void setScreen(GameScreen scr)
        {
            currentGameScreen = scr;
            scr.Shown();
        }

        void GameHelper_QuitToTitle()
        {
            setScreen(mainMenuScreen);
        }

        void mainMenuScreen_GameStarted(Common.IShanoEngine engine)
        {
            Common.IReceptor receptor;
            if (!inGameScreen.TryConnect(engine, "Pesho i Gosho", out receptor))
                throw new Exception("Unable to connect to the local server!");

            engine.StartPlaying(receptor);

            setScreen(inGameScreen);
        }

        void recreateDrawBuffer()
        {
            var s = Screen.RenderSize;
            var w = (int)(Window.ClientBounds.Width * s);
            var h = (int)(Window.ClientBounds.Height * s);

            if (w == Window.ClientBounds.Width && h == Window.ClientBounds.Height)
                inGameScreen.RenderTarget = null;
            else
                inGameScreen.RenderTarget = new RenderTarget2D(GraphicsDevice, w, h);
        }

        void reloadGraphicsEngine()
        {
            Screen.SetRenderSize(Settings.Current.RenderSize);

            IsFixedTimeStep = Settings.Current.VSync;
            graphics.SynchronizeWithVerticalRetrace = Settings.Current.VSync;
            graphics.IsFullScreen = Settings.Current.FullScreen;

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
                GameScreen.SetWindowSize(sz.Size.ToPoint());

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
            if (!GameScreen.LoadContent())
            {
                //TODO
            }
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!_isLoaded)
            {
                _isLoaded = true;
                Console.WriteLine("Boom! Let's play");
            }

            //input
            MouseInfo.Update(msElapsed, IsActive);
            KeyboardInfo.Update(msElapsed, IsActive);

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
