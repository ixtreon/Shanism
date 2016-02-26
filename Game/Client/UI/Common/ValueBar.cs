using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

namespace Client.UI.Common
{
    /// <summary>
    /// Displays a bar representing the current value of a property relative to the maximum value of that property. 
    /// </summary>
    class ValueBar : Control
    {
        public bool ShowText = false;

        public double Value;

        public double MaxValue;

        public Color ForeColor = Color.Azure;

        public ValueBar()
        {
            BackColor = new Color(64, 64, 64, 64);
            CanHover = true;
        }

        public override void OnDraw(Graphics g)
        {
            var hasText = ShowText && MaxValue > 0;
            var text = hasText ? (Value.ToString("0") + "/" + MaxValue.ToString("0")) : string.Empty;

            //background
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);

            //value
            if(MaxValue > 0)
            {
                var borderSize = new Vector(Size.Y / 10);
                var fullSize = Size - borderSize * 2;
                var valSize = fullSize * new Vector(Value / MaxValue, 1);

                g.Draw(Content.Textures.Blank, borderSize, valSize, ForeColor);
            }

            //text
            if(!string.IsNullOrEmpty(text))
            {
                var textPos = Size / 2;
                g.DrawString(Content.Fonts.NormalFont, text, Color.White, textPos, 0.5f, 0.5f);
            }
        }
    }
}
