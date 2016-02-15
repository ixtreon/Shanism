using Client.Input;
using Client.UI.Common;
using IO.Common;
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

        static KeyBoxLabel SelectedKeyBox;

        readonly Label lblText;
        readonly Label lblValue;

        public GameAction Action { get; }

        bool IsActive { get; set; }

        public KeyBoxLabel(GameAction action)
        {
            var labelFont = Content.Fonts.NormalFont;

            Action = action;
            Size = new Vector(0.77, labelFont.UiHeight + 2 * Padding);
            this.MouseUp += onMouseUp;

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

        void onMouseUp(MouseButtonEvent obj)
        {
            if (!IsActive && SelectedKeyBox != this)
            {
                if(SelectedKeyBox != null)
                    SelectedKeyBox.IsActive = false;

                SelectedKeyBox = this;
            }
            IsActive = !IsActive;
        }

        protected override void OnUpdate(int msElapsed)
        {
            var kb = ShanoSettings.Current.Keybinds.TryGet(Action);
            lblValue.Text = kb?.ToString() ?? NoKeybindString;

            lblText.TextColor = MouseOver ? Color.Goldenrod.Brighten(20) : Color.Goldenrod;
            if (IsActive)
                BackColor = Color.Black.SetAlpha((int)Ticker.Default.GetValue(150, 200));
            else
                BackColor = Color.Black.SetAlpha(IsActive ? 200 : (MouseOver ? 100 : 50));
        }
    }
}
