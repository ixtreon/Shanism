using IO;
using IO.Common;
using IO.Content;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using GraphicsUnit = System.Drawing.GraphicsUnit;

namespace ShanoEditor.Views.Content
{
    class AnimationBox : Panel
    {
        /// <summary>
        /// Gets the current model of this control. 
        /// </summary>
        public AnimationViewModel AnimationModel { get; private set; }



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
            if (frameTicker.Tick() && !AnimationModel.Animation.IsLooping)
                elapsedCounter.Enabled = false;

            Invalidate();
        }     

        public void SetAnimation(AnimationViewModel anim)
        {
            AnimationModel = anim;

            //update timer interval and state
            if (AnimationModel == null || !AnimationModel.Animation.IsDynamic)
            {
                elapsedCounter.Enabled = false;
            }
            else
            {
                var period = AnimationModel.Animation.Period;
                if (period <= 0) period = 10;

                elapsedCounter.Interval = period;
                frameTicker = new Counter(AnimationModel.Animation.Span.Area);

                elapsedCounter.Enabled = true;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (AnimationModel?.Texture == null)
                return;

            //draw image
            var imgRatio = (float)AnimationModel.Size.X / AnimationModel.Size.Y;

            var destSz = new Vector(Math.Min(Width, Height * imgRatio),
                Math.Min(Height, Width / imgRatio)).ToPoint();

            var destPos = (Size.ToPoint() - destSz) / 2;

            var anim = AnimationModel.Animation.IsDynamic ? frameTicker.Value : 0;
            AnimationModel.Paint(g, new Rectangle(destPos, destSz), anim);

            base.OnPaint(e);
        }
    }
}
