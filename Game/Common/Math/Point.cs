using ProtoBuf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Shanism.Common
{
    /// <summary>
    /// Represents a point in the 2D plane with integral coordinates. 
    /// </summary>
    [ProtoContract]
    public struct Point
    {
        /// <summary>
        /// The point that has both of its X and Y coordinates set to zero. 
        /// </summary>
        public static readonly Point Zero = new Point();

        /// <summary>
        /// The point that has both of its X and Y coordinates set to one. 
        /// </summary>
        public static readonly Point One = new Point(1);



        /// <summary>
        /// The X coordinate of the point.
        /// </summary>
        [ProtoMember(1)]
        public int X;

        /// <summary>
        /// The Y coordinate of the point.
        /// </summary>
        [ProtoMember(2)]
        public int Y;


        /// <summary>
        /// Creates a new point with both its X and Y set to the given value. 
        /// </summary>
        /// <param name="v">The value for both the X and Y values of the point. </param>
        public Point(int v)
        {
            X = Y = v;
        }

        /// <summary>
        /// Creates a new point with the given X and Y coordinates. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        
        public static Point operator -(Point p)
        {
            return new Point(-p.X, -p.Y);
        }


        #region Point-point operators
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }
        public static Point operator /(Point a, Point b)
        {
            return new Point(a.X / b.X, a.Y / b.Y);
        }
        #endregion

        #region Point-int operators
        public static Point operator -(Point a, int other)
        {
            return new Point(a.X - other, a.Y - other);
        }
        public static Point operator +(Point a, int other)
        {
            return new Point(a.X + other, a.Y + other);
        }

        public static Point operator *(Point a, int multiplier)
        {
            return new Point(a.X * multiplier, a.Y * multiplier);
        }
        public static Point operator /(Point a, int divisor)
        {
            return new Point(a.X / divisor, a.Y / divisor);
        }

        public static Point operator %(Point a, int modulus)
        {
            return new Point(a.X % modulus, a.Y % modulus);
        }
        #endregion


        /// <summary>
        /// Returns all points on the rectangle having this point as its bottom-left corner
        /// and the given point as its top-right corner. 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public IEnumerable<Point> IterateToInclusive(Point b)
        {
            return IterateTo(b + 1);
        }
        /// <summary>
        /// Returns all points on the rectangle having this point as its bottom-left corner
        /// and the given point minus one as its top-right corner. 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public IEnumerable<Point> IterateTo(Point b)
        {
            for (int ix = X; ix < b.X; ix++)
                for (int iy = Y; iy < b.Y; iy++)
                    yield return new Point(ix, iy);
        }

        /// <summary>
        /// Returns a point with coordinates clamped 
        /// within the rectangle specified by the given points. 
        /// </summary>
        public Point Clamp(Point lowLeft, Point topRight)
        {
            var x = Math.Min(topRight.X, Math.Max(lowLeft.X, X));
            var y = Math.Min(topRight.Y, Math.Max(lowLeft.Y, Y));
            return new Point(x, y);
        }

        /// <summary>
        /// Gets the Euclidean distance from this point to the given point. 
        /// </summary>
        public double DistanceTo(Point other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            var p = (Point)obj;
            return p == this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            // http://stackoverflow.com/questions/5221396/what-is-an-appropriate-gethashcode-algorithm-for-a-2d-point-struct-avoiding
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        #endregion
    }
}
