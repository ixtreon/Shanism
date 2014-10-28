using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShanoRpgWinGl.Sprites;

namespace ShanoRpgWinGl.UI
{
    class ValueLabel : Control
    {
        public string Text = string.Empty;

        public Color TextColor = Color.Goldenrod;

        public Color ValueColor = Color.White;

        public Color BackColor = Color.Transparent;

        public string Value = string.Empty;

        //public TextureFont Font { get; set; }

        public ValueLabel()
        {
            this.Size = new Vector2(0.37f, 0.08f);
            this.Locked = true;
            this.ClickThrough = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            SpriteCache.BlankTexture.Draw(sb, ScreenPosition, ScreenSize, BackColor);

            TextureCache.MainFont.DrawString(sb, Text, TextColor, ScreenPosition.Add(6, ScreenSize.Y / 2), yAnchor: 0.5f);
            TextureCache.UiFont.DrawString(sb, Value, ValueColor, ScreenPosition.Add(ScreenSize.X - 6, ScreenSize.Y / 2), xAnchor: 1.0f, yAnchor: 0.5f);

        }
    }
}
