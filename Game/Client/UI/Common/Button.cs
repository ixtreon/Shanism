using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Textures;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;
using Client.Assets.Fonts;

namespace Client.UI.Common
{
    /// <summary>
    /// A button that shows an image and can be clicked. 
    /// </summary>
    class Button : Control
    {
        /// <summary>
        /// Gets or sets the texture that is drawn stretched on the button. 
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets the texture color (tint). 
        /// </summary>
        public Color TextureColor { get; set; } = Color.White;

        public string Text { get; set; }

        public TextureFont Font { get; set; } = Content.Fonts.MediumFont;

        public Color FontColor { get; set; } = Color.Black;

        public Keys Keybind { get; set; }

        public bool HasBorder { get; set; }

        public Color BackHoverColor { get; set; }

        public Button(string text = null, Texture2D texture = null)
        {
            var sz = Font.MeasureStringUi("WOWyglj");
            Size = new Vector(0.05f, sz.Y);

            Text = text;
            Texture = texture;

            BackColor = Color.White.Darken(95);
            BackHoverColor = BackColor.Darken(50);
        }


        public override void Draw(Graphics g)
        {
            //draw background
            var bgc = MouseOver ? BackHoverColor : BackColor;
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, bgc);

            //draw texture
            if (Texture != null)
                g.Draw(Texture, Vector.Zero, Size, TextureColor);

            //draw text
            if (!string.IsNullOrEmpty(Text))
                g.DrawString(Font, Text, FontColor, AbsolutePosition + Size / 2, 0.5f, 0.5f);

            //draw border
            if (HasBorder)
            {
                var border = MouseOver ? Content.Textures.TryGetIcon("border_hover") : Content.Textures.TryGetIcon("border");
                g.Draw(border, Vector.Zero, Size);
            }
        }

    }
}
