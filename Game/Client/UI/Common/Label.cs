using Shanism.Client.Assets;
using Shanism.Common.Game;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Common;

namespace Shanism.Client.UI.Common
{
    /// <summary>
    /// Displays a simple label. 
    /// </summary>
    class Label : Control
    {
        public string Text { get; set; } = string.Empty;

        public Color TextColor { get; set; } = Color.Goldenrod;

        public TextureFont Font { get; set; } = Content.Fonts.FancyFont;

        /// <summary>
        /// Gets or sets whether this label automatically fits to the text inside it. 
        /// </summary>
        public bool AutoSize { get; set; } = true;

        /// <summary>
        /// Gets or sets the X-align of the text. Recommended values are 0 (left), 0.5 (center) and 1 (right). 
        /// </summary>
        public float TextXAlign { get; set; }


        public Label()
        {
            BackColor = Color.Transparent;
            Size = new Vector(0.4, 0.1);

            CanHover = true;
        }
        protected override void OnUpdate(int msElapsed)
        {
            if(AutoSize)
                Size = Font.MeasureStringUi(Text) + new Vector(Padding * 2) + 0.001;

            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {

            var maxTextLen = Size.X - 2 * Padding;
            var textPos = new Vector(Padding + maxTextLen * TextXAlign, Size.Y / 2);

            var maxTexLen2 = Font.MeasureStringUi(Text);

            g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);
            g.DrawString(Font, Text, TextColor, textPos, TextXAlign, 0.5f, maxTextLen);
        }
    }
}
