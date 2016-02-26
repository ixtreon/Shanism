using Client.Input;
using Client.UI.Common;
using IO.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI.Menus.Keybinds
{
    class KeyBoxLabel : Control
    {
        static readonly string NoKeybindString = "<none>";

        readonly Label lblText;
        readonly Label lblValue;

        /// <summary>
        /// Gets the gameaction this keybox can change the binding to. 
        /// </summary>
        public GameAction Action { get; }

        public KeyBoxLabel(GameAction action)
        {
            var labelFont = Content.Fonts.NormalFont;

            Action = action;
            Size = new Vector(0.77, labelFont.UiHeight + 2 * Padding);
            CanFocus = true;
            KeyPressed += onKeyPressed;

            var captionSize = new Vector(0.4, Size.Y);
            var keyLabelWidth = Size.X - captionSize.X;

            lblText = new Label
            {
                Location = new Vector(),
                AutoSize = false,
                Size = captionSize,

                //BackColor = Color.Green.SetAlpha(100),


                CanHover = false,
                Text = action.ToString(),
                Font = labelFont,
            };

            lblValue = new Label
            {
                Location = new Vector(Size.X - keyLabelWidth, 0),
                AutoSize = false,
                Size = new Vector(keyLabelWidth, captionSize.Y),

                //BackColor = Color.Red.SetAlpha(100),

                CanHover = false,
                Font = labelFont,
                TextColor = Color.White,
                TextXAlign = 1,
            };

            Add(lblText);
            Add(lblValue);
        }

        void onKeyPressed(Keybind k)
        {
            if (k == Keys.LeftControl || k == Keys.RightControl
                || k == Keys.LeftAlt || k == Keys.RightAlt
                || k == Keys.LeftShift || k == Keys.RightShift)
                return;

            if (k != Keys.Escape)
            {
                var modKeys = ModifierKeys.None;
                if (KeyboardInfo.IsShiftDown)
                    modKeys |= ModifierKeys.Shift;
                if (KeyboardInfo.IsControlDown)
                    modKeys |= ModifierKeys.Control;
                if (KeyboardInfo.IsAltDown)
                    modKeys |= ModifierKeys.Alt;

                var kb = new Keybind(modKeys, k.Key);
                ShanoSettings.Current.Keybinds[Action] = kb;
            }

            ClearFocus();
        }

        protected override void OnUpdate(int msElapsed)
        {
            var kb =ShanoSettings.Current.Keybinds.TryGet(Action);

            lblValue.Text = kb?.ToShortString() ?? NoKeybindString;

            lblText.TextColor = MouseOver ? Color.Goldenrod.Brighten(20) : Color.Goldenrod;
            ToolTip = kb?.ToString() ?? string.Empty;

            if (IsFocused)
                BackColor = Color.Black.SetAlpha((int)Ticker.Default.GetValue(150, 200));
            else
                BackColor = Color.Black.SetAlpha(MouseOver ? 100 : 50);
        }
    }
}
