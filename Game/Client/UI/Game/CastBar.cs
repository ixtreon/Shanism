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

        public override void Update(int msElapsed)
        {
            this.Visible = (Target != null && Target.OrderType == OrderType.Casting && Target.CastingAbility.CastTime > 0);

            if(this.Visible)
            {
                Bar.Value = Target.CastingProgress;
                Bar.MaxValue = Target.CastingAbility.CastTime;
                Bar.Size = this.Size;
            }
            base.Update(msElapsed);
        }

        public double castTimeLeft
        {
            get {  return Math.Max(0, (Target.CastingAbility.CastTime - Target.CastingProgress) / 1000.0); }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);

            //if(Visible)
                g.DrawString(Content.Fonts.MediumFont, "- {0:0.0}s".Format(castTimeLeft), Color.White,
                    Location + new Vector(Size.X, Size.Y / 2), 1, 0.5f);
        }
    }
}
