using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Game;
using Shanism.Client.IO;
using Shanism.Common;

namespace Shanism.Client.UI.Menus.Keybinds
{
    class KeyBoxLabel : Control
    {
        static readonly string NoKeybindString = "<none>";

        readonly Label lblText;
        readonly Label lblValue;


        readonly GlowTimer glow = new GlowTimer(1000);

        /// <summary>
        /// Gets the gameaction this keybox can change the binding to. 
        /// </summary>
        public ClientAction Action { get; }

        public KeyBoxLabel(ClientAction action)
        {
            var labelFont = Content.Fonts.NormalFont;

            Action = action;
            Size = new Vector2(0.77f, labelFont.Height + 2 * Padding);
            CanFocus = true;

            var captionSize = new Vector2(0.4f, Size.Y);
            var keyLabelWidth = Size.X - captionSize.X;

            lblText = new Label
            {
                Location = new Vector2(),
                Size = captionSize,

                //BackColor = Color.Green.SetAlpha(100),


                CanHover = false,
                Text = action.ToString(),
                Font = labelFont,
            };

            lblValue = new Label
            {
                Location = new Vector2(Size.X - keyLabelWidth, 0),
                Size = new Vector2(keyLabelWidth, captionSize.Y),

                //BackColor = Color.Red.SetAlpha(100),

                CanHover = false,
                Font = labelFont,
                TextColor = UiColors.Text,
                TextAlign = AnchorPoint.CenterRight,
            };

            Add(lblText);
            Add(lblValue);
        }

        protected override void OnKeyPress(KeyboardArgs e)
        {
            base.OnKeyPress(e);

            if (e.Key != Keys.Escape)
                Settings.Current.Keybinds[Action] = e.Keybind;

            ClearFocus();
        }

        public override void Update(int msElapsed)
        {
            var kb = Settings.Current.Keybinds[Action];

            lblValue.Text = kb.ToShortString(NoKeybindString);
            lblText.TextColor = UiColors.TextTitle.Brighten(IsHoverControl ? 20 : 0);

            ToolTip = kb.ToString();

            if (IsFocusControl)
                BackColor = UiColors.ControlBackground.Brighten((int)glow.GetValue(25, 50));
            else
                BackColor = UiColors.ControlBackground.Brighten(IsHoverControl ? 25 : 0);
        }
    }
}
