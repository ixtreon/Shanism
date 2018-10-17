using Microsoft.Xna.Framework.Input;
using Shanism.Client.IO;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Menus;
using Shanism.Client.Views;
using System;
using System.Numerics;
using System.Threading;

namespace Shanism.Client.Game.Views
{
    class MainMenuView : TitleView
    {

        ListPanel flowPanel;

        Button optionsButton;
        Button singlePlayer, multiPlayer, exit;

        ExitDialog exitDialog;
        Control optionsWindow;

        public Vector2 ButtonSize { get; set; } = new Vector2(0.8f, 0.18f);

        protected override void OnReload()
        {
            base.OnReload();

            var panelSize = new Vector2(0.8f, 0.7f);
            var panelPadding = 2 * Control.DefaultPadding;
            var btnSize = new Vector2(panelSize.X - 2 * panelPadding, 0.18f);
            
            //main menu & entries
            Add(flowPanel = new ListPanel(Direction.TopDown, sizeMode: ListSizeMode.ResizeBoth)
            {
                Top = ContentStartY,

                Padding = 2 * Control.DefaultPadding,
                ParentAnchor = AnchorMode.Top,
            });

            flowPanel.Add(singlePlayer  = newButton("Single Player"));
            flowPanel.Add(multiPlayer   = newButton("Multi Player"));
            flowPanel.Add(optionsButton = newButton("Settings"));
            flowPanel.Add(exit          = newButton("Exit"));

            flowPanel.CenterX = true;

            //options & exit
            Add(optionsWindow = new OptionsWindow
            {
                IsVisible = false,
                CanFocus = true,
                CenterBoth = true,
            });

            Add(exitDialog = new ExitDialog());


            singlePlayer.MouseClick += onSinglePlayerClick;
            multiPlayer.MouseClick += onMultiPlayerClick;
            optionsButton.MouseClick += (o, e) => optionsWindow.IsVisible = !optionsWindow.IsVisible;
            exit.MouseClick += (o, e) => exitDialog.IsVisible = true;
            KeyPress += (o, e) => handleKeyPress(e.Keybind);
        }

        protected override void OnShown(EventArgs e)
        {
            SynchronizationContext.SetSynchronizationContext(null);
        }

        protected override void OnHidden(EventArgs e)
        {
            optionsWindow.IsVisible = false;
            exitDialog.IsVisible = false;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        Button newButton(string text)
            => new Button(text) { Size = ButtonSize };

        void handleKeyPress(Keybind k)
        {
            switch(k.Key)
            {
                case Keys.Escape when k.Modifiers == ModifierKeys.None:
                    exitDialog.IsVisible ^= true;
                    break;
            }
        }


        void onMultiPlayerClick(Control sender, MouseButtonArgs e)
        {
            var mpScreen = new MultiPlayer();

            ViewStack.Push(mpScreen);
        }

        void onSinglePlayerClick(Control sender, MouseButtonArgs e)
        {
            ViewStack.Push(new SinglePlayer());
        }

    }
}
