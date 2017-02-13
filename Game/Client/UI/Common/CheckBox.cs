using Shanism.Client.Drawing;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.UI
{
    class CheckBox : Control
    {
        //size of the checkbox elements
        const double FrameWidth = Padding / 2;
        const double BoxInset = Padding / 2;
        const double ContentOffset = FrameWidth + BoxInset;

        static readonly Vector DefaultSize = new Vector(0.5, 0.1);

        public const double DefaultBoxSize = 0.05;

        /// <summary>
        /// Gets or sets the size of the box in UI units.
        /// </summary>
        public double BoxSize { get; set; } = DefaultBoxSize;

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
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the font of the text displayed next to the checkbox.
        /// </summary>
        public TextureFont Font { get; set; }

        /// <summary>
        /// Gets or sets the color of the box.
        /// </summary>
        public Color BoxColor { get; set; } = Color.Black;

        RectangleF boxFrame;
        RectangleF boxContent;
        Vector textPos;

        Color curBackColor;

        /// <summary>
        /// Occurs when the checked state of the checkbox has changed.
        /// </summary>
        public event Action<CheckBox> CheckedChanged;

        public CheckBox()
        {
            Font = Content.Fonts.NormalFont;
            Size = DefaultSize;
            MouseClick += (args) => toggleCheckedState();
        }

        void toggleCheckedState()
        {
            if (!IsLocked)
            {
                IsChecked = !IsChecked;
                CheckedChanged?.Invoke(this);
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            boxFrame = new RectangleF(Size.X - BoxSize - Padding, (Size.Y - BoxSize) / 2, BoxSize, BoxSize);
            boxContent = new RectangleF(boxFrame.Position + ContentOffset, new Vector(BoxSize - 2 * ContentOffset));

            var textX = Padding;
            var textY = Size.Y / 2;
            textPos = new Vector(textX, textY);

            if (HoverControl == this)
                curBackColor = Color.Black.SetAlpha(50);
            else
                curBackColor = BackColor;
        }

        public override void OnDraw(Graphics g)
        {

            var blank = Content.Textures.Blank;
            g.Draw(blank, Vector.Zero, g.Size, curBackColor);

            //draw box frame
            var hSz = new Vector(BoxSize, FrameWidth);
            var vSz = new Vector(FrameWidth, BoxSize);
            g.Draw(blank, boxFrame.Position, hSz, BoxColor);
            g.Draw(blank, boxFrame.Position, vSz, BoxColor);
            g.Draw(blank, boxFrame.FarPosition - hSz, hSz, BoxColor);
            g.Draw(blank, boxFrame.FarPosition - vSz, vSz, BoxColor);

            //draw checkbox
            if (IsChecked)
                g.Draw(blank, boxContent.Position, boxContent.Size, BoxColor);

            //draw text
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Font, Text, TextColor, textPos, 0f, 0.5f);
        }
    }
}
