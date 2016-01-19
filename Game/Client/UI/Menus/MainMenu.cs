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
        Button btnKeys, btnOptions, btnExit;


        public MainMenu()
        {
            HasTitleBar = false;
            Location = new Vector(0.75, 0.6);
            Size = new Vector(0.5, 0.6);
            ParentAnchor = AnchorMode.None;

            BackColor = Color.Black.SetAlpha(150);
            Action = GameAction.ToggleMainMenu;

            var distFromEdge = Padding * 3;

            btnKeys = new Button("Keybinds")
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(distFromEdge, distFromEdge),
                Size = new Vector(this.Size.X - 2 * distFromEdge, 0.12),
            };

            btnOptions = new Button("Options")
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(distFromEdge, Padding + btnKeys.Bottom),
                Size = new Vector(this.Size.X - 2 * distFromEdge, 0.12),
            };

            btnExit = new Button("Exit")
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,
                Location = new Vector(distFromEdge, Padding + btnOptions.Bottom),
                Size = new Vector(this.Size.X - 2 * distFromEdge, 0.12),
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
            //lol hacky exit
            Process.GetCurrentProcess().Close();
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
