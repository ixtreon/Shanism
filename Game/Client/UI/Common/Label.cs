using Shanism.Client.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Client.UI
{
    /// <summary>
    /// Displays a simple label. 
    /// </summary>
    class Label : Control
    {
        public static TextureFont DefaultFont => Content.Fonts.FancyFont;

        string _text = string.Empty;
        TextureFont font;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                resize();
            }
        }

        void resize()
        {
            if (AutoSize)
                Size = Font.MeasureStringUi(Text) + new Vector(Padding * 2) + 1E-5;
        }

        public Color TextColor { get; set; } = Color.Goldenrod;

        public TextureFont Font
        {
            get { return font; }
            set
            {
                font = value;
                resize();
            }
        }

        /// <summary>
        /// Gets or sets whether this label automatically fits to the text inside it. 
        /// </summary>
        public bool AutoSize { get; set; } = true;

        /// <summary>
        /// Gets or sets the X-align of the text. Recommended values are 0 (left), 0.5 (center) and 1 (right). 
        /// </summary>
        public float TextXAlign { get; set; }

        public float TextScale { get; set; } = 1;

        public Label()
        {
            BackColor = Color.Transparent;
            Size = new Vector(0.4, 0.1);
            font = DefaultFont;

            CanHover = true;
        }
        protected override void OnUpdate(int msElapsed)
        {

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
