using Shanism.Client.Input;
using Shanism.Client.UI.Common;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI.Menus.Keybinds
{
    class KeyBoxLabel : Control
    {
        static readonly string NoKeybindString = "<none>";

        readonly Label lblText;
        readonly Label lblValue;

        /// <summary>
        /// Gets the gameaction this keybox can change the binding to. 
        /// </summary>
        public ClientAction Action { get; }

        public KeyBoxLabel(ClientAction action)
        {
            var labelFont = Content.Fonts.NormalFont;

            Action = action;
            Size = new Vector(0.77, labelFont.HeightUi + 2 * Padding);
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
            if (k.Key != Keys.Escape)
            {
                var kb = new Keybind(KeyboardInfo.Modifiers, k.Key);
                Settings.Current.Keybinds[Action] = kb;
            }

            ClearFocus();
        }

        protected override void OnUpdate(int msElapsed)
        {
            var kb = Settings.Current.Keybinds[Action];

            lblValue.Text = kb.ToShortString(NoKeybindString);
            lblText.TextColor = HasHover ? Color.Goldenrod.Brighten(20) : Color.Goldenrod;
            ToolTip = kb.ToString();

            if (HasFocus)
                BackColor = Color.Black.SetAlpha((int)Ticker.Default.GetValue(150, 200));
            else
                BackColor = Color.Black.SetAlpha(HasHover ? 100 : 50);
        }
    }
}
