using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Drawing;
using Shanism.Common;
using Shanism.Client.Input;

namespace Shanism.Client.UI.Tooltips
{
    /// <summary>
    /// A text-only tooltip.
    /// </summary>
    class SimpleTip : Control
    {
        string Text;

        public TextureFont Font { get; set; }

        public double MaxWidth { get; set; } = 0.5;

        public SimpleTip()
        {
            Font = Content.Fonts.NormalFont;
            BackColor = Color.Black.SetAlpha(25);
        }


        protected override void OnUpdate(int msElapsed)
        {
            var tipAsString = (HoverControl?.ToolTip as string);

            //visible if that's a string
            if (string.IsNullOrWhiteSpace(tipAsString))
            {
                IsVisible = false;
                return;
            }

            BringToFront();

            if (Text != tipAsString)
            {
                Text = tipAsString;

                //update size, position
                Size = Font.MeasureString(Text, MaxWidth) + new Vector(Padding * 2);
            }

            Location = ((MouseInfo.ScreenPosition + MouseInfo.CursorSize) / Screen.UiScale)
                .Clamp(Vector.Zero, Screen.UiSize - Size);
            IsVisible = true;
        }


        public override void OnDraw(Canvas g)
        {
            if (IsVisible)
            {
                g.Draw(Content.Textures.Blank, Vector.Zero, Size, Color.Black.SetAlpha(150));
                g.DrawString(Font, Text, Color.White, new Vector(Padding), 0, 0, MaxWidth);
            }
        }
    }
}
