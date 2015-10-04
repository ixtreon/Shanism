using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Sprites;
using Client.Textures;

namespace Client.UI.Common
{
    class ValueBar : Control
    {
        public bool ShowText = false;

        public double Value;

        public double MaxValue;

        public Color ForeColor = Color.Azure;

        public ValueBar()
        {
            BackColor = new Color(64, 64, 64, 64);
            ClickThrough = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            var hasText = ShowText && MaxValue != 0;
            var text = hasText ? (Value.ToString("0") + "/" + MaxValue.ToString("0")) : string.Empty;

            DrawValueBar(sb, Value, MaxValue, ScreenPosition, ScreenSize, BackColor, ForeColor, text);
        }

        public static void DrawValueBar(SpriteBatch sb, double value, double max, Point position, Point size, Color backColor, Color foreColor, string text = "")
        {
            SpriteFactory.Blank.DrawScreen(sb, position, size, backColor);

            if (max != 0)
            {
                var borderSize = size.Y / 10;
                position += new Point(borderSize, borderSize);
                size -= new Point(borderSize * 2, borderSize * 2);
                SpriteFactory.Blank.DrawScreen(sb, position, new Point((int)(size.X * value / max), size.Y), foreColor);
            }
            if (!string.IsNullOrEmpty(text))
            {
                var textPos = position + size.DivideBy(2);
                TextureCache.StraightFont.DrawStringScreen(sb, text, Color.White, textPos, 0.5f, 0.5f);
            }
        }
    }
}
