using Ix.Math;
using Shanism.Client.Game;
using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Menus.Keybinds;
using Shanism.Common;
using System.Numerics;

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
            MinimumSize = new Vector2(1.611f, 1.0f);
            Size = new Vector2(1.611f, 1.0f);
            Location = (new Vector2(2, 1)- Size) / 2;
            ParentAnchor = AnchorMode.None;
            TitleText = "Keybinds";

            var lblPadding = 3 * Padding;
            var labelFont = Content.Fonts.NormalFont;
            var lblWidth = ClientBounds.Width - Padding - Button.DefaultSize.X;

            lblKeybinds = new Label
            {
                BackColor = Color.Green,
                ParentAnchor = AnchorMode.Top | AnchorMode.Horizontal,
                Location = new Vector2(0, DefaultTitleHeight).Offset(lblPadding),
                Font = Content.Fonts.NormalFont,

                Width = lblWidth,
                LineHeight = 3,

                Size = new Vector2(lblWidth, labelFont.Height * 3 + 2 * Padding),

                Text = "Select the action you wish to assign, "
                    + "then press the button/s you want to bind it to. "
                    + "Press Esc at any time to cancel. ",
            };

            btnResetKeybinds = new Button
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,
                Location = new Vector2(lblPadding + lblWidth, DefaultTitleHeight + labelFont.Height).Offset(lblPadding),

                TextFont = Content.Fonts.NormalFont,
                Text = "Reset",
                ToolTip = "Reset all keybindings to their default values. ",
            };

            keybinds = new KeybindPanel
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Top | AnchorMode.Right | AnchorMode.Bottom,
                Location = new Vector2(0, lblKeybinds.Bottom).Offset(Padding),
                Size = new Vector2(ClientBounds.Width, 0.41f),

                BackColor = BackColor.Darken(20),
            };

            lblActionBars = new Label
            {
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Bottom,
                Location = new Vector2(lblKeybinds.Left, keybinds.Bottom + lblPadding),
                Size = new Vector2(Size.X - 2 * lblPadding, lblKeybinds.Size.Y),

                Font = Content.Fonts.NormalFont,
                Text = "To change the keybind for an action bar button, hover over it using the mouse and press the button/s you want to bind it to. NYI. ",
            };

            btnCancel = new Button
            {
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,
                Location = Size - Button.DefaultSize - new Vector2(2 * Padding),

                TextFont = Content.Fonts.NormalFont,
                Text = "Cancel",
                ToolTip = "Discards all changes made so far. ",
            };

            btnOk = new Button
            {
                ParentAnchor = AnchorMode.Bottom | AnchorMode.Right,
                Location = btnCancel.Location - new Vector2(2 * Padding + Button.DefaultSize.X, 0),

                TextFont = Content.Fonts.NormalFont,
                Text = "Accept",
                ToolTip = "Saves all changed keybindings. ",
            };

            btnResetKeybinds.MouseClick += (o, e) => resetKeybindings();
            btnCancel.MouseClick += (o, e) => discardAndHide();
            btnOk.MouseClick += (o, e) => saveAndHide();

            Add(lblKeybinds);
            Add(btnResetKeybinds);
            Add(keybinds);
            Add(lblActionBars);

            Add(btnCancel);
            Add(btnOk);
        }

        protected override void OnKeyPress(KeyboardArgs e)
        {
            base.OnKeyPress(e);
         
            if(HoverControl is SpellBarButton bar)
                Settings.Current.Keybinds[bar.BarId, bar.ButtonId] = e.Keybind;
        }

        protected override void OnClosed()
        {
            Settings.Current.ReloadFromDisk();
        }

        void saveAndHide()
        {
            Settings.Current.Save();
            IsVisible = false;
        }

        void discardAndHide()
        {
            Settings.Current.ReloadFromDisk();
            IsVisible = false;
        }

        void resetKeybindings()
        {
            Settings.Current.ResetKeybinds();
        }
    }
}
