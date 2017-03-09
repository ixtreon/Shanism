using Shanism.Client.Input;
using Shanism.Client.UI.Menus.Keybinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Client.UI.Menus
{
    class KeybindsMenu : Window
    {

        readonly Label lblKeybinds;
        readonly Label lblActionBars;
        readonly KeybindPanel keybinds;
        readonly Button btnResetKeybinds;
        readonly Button btnOk;
        readonly Button btnCancel;

        public KeybindsMenu()
        {
            MinimumSize = new Vector(1.6, 1.0);
            Size = new Vector(1.6, 1.0);
            Location = (new Vector(2, 1)- Size) / 2;
            ParentAnchor = AnchorMode.None;
            TitleText = "Keybinds";

            var lblPadding = 3 * Padding;
            var labelFont = Content.Fonts.NormalFont;
            var lblWidth = Size.X - 3 * lblPadding - Button.DefaultSize.X;

            lblKeybinds = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right,
                Location = new Vector(0, TitleHeight) + lblPadding,

                Width = lblWidth,
                LineHeight = 3,

                Size = new Vector(lblWidth, labelFont.HeightUi * 3 + 2 * Padding),
                AutoSize = false,

                Font = Content.Fonts.NormalFont,
                Text = "Select the action you wish to assign, then press the button/s you want to bind it to. Press Esc at any time to cancel. ",
            };

            btnResetKeybinds = new Button
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,
                Location = new Vector(lblPadding + lblWidth, TitleHeight + labelFont.HeightUi) + lblPadding,

                Font = Content.Fonts.NormalFont,
                Text = "Reset",
                ToolTip = "Reset all keybindings to their default values. ",
            };

            keybinds = new KeybindPanel
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right | AnchorMode.Bottom,
                Location = new Vector(0, lblKeybinds.Bottom) + Padding,
                Size = new Vector(Size.X - 2 * Padding, 0.4),

                BackColor = BackColor.Darken(20),
            };

            lblActionBars = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Bottom,
                Location = new Vector(lblKeybinds.Left, keybinds.Bottom + lblPadding),
                Size = new Vector(Size.X - 2 * lblPadding, lblKeybinds.Size.Y),
                AutoSize = false,

                Font = Content.Fonts.NormalFont,
                Text = "To change the keybind for an action bar button, hover over it using the mouse and press the button/s you want to bind it to. NYI. ",
            };

            btnCancel = new Button
            {
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,
                Location = Size - Button.DefaultSize - 2 * Padding,

                Font = Content.Fonts.NormalFont,
                Text = "Cancel",
                ToolTip = "Discards all changes made so far. ",
            };

            btnOk = new Button
            {
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,
                Location = btnCancel.Location - new Vector(2 * Padding + Button.DefaultSize.X, 0),

                Font = Content.Fonts.NormalFont,
                Text = "Accept",
                ToolTip = "Saves all changed keybindings. ",
            };

            btnResetKeybinds.MouseClick += (e) => resetKeybindings();
            btnCancel.MouseClick += (e) => discardAndHide();
            btnOk.MouseClick += (e) => saveAndHide();

            Add(lblKeybinds);
            Add(btnResetKeybinds);
            Add(keybinds);
            Add(lblActionBars);

            Add(btnCancel);
            Add(btnOk);

            KeyPressed += onKeyPressed;
        }

        void onKeyPressed(Keybind kb)
        {
            var bar = HoverControl as SpellBarButton;
            if (bar != null)
            {
                Settings.Current.Keybinds[bar.BarId, bar.ButtonId] = kb;
            }
        }

        protected override void OnUpdate(int msElapsed)
        {

        }

        protected override void OnClosed()
        {
            Settings.Reload();
        }

        void saveAndHide()
        {
            Settings.Current.Save();
            IsVisible = false;
        }

        void discardAndHide()
        {
            Settings.Reload();
            IsVisible = false;
        }

        void resetKeybindings()
        {
            Settings.Current.ResetKeybinds();
        }
    }
}
