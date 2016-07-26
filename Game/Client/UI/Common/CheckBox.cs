using Microsoft.Xna.Framework;
using Shanism.Client.Drawing;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Common
{
    class CheckBox : Control
    {
        const double FrameWidth = Padding / 2;
        const double BoxInset = Padding / 2;
        const double ContentOffset = FrameWidth + BoxInset;
        static readonly Vector DefaultSize = new Vector(0.5, 0.1);

        public const double DefaultBoxSize = 0.05;


        public double BoxSize { get; set; } = DefaultBoxSize;

        public bool IsChecked { get; set; }

        public bool IsLocked { get; set; }

        public string Text { get; set; }

        public Color TextColor { get; set; } = Color.White;

        public TextureFont Font { get; set; }

        public Color BoxColor { get; set; } = Color.Black;

        RectangleF boxFrame;
        RectangleF boxContent;
        Vector textPos;

        Color curBackColor;

        public event Action<CheckBox> CheckedChanged;

        public CheckBox()
        {
            Font = Content.Fonts.NormalFont;
            Size = DefaultSize;
            MouseUp += onMouseUp;
        }

        void onMouseUp(Input.MouseButtonArgs obj)
        {
            if (!IsLocked)
            {
                IsChecked = !IsChecked;
                CheckedChanged?.Invoke(this);
            }
        }

        protected override void OnUpdate(int msElapsed)
        {
            boxFrame = new RectangleF(Padding, (Size.Y - BoxSize) / 2, BoxSize, BoxSize);
            boxContent = new RectangleF(boxFrame.Position + ContentOffset, new Vector(BoxSize - 2 * ContentOffset));
            var textX = ((Size.X - Padding) + boxFrame.Right) / 2;
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
                g.DrawString(Font, Text, TextColor, textPos, 0.5f, 0.5f);
        }
    }
}
