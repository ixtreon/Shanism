using Client.Input;
using Client.Input;
using Client.UI.Common;
using Client.UI.Menus;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class MainMenu : Window
    {
        const int NButtons = 3;

        Button btnKeys;
        Button btnOptions;
        Button btnExit;


        public MainMenu()
        {
            const double edgeAnchor = Padding * 3;
            var btnSize = new Vector(0.45, 0.14);

            HasTitleBar = false;
            Location = new Vector(0.75, 0.6);
            Size = new Vector(btnSize.X, NButtons * (btnSize.Y + Padding) - Padding) + 2 * edgeAnchor;
            ParentAnchor = AnchorMode.None;

            BackColor = Color.Black.SetAlpha(150);
            Action = GameAction.ToggleMainMenu;


            btnKeys = new Button("Keybinds")
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(edgeAnchor, edgeAnchor),
                Size = btnSize,
            };

            btnOptions = new Button("Options")
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(edgeAnchor, Padding + btnKeys.Bottom),
                Size = btnSize,
            };

            btnExit = new Button("Exit")
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(edgeAnchor, Padding + btnOptions.Bottom),
                Size = btnSize,
            };


            Add(btnKeys);
            Add(btnOptions);
            Add(btnExit);

            btnKeys.MouseUp += btnKeys_MouseUp;
            btnOptions.MouseUp += btnOptions_MouseUp;
            btnExit.MouseUp += btnExit_MouseUp;
        }

        void btnOptions_MouseUp(MouseButtonEvent obj)
        {

        }

        void btnKeys_MouseUp(MouseButtonEvent e)
        {
            Hide();

            if (Parent.Controls.OfType<KeybindMenu>().Any()) return;

            var kbMenu = new KeybindMenu { Visible = true };
            Parent.Add(kbMenu);
        }

        void btnExit_MouseUp(MouseButtonEvent e)
        {
            ExitHelper.Exit();
        }

        protected override void OnUpdate(int msElapsed)
        {
            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);
        }
    }
}
