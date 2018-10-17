using Shanism.Common;
using System.Numerics;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A button that shows an image and/or text and can be clicked. 
    /// </summary>
    public class Button : SpriteBox
    {

        public static readonly Vector2 DefaultSize = new Vector2(0.15f, 0.06f);

        string _iconName;

        public string Text { get; set; }

        public AnchorPoint TextAlign { get; set; } = AnchorPoint.Center;

        public Font TextFont { get; set; }

        public string IconName
        {
            get => _iconName;
            set
            {
                _iconName = value;
                Sprite = string.IsNullOrEmpty(value) ? null
                    : Content.Icons.Get(value);
            }
        }

        public Button(string text = null)
        {
            Text = text;
            Cursor = GameCursor.ClickMe;

            Size = DefaultSize;
            Padding = LargePadding;

            BackColor = UiColors.Button;

            TextFont = Content.Fonts.NormalFont;
        }


        /// <summary>
        /// Draws the background, texture and text of the button.
        /// </summary>
        public override void Draw(Canvas c)
        {
            DrawBackground(c, curBack);
            DrawSprite(c, curSprite);

            // text
            if (!string.IsNullOrEmpty(Text))
            {
                var color = new Color(60, 60, 250, 150);

                c.FillRectangle(ClientBounds, color);
                c.DrawString(TextFont, Text, curText, ClientBounds, TextAlign);
            }
        }


        Color curBack, curSprite, curText;

        public override void Update(int msElapsed)
        {
            if(!CanHover)
            {
                // disabled
                curBack = UiColors.DisabledButton;
                curSprite = Tint.MixWith(Color.DarkGray, 0.25f);
                curText = UiColors.DisabledText;
            }
            else if(!IsHoverControl)
            {
                // enabled, no hover
                curBack = BackColor;
                curSprite = Tint;
                curText = UiColors.Text;
            }
            else
            {
                // enabled, hover
                curBack = BackColor.MixWith(Color.Black, 0.1f);
                curSprite = Tint.MixWith(Color.Black, 0.1f);
                curText = UiColors.Text.MixWith(Color.Black, 0.1f);
            }

            base.Update(msElapsed);
        }

        public void FitToText()
        {
            if(string.IsNullOrEmpty(Text))
            {
                Width = 2 * Padding;
                return;
            }

            Width = TextFont.MeasureString(Text).X + 2 * Padding;
        }
    }
}
