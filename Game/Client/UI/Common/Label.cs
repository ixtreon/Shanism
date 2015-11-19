using Client.Assets.Fonts;
using IO.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI.Common
{
    class Label : Control
    {
        public string Text { get; set; } = string.Empty;

        public Color TextColor { get; set; } = Color.Goldenrod;

        public TextureFont Font { get; set; } = Content.Fonts.FancyFont;

        /// <summary>
        /// Gets or sets whether this label automatically fits to the text inside it. 
        /// </summary>
        public bool AutoSize { get; set; } = true;

        public Label()
        {
            BackColor = Color.Transparent;
            Size = new Vector(0.4, 0.1);

            ClickThrough = true;
        }
        public override void Update(int msElapsed)
        {
            if(AutoSize)
                Size = Font.MeasureStringUi(Text) + new Vector(Padding * 2);

            base.Update(msElapsed);
        }

        public override void Draw(Graphics g)
        {
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);
            g.DrawString(Font, Text, TextColor, new Vector(Padding, Padding), yAnchor: 0.5f);
        }
    }
}
