using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client.Drawing
{
    class Shadows
    {
        int _granularity = 1;

        /// <summary>
        /// Gets or sets the amount of cells per game unit.
        /// </summary>
        public int Granularity
        {
            get { return _granularity; }
            set
            {
                value = value.Clamp(1, 100);
                if (value != _granularity)
                {
                    _granularity = value;
                }
            }
        }

        bool[,] map;

        NGraphics.IImageCanvas canvas;

        Point MapSize;

        Texture2D texmex;


        public Shadows()
        {

        }

        public void SetSize(int w, int h)
        {

        }

        void recreateMap()
        {
            var bitmapSize = MapSize * _granularity;
            canvas = NGraphics.Platforms.Current.CreateImageCanvas(new NGraphics.Size(bitmapSize.X, bitmapSize.Y));
        }

        public void Update(List<IUnit> visibleUnits, int pid)
        {
            for (int i = 0; i < visibleUnits.Count; i++)
            {
                var u = visibleUnits[i];
                if (u.OwnerId == pid)
                    canvas.DrawEllipse(new NGraphics.Rect(u.Position.X - u.Scale, u.Position.Y - u.Scale, u.Scale * 2, u.Scale * 2), brush: NGraphics.Brushes.Black);
            }

            for (int i = 0; i < visibleUnits.Count; i++)
                for (int j = i + 1; j < visibleUnits.Count; j++)
                {
                    var a = visibleUnits[i];
                    var b = visibleUnits[j];

                    var aOurs = a.OwnerId == pid;
                    var bOurs = b.OwnerId == pid;

                    if (aOurs == bOurs)
                        continue;   //no shadow interaction: either both are ours or neither is

                    if (aOurs)
                        drawShadowCone(a, b);
                    else
                        drawShadowCone(b, a);
                }
        }

        public Texture2D genTex()
        {
            throw new NotImplementedException();
        }

        void drawShadowCone(IUnit ours, IUnit shadow)
        {
            var o = ours.Position;

            var d = o.DistanceTo(shadow.Position);
            var ang = o.AngleTo(shadow.Position);
            var v = ours.VisionRange;

            var c1 = shadow.Scale;
            var c2 = c1 * v / d;

            var s = o.PolarProjection(ang, v);
            var a = s.PolarProjection(ang + Math.PI / 2, c2);
            var b = s.PolarProjection(ang - Math.PI / 2, c2);

            var p = new NGraphics.Path(null, NGraphics.Brushes.White);
            p.MoveTo(o.X, o.Y);
            p.LineTo(a.X, a.Y);
            p.LineTo(b.X, b.Y);
            p.LineTo(o.X, o.Y);
            p.Draw(canvas);
        }
    }
}
