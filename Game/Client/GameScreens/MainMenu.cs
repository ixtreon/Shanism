using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using Shanism.Client.UI.Menus;
using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Client.GameScreens
{
    class MainMenu : UiScreen
    {
        static readonly Vector panelSize = new Vector(0.6, 0.7);
        static readonly Vector btnSize = new Vector(panelSize.X - 2 * Control.Padding, 0.15);

        readonly FlowPanel flowPanel;

        readonly Button settings;
        readonly Button singlePlayer, multiPlayer, exit;

        readonly ConfirmExit exitDialog;
        readonly Control optionsWindow;

        public event Action<IShanoEngine> GameStarted;

        public MainMenu(GraphicsDevice device)
            : base(device)
        {
            //main menu & entries
            Root.Add(flowPanel = new FlowPanel
            {
                Width = panelSize.X,

                BackColor = Color.Black.SetAlpha(100),

                ParentAnchor = AnchorMode.None,
            });

            flowPanel.Add(singlePlayer = new Button("Single Player")
            {
                Size = btnSize,
            });
            singlePlayer.MouseUp += SinglePlayer_MouseUp;

            flowPanel.Add(multiPlayer = new Button("Multi Player")
            {
                Size = btnSize,
            });
            multiPlayer.MouseUp += MultiPlayer_MouseUp;

            flowPanel.Add(settings = new Button("Settings")
            {
                Size = btnSize,
            });
            settings.MouseUp += Settings_MouseUp;

            flowPanel.Add(exit = new Button("Exit")
            {
                Size = btnSize,
            });
            exit.MouseUp += Exit_MouseUp;

            flowPanel.AutoSize = true;
            flowPanel.CenterBoth();

            //options & exit
            Root.Add(optionsWindow = new OptionsWindow
            {
                IsVisible = false,
                CanFocus = true,
            });
            optionsWindow.CenterBoth();

            Root.Add(exitDialog = new ConfirmExit());


            Root.KeyPressed += Root_KeyPressed;
            Root.MouseDown += Root_MouseDown;
        }

        public override void Shown()
        {
            SynchronizationContext.SetSynchronizationContext(null);
        }

        private void Root_MouseDown(Input.MouseButtonArgs obj)
        {
            //flowPanel.Location = obj.Position;
        }

        private void Settings_MouseUp(Input.MouseButtonArgs obj)
        {
            optionsWindow.IsVisible = !optionsWindow.IsVisible;
        }

        void Root_KeyPressed(Input.Keybind kb)
        {
            if (kb.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
            {
                exitDialog.IsVisible = true;
            }
        }

        void Exit_MouseUp(Input.MouseButtonArgs obj)
        {
            exitDialog.IsVisible = true;
        }

        void MultiPlayer_MouseUp(Input.MouseButtonArgs e)
        {
            var mpScreen = new MultiPlayer(device);
            mpScreen.GameStarted += onGameStarted;

            SetScreen(mpScreen);
        }

        void SinglePlayer_MouseUp(Input.MouseButtonArgs e)
        {
            var spScreen = new SinglePlayer(device);
            spScreen.GameStarted += onGameStarted;

            SetScreen(spScreen);
        }

        void onGameStarted(IShanoEngine engine)
        {
            ResetSubScreen();
            GameStarted?.Invoke(engine);
        }

        protected override void SetScreen(UiScreen newScreen)
        {
            optionsWindow.IsVisible = false;
            exitDialog.IsVisible = false;

            base.SetScreen(newScreen);
        }

    }
}
