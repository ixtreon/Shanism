using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Client.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Textures;

namespace Client.UI
{
    /// <summary>
    /// A button that shows an image and can be clicked. 
    /// </summary>
    class Button : Control
    {
        public Texture2D Texture { get; set; }

        public string Text { get; set; }

        public TextureFont Font { get; set; }

        public Color FontColor { get; set; }

        public Keys Keybind { get; set; }

        public bool HasBorder { get; set; }

        public Color CurrentColor { get; private set; }

        public Button()
        {
            this.Font = TextureCache.StraightFont;
            this.FontColor = Color.Black;

            this.BackColor = Color.Black.Darken(-5);

            var sz = Font.MeasureStringUi("WOW");
            this.Size = new Vector2(0.05f, sz.Y);
        }

        public Button(string text = null, Texture2D texture = null)
        : this()
        {
            Text = text;
            Texture = texture;
        }

        public override void Update(int msElapsed)
        {
            if (MouseOver)
                CurrentColor = BackColor.Darken(30);
            else
                CurrentColor = BackColor;
        }

        public override void Draw(SpriteBatch sb)
        {
            //draw background
            SpriteFactory.Blank.DrawScreen(sb, ScreenPosition, ScreenSize, CurrentColor);
            if (Texture != null)
                sb.Draw(Texture, ScreenPosition, ScreenSize);

            if (!string.IsNullOrEmpty(Text))
                Font.DrawString(sb, Text, FontColor, AbsolutePosition + Size / 2, 0.5f, 0.5f);

            if (HasBorder)
            {
                var border = MouseOver ? SpriteFactory.Icon.BorderHover : SpriteFactory.Icon.Border;
                border.DrawScreen(sb, ScreenPosition, ScreenSize);
            }
        }

    }
}
