using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public IEnumerable<Point> IterateToInclusive(Point b)
        {
            for (int ix = X; ix <= b.X; ix++)
                for (int iy = Y; iy <= b.Y; iy++)
                    yield return new Point(ix, iy);
        }

        public static implicit operator Vector(Point p)
        {
            return new Vector(p.X, p.Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        public static Point operator /(Point a, int divisor)
        {
            return new Point(a.X / divisor, a.Y / divisor);
        }
    }
}
