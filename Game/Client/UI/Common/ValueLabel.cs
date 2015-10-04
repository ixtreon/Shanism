using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Sprites;
using Client.Textures;

namespace Client.UI.Common
{
    class ValueLabel : Control
    {
        private static readonly TextureFont InvertedValueFont = new TextureFont(TextureCache.FancyFont, 2);

        public string Text = string.Empty;

        public TextureFont TextFont;

        public TextureFont ValueFont;


        public Color TextColor = Color.Goldenrod;

        public Color ValueColor = Color.White;

        public string Value = string.Empty;

        public readonly bool Inverted = false;

        //public TextureFont Font { get; set; }

        public ValueLabel(bool inverted = false)
        {
            this.BackColor = Color.Transparent;
            if (!inverted)
            {
                this.Size = new Vector2(0.4f, 0.08f);
                this.TextFont = TextureCache.FancyFont;
                this.ValueFont = TextureCache.StraightFont;
            }
            else
            {
                this.Size = new Vector2(0.15f, 0.15f);
                this.TextFont = new TextureFont(TextureCache.StraightFont, 0.75f);
                this.ValueFont = new TextureFont(TextureCache.FancyFont, 1.3f);
            }

            this.Inverted = inverted;
            this.Locked = true;
            this.ClickThrough = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            SpriteFactory.Blank.DrawScreen(sb, ScreenPosition, ScreenSize, BackColor);

            if (!Inverted)
            {
                TextFont.DrawStringScreen(sb, Text, TextColor, ScreenPosition.Add(6, ScreenSize.Y / 2), yAnchor: 0.5f);
                ValueFont.DrawStringScreen(sb, Value, ValueColor, ScreenPosition.Add(ScreenSize.X - 6, ScreenSize.Y / 2), xAnchor: 1.0f, yAnchor: 0.5f);
            }
            else
            {
                ValueFont.DrawStringScreen(sb, Value, TextColor, ScreenPosition.Add(ScreenSize.X / 2, 0), xAnchor: 0.5f, yAnchor: 0f);
                TextFont.DrawStringScreen(sb, Text, ValueColor, ScreenPosition.Add(ScreenSize.X / 2, ScreenSize.Y), xAnchor: 0.5f, yAnchor: 1f);
            }
        }
    }
}
