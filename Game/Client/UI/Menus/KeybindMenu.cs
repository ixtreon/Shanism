using Client.Input;
using Client.UI.Common;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI.Menus
{
    class KeybindMenu : Window
    {
        static Vector menuSize = new Vector(1.0, 0.4);

        readonly Label textLabel = new Label
        {
            ParentAnchor = AnchorMode.All,
            Location = new Vector(0, TitleHeight) + (3 * Padding),
            Size = menuSize - Padding * 2,
            AutoSize = false,

            Font = Content.Fonts.NormalFont,
            Text = "To change a button's keybind, hover over it with the mouse and press the button you wish to bind it to",
        };

        public KeybindMenu()
        {
            Location = new Vector(0.5, 0.5);
            Size = menuSize;
            ParentAnchor = AnchorMode.None;

            TitleText = "Keybinds";

            WindowClosed += keybindMenu_WindowClosed;

            Add(textLabel);
        }

        void keybindMenu_WindowClosed(Window obj)
        {
            //remove on close
            Parent?.Remove(this);
        }
    }
}
