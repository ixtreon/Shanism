using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;
using IO;

namespace Client.UI.Common
{
    /// <summary>
    /// A progress bar that exposes a value between 0 and 1, and some text. 
    /// </summary>
    class ProgressBar : Control
    {
        double _progress;

        public bool ShowText { get; set; } = true;

        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress as a value in the range from 0.0 to 1.0. 
        /// </summary>
        public double Progress
        {
            get { return _progress; }
            set { _progress = value.Clamp(0.0, 1.0); }
        }

        public Color ForeColor { get; set; } = Color.Goldenrod;

        public ProgressBar()
        {
            BackColor = new Color(64, 64, 64, 64);
            CanHover = true;
        }

        public override void OnDraw(Graphics g)
        {
            //background
            g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);

            //value
            if(Progress > 0)
            {
                var borderSize = new Vector(Size.Y / 10);
                var fullSize = Size - borderSize * 2;
                var valSize = fullSize * new Vector(Progress, 1);

                g.Draw(Content.Textures.Blank, borderSize, valSize, ForeColor);
            }

            //text
            if(ShowText && !string.IsNullOrEmpty(Text))
                g.DrawString(Content.Fonts.NormalFont, Text, Color.White, Size / 2, 0.5f, 0.5f);
        }
    }
}
