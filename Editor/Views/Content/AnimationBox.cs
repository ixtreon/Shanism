using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using GraphicsUnit = System.Drawing.GraphicsUnit;

namespace Shanism.Editor.Views.Content
{
    class AnimationBox : Panel
    {
        /// <summary>
        /// Gets the current model of this control. 
        /// </summary>
        AnimationViewModel Animation;



        Timer elapsedCounter = new Timer();
        Counter frameTicker;

        public AnimationBox()
        {
            elapsedCounter.Tick += ElapsedCounter_Tick;

            //Set control styles to eliminate flicker on redraw and to redraw on resize
            this.SetStyle(
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);
        }

        private void ElapsedCounter_Tick(object sender, EventArgs e)
        {
            var toContinue = !frameTicker.Tick() || Animation.IsLooping;

            if (!toContinue)
            {
                elapsedCounter.Enabled = false;
                frameTicker.Tick(Animation.Span.Area - 1);
            }

            Invalidate();
        }     

        public void SetAnimation(AnimationViewModel anim)
        {
            Animation = anim;

            //update timer interval and state
            if (Animation == null || !Animation.IsDynamic)
            {
                elapsedCounter.Enabled = false;
            }
            else
            {
                var period = Animation.Period;
                if (period <= 0) period = 10;

                elapsedCounter.Interval = period;
                frameTicker = new Counter(Animation.Span.Area);

                elapsedCounter.Enabled = true;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (Animation?.Texture == null)
                return;

            //draw image
            var imgRatio = (float)Animation.Size.X / Animation.Size.Y;

            var destSz = new Vector(Math.Min(Width, Height * imgRatio),
                Math.Min(Height, Width / imgRatio)).ToPoint();

            var destPos = (Size.ToPoint() - destSz) / 2;

            var anim = Animation.IsDynamic ? frameTicker.Value : 0;
            Animation.Paint(g, new Rectangle(destPos, destSz), anim);

            base.OnPaint(e);
        }
    }
}
