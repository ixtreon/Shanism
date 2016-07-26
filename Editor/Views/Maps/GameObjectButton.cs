﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Common.StubObjects;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Editor.Views.Maps
{

    class GameObjectButton : ObjectButton<IEntity>
    {

        /// <summary>
        /// Gets or sets whether the object's name is shown underneath its thumbnail. 
        /// </summary>
        public bool ShowObjectName { get; set; } = true;


        public float FontSizePixels = 14;


        public GameObjectButton(IEntity obj)
            : base(obj, 64)
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
        }


        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            const float fontMargin = 3f;

            base.OnPaint(pevent);

            if (Model == null)
                return;

            var g = pevent.Graphics;
            var animMargin = fontMargin;
            
            //draw object name
            if (ShowObjectName)
            {
                using (var font = new Font(FontFamily.GenericSansSerif, FontSizePixels, GraphicsUnit.Pixel))
                {
                    var txt = Object.Name;
                    var txtSize = g.MeasureString(txt, font);
                    var txtPos = new PointF((Width - txtSize.Width) / 2, Height - txtSize.Height - fontMargin);

                    g.DrawString(txt, font, Brushes.Black, txtPos);
                }

                animMargin = (fontMargin * 3 + FontSizePixels) / 2;
            }


            //draw animation
            var anim = Model.Content.Animations.TryGet(Object.Model);
            var animDest = new Common.RectangleF(animMargin, fontMargin, Width - 2 * animMargin, Height - (2 * animMargin - fontMargin));
            if (anim == null)
                g.FillRectangle(Brushes.Red, animDest.ToNetRectangle());
            else
                anim.Paint(g, new[] { animDest.TopLeft, animDest.TopRight, animDest.BottomLeft });


        }
    }
}
