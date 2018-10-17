using Shanism.Client.Game.Views;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.Game
{
    /// <summary>
    /// The entry game class that starts the <see cref="GameView"/>. 
    /// Extends the <see cref="Game"/> class.
    /// </summary>
    class ShanismGame : ShanismClient
    {

        MainMenuView mainScreen;

        GameSettings settings => Settings.Current;


        public string StartupMap { get; set; }

        public ShanismGame()
        {
            Window.Title = "Shanism";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += onWindowSizeChanged;

            Graphics.HardwareModeSwitch = false;
            IsMouseVisible = true;
        }


        protected override void LoadGame()
        {
            base.LoadGame();

            // hacks
            GameHelper.ExitGame = Exit;

            initSettings();
            settings.Saved += (s) => applySettings();
        }

        public void StartGame(IClientReceptor receptor)
        {

        }

        protected override void OnGameLoaded()
        {
            Graphics.ApplyChanges();

            // create main view
            mainScreen = new MainMenuView();
            Views.SetMain(mainScreen);

            // and/or auto-start a map
            if (!string.IsNullOrWhiteSpace(StartupMap))
                Views.Push(new SinglePlayer { StartupMap = StartupMap });
        }

        void onWindowSizeChanged(object sender, EventArgs e)
        {
            if (HasLoaded && settings.WindowBounds != Window.ClientBounds.ToRect())
            {
                settings.WindowBounds = Window.ClientBounds.ToRect();
                settings.Save();
            }
        }

        // run on initial settings load
        void initSettings()
        {
            Keyboard.SetKeybinds(settings.Keybinds.BoundActions);

            // position & size
            Window.Position = settings.WindowBounds.Position.ToXnaPoint();
            Graphics.PreferredBackBufferWidth = settings.WindowBounds.Width;
            Graphics.PreferredBackBufferHeight = settings.WindowBounds.Height;

            applySettings();
        }

        // run on settings change
        void applySettings()
        {
            // graphics
            Graphics.IsFullScreen = settings.FullScreen;
            Graphics.SynchronizeWithVerticalRetrace = settings.VSync;
            TargetFps = settings.MaxFps.Clamp(30, 300);

            // screen
            Screen.Game.SetRenderScale(settings.RenderScale);
        }
    }
}
