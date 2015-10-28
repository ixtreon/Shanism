using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Sprites;
using Client.Textures;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class ToolTip : Control
    {
        string Text;

        public TextureFont Font { get; set; }

        public ToolTip()
        {
            Font = TextureCache.StraightFont;
            BackColor = Color.Black.SetAlpha(25);
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
            Visible = HoverControl != null && !string.IsNullOrEmpty(HoverControl.TooltipText);
            if (Visible)
            {
                this.Text = HoverControl.TooltipText;

                this.Size = Font.MeasureStringUi(Text, maxSize()) + new Vector(0.02f, 0.02f);
                var screenPos =
                    (mouseState.Position.ToPoint() + new Point(14, 26))
                    .ConstrainWithin(Point.Zero, Screen.Size - this.ScreenSize);
                this.AbsolutePosition = Screen.ScreenToUi(screenPos);

            }
        }

        private int maxSize()
        {
            return Screen.UiToScreen(0.5);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Visible)
            {
                SpriteFactory.Blank.DrawScreen(sb, ScreenPosition, ScreenSize, Color.Black.SetAlpha(150));
                Font.DrawStringScreen(sb, Text, Color.White, this.ScreenPosition + new Point(anchor, anchor), 0, 0, maxWidth: maxSize());
            }
        }
    }
}
