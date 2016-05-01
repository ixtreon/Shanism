using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Textures;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;
using Client.Assets;

namespace Client.UI.Tooltips
{
    class SimpleTip : Control
    {
        string Text;

        public TextureFont Font { get; set; }

        /// <summary>
        /// Gets or sets the max size of the tooltip in UI units. 
        /// </summary>
        public int MaxSize { get; set; }

        public SimpleTip()
        {
            Font = Content.Fonts.NormalFont;
            BackColor = Color.Black.SetAlpha(25);
        }


        protected override void OnUpdate(int msElapsed)
        {
            var tipAsString = (HoverControl?.ToolTip as string);

            //visible if that's a string
            IsVisible = HoverControl != null && !string.IsNullOrWhiteSpace(tipAsString);
            if (!IsVisible)
                return;

            Text = tipAsString;

            //update size, position
            Size = Font.MeasureStringUi(Text, 0.5) + new Vector(Padding * 2);
            var screenPosRaw =
                (mouseState.Position.ToPoint() + new Point(14, 26));
            var screenPos = screenPosRaw
                .Clamp(Point.Zero, Screen.Size - ScreenSize);
            AbsolutePosition = Screen.ScreenToUi(screenPos);

            BringToFront();
        }


        public override void OnDraw(Graphics g)
        {
            if (IsVisible)
            {
                g.Draw(Content.Textures.Blank, Vector.Zero, new Vector(555), Color.Black.SetAlpha(150));
                g.DrawString(Font, Text, Color.White, new Vector(Padding), 0, 0, 0.5);
            }
        }
    }
}
