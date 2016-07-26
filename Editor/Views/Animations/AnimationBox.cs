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

        double _rotation;
        bool _lookingRight;

        public AnimationBox()
        {
            elapsedCounter.Tick += onTick;

            MouseMove += onMouseMove;

            //Set control styles to eliminate flicker on redraw and to redraw on resize
            this.SetStyle(
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer,
                true);
        }

        void onMouseMove(object sender, MouseEventArgs e)
        {
            if (Animation == null)
                return;

            var mouse = new Vector(e.X, e.Y);
            var center = new Vector(Width, Height) / 2;
            var d = mouse - center;

            var newRotation = Math.Atan2(d.Y, d.X) + Math.PI / 2;
            var newLookingRight = (d.X > 0);

            if (Animation.Style == AnimationStyle.TopDown)
            {
                if (Math.Abs(newRotation - _rotation) > 0.01)
                    Invalidate();
            }
            else if (Animation.Style != AnimationStyle.Fixed)
            {
                if (newLookingRight != _lookingRight)
                    Invalidate();
            }

            _rotation = newRotation;
            _lookingRight = newLookingRight;
        }

        void onTick(object sender, EventArgs e)
        {
            frameTicker.Tick();

            Invalidate();
        }
        public override void Refresh()
        {
            elapsedCounter.Interval = Math.Max(10, Animation?.Period ?? 10);
            elapsedCounter.Enabled = Animation?.IsDynamic ?? false;
            frameTicker = new Counter(Animation?.Span.Area ?? 1);

            base.Refresh();
        }

        public void SetAnimation(AnimationViewModel anim)
        {
            Animation = anim;

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (Animation?.Texture == null)
                return;

            //draw image
            var imgRatio = (float)Animation.AspectRatio.Value;

            var w = (int)Math.Min(Width, Height * imgRatio);
            var h = (int)(w / imgRatio);
            var x = (Width - w) / 2;
            var y = (Height - h) / 2;
            var rect = new Rectangle(x, y, w, h);

            var anim = Animation.IsDynamic ? frameTicker.Value : 0;


            Animation.Paint(g, transformDest(rect), anim);

            base.OnPaint(e);
        }

        Vector[] transformDest(RectangleF dest)
        {
            var baseArray = new[] { dest.TopLeft, dest.TopRight, dest.BottomLeft };
            switch (Animation.Style)
            {
                case AnimationStyle.Fixed:
                    return baseArray;

                case AnimationStyle.FullSizeLeft:
                    if (!_lookingRight)
                        return baseArray;
                    return new[] { dest.TopRight, dest.TopLeft, dest.BottomRight };

                case AnimationStyle.FullSizeRight:
                    if (_lookingRight)
                        return baseArray;
                    return new[] { dest.TopRight, dest.TopLeft, dest.BottomRight };

                case AnimationStyle.TopDown:
                    var center = dest.Center;
                    return baseArray
                        .Select(v => v.RotateAround(center, _rotation))
                        .ToArray();

                default:
                    throw new Exception("Missing switch case!");
            }
        }
    }
}
