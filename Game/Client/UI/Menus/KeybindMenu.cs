using Client.Input;
using Client.UI.Common;
using Client.UI.Menus.Keybinds;
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

        readonly Label lblKeybinds;
        readonly Label lblActionBars;
        readonly KeybindPanel keybinds;
        readonly Button btnResetKeybinds;

        public KeybindMenu()
        {
            Location = new Vector(0.5, 0.5);
            Size = new Vector(1.6, 1.0);
            ParentAnchor = AnchorMode.None;
            TitleText = "Keybinds";

            WindowClosed += keybindMenu_WindowClosed;


            var lblPadding = 3 * Padding;
            var labelFont = Content.Fonts.NormalFont;
            var btnSize = new Vector(0.15, 0.06);
            var lblWidth = Size.X - 3 * lblPadding - btnSize.X;

            lblKeybinds = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
                Location = new Vector(0, TitleHeight) + lblPadding,
                Size = new Vector(lblWidth, labelFont.UiHeight * 3 + 2 * Padding),
                AutoSize = false,

                Font = Content.Fonts.NormalFont,
                Text = "Click on the action you wish to change, and then press the button that will activate it. Press Esc at any time to cancel. ",
            };

            btnResetKeybinds = new Button
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,
                Location = new Vector(lblPadding + lblWidth, TitleHeight + labelFont.UiHeight) + lblPadding,
                Size = btnSize,

                Font = Content.Fonts.NormalFont,
                Text = "Reset",
                ToolTip = "Reset all keybindings to default",
                BackColor = BackColor.Darken(20),
            };

            keybinds = new KeybindPanel
            {
                Location = new Vector(0, lblKeybinds.Bottom) + Padding,
                Size = new Vector(Size.X - 2 * Padding, 0.4),

                BackColor = BackColor.Darken(20),
            };

            keybinds.InitKeybindLabels();

            lblActionBars = new Label
            {
                ParentAnchor = AnchorMode.All,
                Location = new Vector(lblKeybinds.Left, keybinds.Bottom + lblPadding),
                Size = lblKeybinds.Size,
                AutoSize = false,

                Font = Content.Fonts.NormalFont,
                Text = "To change the keybind for an action bar button just hover over it and press the corresponding keyboard button. ",
            };


            Add(lblKeybinds);
            Add(btnResetKeybinds);
            Add(keybinds);
            Add(lblActionBars);
        }

        void keybindMenu_WindowClosed(Window obj)
        {
            //remove on close
            Parent?.Remove(this);
        }
    }
}
