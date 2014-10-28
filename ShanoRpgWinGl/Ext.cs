using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;

namespace ShanoRpgWinGl
{
    public static class Ext
    {
        public static Color SetAlpha(this Color c, int a)
        {
            return new Color(c.R, c.G, c.B, a);
        }

        public static Vector2 ToVector2(this IO.Common.Vector v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

        public static IO.Common.Vector ToVector(this Vector2 v)
        {
            return new IO.Common.Vector(v.X, v.Y);
        }

        public static Point DivideBy(this Point p, int divisor)
        {
            return new Point(p.X / divisor, p.Y / divisor);
        }

        public static Point Add(this Point p, int dx, int dy)
        {
            return new Point(p.X + dx, p.Y + dy);
        }

        /// <summary>
        /// Constrains the first point between the other two. 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static Point ConstrainWithin(this Point p, Point low, Point high)
        {
            var x = Math.Min(high.X, Math.Max(low.X, p.X));
            var y = Math.Min(high.Y, Math.Max(low.Y, p.Y));
            return new Point(x, y);
        }
    }
}
