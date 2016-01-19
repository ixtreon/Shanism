using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.ShadowCalc
{

    using Line = Tuple<Vector, Vector>;

    class ShadowGuy
    {
        struct Corner
        {
            public Vector CornerPoint { get; set; }
            public double FarX { get; set; }
            public double FarY { get; set; }
        }

        public static IEnumerable<Line> GetVisionLines(Vector center, IEnumerable<RectangleF> objects)
        {
            var r = objects
                .SelectMany(o => getLines(center, o))
                .OrderBy(l => center.AngleTo(l.Item1));

            throw new NotImplementedException();
        }

        static int getQuadrant(Vector center, RectangleF rect)
        {
            var a = (rect.Center.X > center.X) ? 2 : 0;
            var b = (rect.Center.Y > center.Y) ? 1 : 0;
            return a + b;
        }

        static IEnumerable<Line> getLines(Vector center, RectangleF rect)
        {
            var q = getQuadrant(center, rect);
            switch (q)
            {
                case 0: //<x, <y -> right, top
                    yield return new Line(rect.BottomRight, rect.TopRight);
                    yield return new Line(rect.TopRight, rect.TopLeft);
                    break;
                case 1: //<x, >y -> right, bottom
                    yield return new Line(rect.BottomLeft, rect.BottomRight);
                    yield return new Line(rect.BottomRight, rect.TopRight);
                    break;
                case 2: //>x, <y -> left, top
                    yield return new Line(rect.BottomLeft, rect.TopLeft);
                    yield return new Line(rect.TopLeft, rect.TopRight);

                    break;
                case 3: //>x, >y -> left, bottom
                    yield return new Line(rect.TopLeft, rect.BottomLeft);
                    yield return new Line(rect.BottomLeft, rect.BottomRight);
                    break;
            }
        }
    }
}
