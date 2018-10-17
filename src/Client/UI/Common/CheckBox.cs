using Ix.Math;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI
{
    public class CheckBox : Control
    {
        //size of the checkbox elements
        const float FrameWidth = DefaultPadding / 2;
        const float BoxInset = DefaultPadding / 2;

        static readonly float ContentOffset = FrameWidth + BoxInset;
        static readonly Vector2 DefaultSize = new Vector2(0.5f, 0.1f);
        static readonly Vector2 DefaultBoxSize = new Vector2(0.05f);

        /// <summary>
        /// Gets or sets the size of the box in UI units.
        /// </summary>
        public Vector2 BoxSize { get; set; } = DefaultBoxSize;

        /// <summary>
        /// Gets or sets the checked state of this checkbox.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets whether the checked state of the checkbox is locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the text displayed next to the checkbox.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the text displayed next to the checkbox.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the font of the text displayed next to the checkbox.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the box border.
        /// </summary>
        public Color BoxColor { get; set; }

        RectangleF boxFrame, boxCheckedRect, textBounds;

        Color curBackColor;

        /// <summary>
        /// Occurs when the checked state of the checkbox has changed.
        /// </summary>
        public event Action<CheckBox> CheckedChanged;

        public CheckBox()
        {
            Font = Content.Fonts.NormalFont;
            Size = DefaultSize;
            TextColor = UiColors.Text;
            BoxColor = UiColors.Border;

            MouseClick += (o, e) => toggleCheckedState();
        }

        void toggleCheckedState()
        {
            if (!IsLocked)
            {
                IsChecked = !IsChecked;
                CheckedChanged?.Invoke(this);
            }
        }

        public override void Update(int msElapsed)
        {
            var boxPos = new Vector2(ClientBounds.Right - BoxSize.X, (Size.Y - BoxSize.Y) / 2);
            boxFrame = new RectangleF(boxPos, BoxSize);
            boxCheckedRect = boxFrame.Inflate(-ContentOffset);

            textBounds = new RectangleF(Vector2.Zero, Size).Inflate(-Padding);

            if (IsHoverControl)
                curBackColor = Color.Black.SetAlpha(50);
            else
                curBackColor = BackColor;
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

            // box frame
            c.DrawRectangle(boxFrame, FrameWidth, BoxColor);

            // checkbox (a square)
            if (IsChecked)
                c.FillRectangle(boxCheckedRect, BoxColor);

            // text
            if (!string.IsNullOrEmpty(Text))
                c.DrawString(Font, Text, TextColor, textBounds, AnchorPoint.CenterLeft);

        }
    }
}
