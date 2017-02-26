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

        public MainMenu(GraphicsDevice device, ContentList content)
            : base(device, content)
        {
            //main menu & entries
            Root.Add(flowPanel = new FlowPanel
            {
                Top = ContentStartY,

                AutoSize = true,
                BackColor = Color.Black.SetAlpha(100),

                ParentAnchor = AnchorMode.None,
            });

            flowPanel.Add(singlePlayer = new Button("Single Player")
            {
                Size = btnSize,
            });

            flowPanel.Add(multiPlayer = new Button("Multi Player")
            {
                Size = btnSize,
            });

            flowPanel.Add(settings = new Button("Settings")
            {
                Size = btnSize,
            });

            flowPanel.Add(exit = new Button("Exit")
            {
                Size = btnSize,
            });

            flowPanel.CenterX();

            //options & exit
            Root.Add(optionsWindow = new OptionsWindow
            {
                IsVisible = false,
                CanFocus = true,
            });
            optionsWindow.CenterBoth();

            Root.Add(exitDialog = new ConfirmExit());


            singlePlayer.MouseClick += SinglePlayer_MouseClick;
            multiPlayer.MouseClick += MultiPlayer_MouseClick;
            settings.MouseClick += (e) => optionsWindow.IsVisible = !optionsWindow.IsVisible;
            exit.MouseClick += (e) => exitDialog.IsVisible = true;
            Root.KeyPressed += Root_KeyPressed;
        }

        public override void Shown()
        {
            SynchronizationContext.SetSynchronizationContext(null);
        }



        void Root_KeyPressed(Input.Keybind kb)
        {
            if (kb.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
                exitDialog.IsVisible = true;

        }

        void MultiPlayer_MouseClick(Input.MouseButtonArgs e)
        {
            var mpScreen = new MultiPlayer(GraphicsDevice, Content);
            mpScreen.GameStarted += onGameStarted;

            SetScreen(mpScreen);
        }

        void SinglePlayer_MouseClick(Input.MouseButtonArgs e)
        {
            var spScreen = new SinglePlayer(GraphicsDevice, Content);
            spScreen.GameStarted += onGameStarted;

            SetScreen(spScreen);
        }

        void onGameStarted(IShanoEngine engine)
        {
            ResetSubScreen();
            StartGame(engine);
        }

        protected override void SetScreen(UiScreen newScreen)
        {
            optionsWindow.IsVisible = false;
            exitDialog.IsVisible = false;

            base.SetScreen(newScreen);
        }

    }
}
