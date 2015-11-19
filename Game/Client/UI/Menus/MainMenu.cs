using Client.Controls;
using Client.UI.Common;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class MainMenu : Control
    {

        Button 
            btnKeys = new Button("Keybinds"),
            btnOptions = new Button("Options"),
            btnExit = new Button("Quit");

        Button[] buttons;

        public MainMenu()
        {
            this.BackColor = Color.Black.SetAlpha(150);
            this.Visible = false;

            var menuAnchor = 3 * Padding;

            buttons = new[]
            {
                btnKeys,
                btnOptions,
                btnExit,
            };

            var btnSize = new Vector(Size.X - 2 * menuAnchor, (Size.Y - menuAnchor) / buttons.Length - menuAnchor);
            var y = menuAnchor;
            foreach(var btn in buttons)
            {
                this.Add(btn);
                btn.Location = new Vector(menuAnchor, y);
                btn.Size = btnSize;
                y += menuAnchor + btnSize.Y;
            }

            btnExit.MouseUp += btnExit_MouseUp;
        }

        void btnExit_MouseUp(Control arg1, Vector arg2)
        {
            
        }

        public override void Update(int msElapsed)
        {
            if (KeyManager.IsActivated(Keybind.MainMenu))
            {
                //close all windows
                var visibleWindows = Parent.Controls
                    .Where(c => typeof(Window).IsAssignableFrom(c.GetType()) && c.Visible);
                if (visibleWindows.Any())
                {
                    foreach (var w in visibleWindows)
                        w.Visible = false;
                    this.Visible = false;
                }
                else    // otherwise toggle visible. 
                {
                    this.Visible = !this.Visible;
                }
            }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
    }
}
