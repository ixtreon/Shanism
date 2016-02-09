using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using IO.Common;
using Client.UI.Common;
using IO.Objects;
using Color = Microsoft.Xna.Framework.Color;

namespace Client.UI
{
    class CastBar : Control
    {
        public IUnit Target { get; set; }

        public double Width
        {
            get { return Size.X; }
            set { Size = new Vector(value, Size.Y); }
        }

        private ValueBar Bar = new ValueBar();

        public CastBar()
        {
            BackColor = new Color(15, 15, 15, 200);
            Bar.ForeColor = Color.Gold;

            this.Size = new Vector(0.5f, 0.08f);
            this.Add(Bar);
        }

        protected override void OnUpdate(int msElapsed)
        {
            this.Visible = (Target != null && Target.OrderType == OrderType.Casting && Target.TotalCastingTime > 0);

            if (this.Visible)
            {
                Bar.Value = Target.CastingProgress;
                Bar.MaxValue = Target.TotalCastingTime;
                Bar.Size = this.Size;
            }
            base.OnUpdate(msElapsed);
        }

        public double castTimeLeft
        {
            get
            {
                var timeLeft = (Target?.TotalCastingTime - Target?.CastingProgress) ?? 0;
                return timeLeft / 1000.0;
            }
        }

        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            if (Visible)
                g.DrawString(Content.Fonts.NormalFont, "- {0:0.0}s".F(castTimeLeft), Color.White,
                    Location + new Vector(Size.X, Size.Y / 2), 1, 0.5f);
        }
    }
}
