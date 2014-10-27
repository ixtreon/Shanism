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


        public override void Update(int msElapsed)
        {
            Font = TextureCache.UiFont;
            Shown = HoverControl != null && !string.IsNullOrEmpty(HoverControl.TooltipText);
            if (Shown)
            {
                this.Text = HoverControl.TooltipText;
                this.AbsolutePosition = Screen.ScreenToUi(mouseState.Position + new Point(14, 26));
                absolutePosition.X = Math.Max(-1, Math.Min(absolutePosition.X, 1 - Size.X));
                absolutePosition.Y = Math.Max(-1, Math.Min(absolutePosition.Y, 1 - Size.Y));
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
                var anchor = Screen.UiToScreen(0.01);
                
                var sz = Font.MeasureString(Text, maxWidth: maxSize()) + new Point(anchor * 2, anchor * 2);
                SpriteCache.BlankTexture.Draw(sb, ScreenPosition, sz, Color.Black.SetAlpha(150));
                Font.DrawString(sb, Text, Color.White, this.ScreenPosition.Add(anchor, anchor), 0, 0, maxWidth: maxSize());
            }
        }
    }
}
