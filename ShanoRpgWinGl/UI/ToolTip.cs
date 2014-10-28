using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShanoRpgWinGl.Sprites;

namespace ShanoRpgWinGl.UI
{
    class ToolTip : UserControl
    {
        string Text;

        Color BackColor = Color.DarkGray.SetAlpha(100);

        public UserControl AssociatedControl;

        public bool Shown { get; private set; }

        public TextureFont Font { get; set; }

        public ToolTip()
        {
            Font = TextureCache.UiFont;
        }

        private int anchor
        {
            get
            {
                return Screen.UiToScreen(0.01);
            }
        }

        public override void Update(int msElapsed)
        {
            Shown = HoverControl != null && !string.IsNullOrEmpty(HoverControl.TooltipText);
            if (Shown)
            {
                this.Text = HoverControl.TooltipText;

                this.Size = Font.MeasureStringUi(Text, maxSize()) + new Vector2(0.02f, 0.02f);
                var screenPos =
                    (mouseState.Position + new Point(14, 26))
                    .ConstrainWithin(Point.Zero, Screen.ScreenSize - this.ScreenSize);
                this.AbsolutePosition = Screen.ScreenToUi(screenPos);

            }
        }

        private int maxSize()
        {
            return Screen.UiToScreen(0.5);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Shown)
            {
                SpriteCache.BlankTexture.Draw(sb, ScreenPosition, ScreenSize, Color.Black.SetAlpha(150));
                Font.DrawString(sb, Text, Color.White, this.ScreenPosition.Add(anchor, anchor), 0, 0, maxWidth: maxSize());
            }
        }
    }
}
