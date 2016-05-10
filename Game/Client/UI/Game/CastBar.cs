using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Game;
using Shanism.Client.UI.Common;
using Shanism.Common.Objects;
using Color = Microsoft.Xna.Framework.Color;

namespace Shanism.Client.UI
{
    class CastBar : Control
    {
        public IUnit Target { get; set; }

        public double Width
        {
            get { return Size.X; }
            set { Size = new Vector(value, Size.Y); }
        }

        double barFillValue;
        double secondsLeft;

        public CastBar()
        {
            BackColor = Color.Black.SetAlpha(75);

            Size = new Vector(0.5f, 0.08f);
        }

        protected override void OnUpdate(int msElapsed)
        {
            IsVisible = Target != null 
                && Target.OrderType == OrderType.Casting 
                && Target.TotalCastingTime > 0;

            if (IsVisible)
            {
                barFillValue = (double)Target.CastingProgress / Target.TotalCastingTime;
                secondsLeft = (Target.TotalCastingTime - Target.CastingProgress) / 1000.0;
            }

            base.OnUpdate(msElapsed);
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            if (IsVisible)
            {
                var borderSize = Padding;
                var fullSize = Size - borderSize * 2;
                var valSize = fullSize * new Vector(barFillValue, 1);

                g.Draw(Content.Textures.Blank, new Vector(Padding), valSize, Color.Goldenrod);

                g.DrawString(Content.Fonts.NormalFont, $"-{secondsLeft:0.0}s", Color.White, new Vector(2 * Padding, Size.Y / 2), 0, 0.5f);
            }
        }
    }
}
