using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Client.Drawing;
using Shanism.Common;
using Shanism.Client.Input;

namespace Shanism.Client.UI.Tooltips
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
            if (string.IsNullOrWhiteSpace(tipAsString))
            {
                IsVisible = false;
                return;
            }

            if (Text != tipAsString)
            {
                Text = tipAsString;
                //update size, position
                Size = Font.MeasureStringUi(Text, 0.5) + new Vector(Padding * 2);
            }

            Location = ((MouseInfo.ScreenPosition + MouseInfo.CursorSize) / Screen.UiScale)
                .Clamp(Vector.Zero, Screen.UiSize - Size);
            IsVisible = true;
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
