using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.GameScreens;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;
using Shanism.Client.UI;

namespace Shanism.Client
{
    /// <summary>
    /// The entry game class that starts the <see cref="ClientEngine"/>. 
    /// Extends the <see cref="Game"/> class.
    /// </summary>
    class ClientGame : ShanoGame, IClientInstance
    {
        ClientEngine inGameScreen;
        MainMenu mainMenuScreen;

        public ClientGame()
        {
            Console.WriteLine("Starting up...");
            FinishLoading += () => Console.WriteLine("Boom! Let's play");

            graphics.HardwareModeSwitch = false;

            //setup MonoGame vars
            IsMouseVisible = true;

            Window.Title = "Shanism";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (o, e) => inGameScreen?.RecreateBuffer();
        }


        protected override void Initialize()
        {
            base.Initialize();

            //game
            Keyboard.SetKeybinds(Settings.Current.Keybinds.BoundActions);
            Screen.SetRenderScale(Settings.Current.RenderScale);
            Screen.SetWindowSize(Window.ClientBounds.Size.ToPoint());

            //hacks
            GameHelper.QuitToTitle += GameHelper_QuitToTitle;
            GameHelper.ExitGame += Exit;
            GameHelper.RestartScenario += () => inGameScreen.RestartScenario();

            // init screens
            inGameScreen = new ClientEngine(this);
            mainMenuScreen = new MainMenu(this);
            mainMenuScreen.GameStarted += mainMenuScreen_GameStarted;

            //settings
            Settings.Saved += (s) => reloadGraphicsSettings();
            reloadGraphicsSettings();

            CurrentScreen = mainMenuScreen;
        }

        void GameHelper_QuitToTitle()
        {
            inGameScreen.Disconnect();
            CurrentScreen = mainMenuScreen;
        }

        void mainMenuScreen_GameStarted(IShanoEngine engine)
        {
            IReceptor receptor;
            if (!inGameScreen.TryConnect(engine, "???", out receptor))
                throw new Exception("Unable to connect to the local server!");

            engine.StartPlaying(receptor);

            CurrentScreen = inGameScreen;
        }

        void reloadGraphicsSettings()
        {
            //vsync
            IsFixedTimeStep = Settings.Current.VSync;
            graphics.SynchronizeWithVerticalRetrace = Settings.Current.VSync;

            //full screen
            graphics.IsFullScreen = Settings.Current.FullScreen;

            //render scale
            Screen.SetRenderScale(Settings.Current.RenderScale);

            //apply
            inGameScreen.RecreateBuffer();
            graphics.ApplyChanges();
        }
    }
}
