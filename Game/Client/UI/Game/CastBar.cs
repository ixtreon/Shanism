using IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using IO.Common;

namespace Client.UI
{
    class CastBar : Control
    {
        public IUnit Target { get; set; }

        public float Width
        {
            get { return Size.X; }
            set { Size = new Vector2(value, Size.Y); }
        }

        private ValueBar Bar = new ValueBar();

        public CastBar()
        {
            BackColor = new Color(15, 15, 15, 200);
            Bar.ForeColor = Color.Gold;

            this.Size = new Vector2(0.5f, 0.08f);
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

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if(this.Visible)
                Textures.TextureCache.StraightFont.DrawString(sb, "- {0:0.0}s".Format(castTimeLeft), Color.White,
                    AbsolutePosition + new Vector2(Size.X, Size.Y / 2), 1, 0.5f);
        }
    }
}
