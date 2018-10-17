using Ix.Logging;
using Microsoft.Xna.Framework;
using Shanism.Client.Assets;
using Shanism.Client.Components;
using Shanism.Client.IO;
using Shanism.Client.Models.Util;
using Shanism.Client.Systems;
using Shanism.Client.UI;
using Shanism.Client.Views;
using Shanism.Common.Util;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;

namespace Shanism.Client
{
    public abstract class ShanismClient : Game, IClient, IHazDebug
    {

        // FPS of the debug-text
        const int DebugFps = 4;

        readonly SystemList systems;
        readonly LineWriter ourConsoleOut;
        readonly PerfCounter perfCounter = new PerfCounter("Client");
        readonly LoadingScreen loadingScreen;
        readonly FpsLimiter fpsLimiter = new FpsLimiter();

        protected CanvasStarter starter;

        public Log Log => ClientLog.Instance;
        protected GraphicsDeviceManager Graphics { get; }

        public ScreenSystem Screen { get; }
        public KeyboardSystem Keyboard { get; }
        public MouseSystem Mouse { get; }
        public ViewStack Views { get; }

        /// <summary>
        /// The vanilla game content list.
        /// </summary>
        public ContentList DefaultContent { get; private set; }

        //public ContentLoader ContentLoader { get; private set; }

        ConsoleSystem Console { get; set; }
        DebugTextSystem DebugText { get; set; }

        public bool HasLoaded { get; private set; }

        Microsoft.Xna.Framework.Content.ContentManager IClient.ContentLoader => Content;

        /// <summary>
        /// Gets or sets the maximum FPS the game will run at.
        /// May behave wierdly if set above 1000 (the amount of ms in a second).
        /// </summary>
        public float TargetFps
        {
            get => 1000f / fpsLimiter.TargetUpdatePeriod;
            set => fpsLimiter.TargetUpdatePeriod = 1000f / value;
        }

        public ShanismClient()
        {
            // console hook
            ourConsoleOut = new LineWriter();
            System.Console.SetOut(new DuplexWriter(System.Console.Out, ourConsoleOut));

            // setup log
            ClientLog.Init();
            Log.Info("Starting up...");

            // FPS
            TargetFps = 100;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1);
            IsFixedTimeStep = false;

            // window
            Window.Title = "Shanism";
            Window.AllowUserResizing = true;

            // graphics
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false,
                PreferMultiSampling = true,
                GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile.HiDef
            };
            Graphics.ApplyChanges();

            // loading screen
            loadingScreen = new LoadingScreen(LoadGame, onLoadSuccess, onLoadError);

            // game systems
            Screen = new ScreenSystem(Window);
            Screen.WindowSizeChanged += updateBackBuffer;
            Keyboard = new KeyboardSystem(this);
            Mouse = new MouseSystem(this, Screen);
            Views = new ViewStack(this, loadingScreen);
            systems = new SystemList
            {
                Screen, Keyboard, Mouse, Views,
            };
        }


        public void WriteDebug(List<string> lines)
        {
            var perfInfo = perfCounter.GetPerformanceData(1000 / DebugFps);
            var curView = Views.Current;

            perfCounter.Reset();

            lines.Add("\nControls");
            lines.Add($"  Hover: {curView?.ViewHoverControl?.GetType().Name}");
            lines.Add($"  Focus: {curView?.ViewFocusControl?.GetType().Name}");
            lines.Add("");

            lines.Add(perfInfo);
            lines.Add("");

            curView?.WriteDebugStats(lines);
        }

        void onLoadSuccess()
        {
            Log.Info("Go shanist4e!");
            OnGameLoaded();
        }

        void onLoadError()
        {
            Log.Error($"Unable to load game content. Exception reads: {loadingScreen.Exception}");
            OnGameLoaded();
        }

        Text.FontSystem fontSystem;

        /// <summary>
        /// Loads all game assets. 
        /// May (will) be executed on a separate thread.
        /// </summary>
        protected virtual void LoadGame()
        {
            const string ContentDir = "Content";

            Log.Info("Loading content...");

            // config
            var configReader = new ScenarioConfigReader();
            var result = configReader.TryReadFromDisk(ContentDir);
            if (!result.IsSuccessful)
            {
                Log.Error($"Unable to load game content. Game will possibly crash.");
            }

            // content loader
            //var manager = new Microsoft.Xna.Framework.Content.ContentManager(Services, "Content");
            //ContentLoader = new ContentLoader(manager, base.GraphicsDevice, ContentDir);

            // new fonts
            fontSystem = new Text.FontSystem(Screen, GraphicsDevice);
            var fonts = new Text.FontCache(fontSystem);


            // default content
            DefaultContent = new ContentList(Screen, fonts, GraphicsDevice, base.Content, ContentDir, result.Value.Content);

            //TODO: this initializes the UI with the default content
            //      so modding the UI is currently not possible!
            ControlContext.Init(Screen, DefaultContent);

            // canvas
            starter = new CanvasStarter(base.GraphicsDevice, Screen);
            DrawBuffer = new RenderBuffer(base.GraphicsDevice);

            // systems: console, debug
            //systems.Add(Content);
            systems.Add(Console = new ConsoleSystem(this, fonts, ourConsoleOut));
            systems.Add(DebugText = new DebugTextSystem(this, fonts, this, DebugFps));
        }

        protected override void Update(GameTime gameTime)
        {
            // FPS limiter
            var msElapsed = fpsLimiter.ActNow();

            // game still loading?
            if (!HasLoaded)
            {
                loadingScreen.Update(msElapsed);

                HasLoaded |= loadingScreen.HasFinished;
            }

            // update systems
            perfCounter.Start("Update");
            {
                systems.Update(msElapsed);

                fontSystem.Update(msElapsed);
            }
            perfCounter.End();

            base.Update(gameTime);
        }


        void updateBackBuffer()
        {
            Graphics.PreferredBackBufferWidth = Screen.WindowBounds.Width;
            Graphics.PreferredBackBufferHeight = Screen.WindowBounds.Height;
            Graphics.ApplyChanges();
        }

        public RenderBuffer DrawBuffer { get; private set; }

        protected override void Draw(GameTime gameTime)
        {
            if (Views.Current == null)
                throw new Exception();

            if (starter == null)
                throw new Exception();

            perfCounter.Start("Draw");
            {
                Views.Current.Draw(starter);

                Console.Draw(starter);
                DebugText.Draw(starter);
            }
            perfCounter.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Log.Info("Shutdown sequence initiated...");
        }

        /// <summary>
        /// Override to run code when the game's finished loading.
        /// </summary>
        protected virtual void OnGameLoaded() { }

    }
}
