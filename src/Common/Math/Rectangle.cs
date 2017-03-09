using ProtoBuf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Shanism.Common
{
    /// <summary>
    /// Represents a rectangle in the 2D plane. 
    /// </summary>
    [ProtoContract]
    [JsonObject(MemberSerialization.OptIn)]
    public struct Rectangle
    {
        /// <summary>
        /// An empty rectangle positioned at the origin. 
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle();


        [ProtoMember(1)]
        [JsonProperty]
        int _x;
        [ProtoMember(2)]
        [JsonProperty]
        int _y;
        [ProtoMember(3)]
        [JsonProperty]
        int _width;
        [ProtoMember(4)]
        [JsonProperty]
        int _height;


        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct. 
        /// </summary>
        /// <param name="position">The position of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        public Rectangle(Point position, Point size)
        {
            _x = position.X;
            _y = position.Y;
            _width = size.X;
            _height = size.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate of the rectangle.</param>
        /// <param name="y">The y coordinate of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        #region Property Shortcuts

        /// <summary>
        /// Gets or sets the position of the bottom-left (low) corner of the rectangle. 
        /// </summary>
        public Point Position => new Point(_x, _y);

        /// <summary>
        /// Gets or sets the size of the rectangle. 
        /// </summary>
        public Point Size => new Point(_width, _height);

        /// <summary>
        /// Gets the far position (high X, high Y) of the rectangle. 
        /// </summary>
        /// <value>
        /// The far position.
        /// </value>
        public Point FarPosition => Position + Size;

        /// <summary>
        /// Gets the vector that lies at the center of this rectangle. 
        /// </summary>
        public Vector Center => Position + (Vector)Size / 2.0;


        /// <summary>
        /// Gets the low X edge of the rectangle. 
        /// </summary>
        public int X => _x;

        /// <summary>
        /// Gets the low Y edge of the rectangle. 
        /// </summary>
        public int Y => _y;

        /// <summary>
        /// Gets the width of the rectangle. 
        /// </summary>
        public int Width => _width;

        /// <summary>
        /// Gets the height of the rectangle. 
        /// </summary>
        public int Height => _height;

        /// <summary>
        /// Gets the left (low X) edge of the rectangle. 
        /// </summary>
        public int Left => _x;

        /// <summary>
        /// Gets the right (high X) edge of the rectangle. 
        /// </summary>
        public int Right => _x + _width;

        /// <summary>
        /// Gets the bottom (low Y) edge of the rectangle. 
        /// </summary>
        public int Bottom => _y;

        /// <summary>
        /// Gets the top (high Y) edge of the rectangle. 
        /// </summary>
        public int Top => _y + _height;


        /// <summary>
        /// Gets the area of the rectangle. 
        /// </summary>
        public int Area => Width * Height;

        #endregion

        #region Binary Operators

        /// <summary>
        /// Multiplies both the position and size of the rectangle 
        /// by the specified vector. 
        /// </summary>
        public static Rectangle operator *(Rectangle r, Vector v)
        {
            return new Rectangle(
                (int)(r.X * v.X), (int)(r.Y * v.Y),
                (int)(r.Width * v.X), (int)(r.Height * v.Y));
        }


        /// <summary>
        /// Offsets the rectangle by the specified amount. 
        /// </summary>
        public static Rectangle operator +(Rectangle r, Point p)
        {
            return new Rectangle(r.X + p.X, r.Y + p.Y, r.Width, r.Height);
        }
        /// <summary>
        /// Offsets the rectangle by the specified amount. 
        /// </summary>
        public static Rectangle operator -(Rectangle r, Point p)
        {
            return new Rectangle(r.X - p.X, r.Y - p.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Divides both the position and size of the rectangle 
        /// by the specified point. 
        /// </summary>
        public static Rectangle operator /(Rectangle r, Point p)
        {
            return new Rectangle(r.X / p.X, r.Y / p.Y, r.Width / p.X, r.Height / p.Y);
        }

        /// <summary>
        /// Determines whether two rectangles are logically equivalent. 
        /// </summary>
        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.Position == b.Position && a.Size == b.Size;
        }

        /// <summary>
        /// Determines whether two rectangles are logically different. 
        /// </summary>
        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        #endregion

        /// <summary>
        /// Inflates this rectangle to the right. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Rectangle Inflate(int x, int y) => new Rectangle(X, Y, Width + x, Height + y);

        /// <summary>
        /// Iterates all points within this rectangle. First X then Y. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point> Iterate()
        {
            for (int ix = 0; ix < Size.X; ix++)
                for (int iy = 0; iy < Size.Y; iy++)
                    yield return new Point(X + ix, Y + iy);
        }

        /// <summary>
        /// Gets the intersection (common area) of the two rectangles. 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public Rectangle IntersectWith(Rectangle rectangle)
        {
            var x = Math.Max(rectangle.X, X);
            var y = Math.Max(rectangle.Y, Y);
            var farX = Math.Min(rectangle.Right, Right);
            var farY = Math.Min(rectangle.Top, Top);

            return new Rectangle(x, y, farX - x, farY - y);
        }

        /// <summary>
        /// Returns whether the given coordinates lie inside this rectangle. 
        /// </summary>
        public bool Contains(double x, double y) 
            => x >= X 
            && y >= Y 
            && x < X + Width
            && y < Y + Height;

        /// <summary>
        /// Returns whether the given point lies inside this rectangle. 
        /// </summary>
        public bool Contains(Point p) => Contains(p.X, p.Y);

        /// <summary>
        /// Returns whether the given vector lies inside this rectangle. 
        /// </summary>
        public bool Contains(Vector v) => Contains(v.X, v.Y);

        /// <summary>
        /// Returns a new rectangle representing the same area,
        /// but with non-negative <see cref="Width"/> and <see cref="Height"/>. 
        /// </summary>
        public Rectangle MakePositive() 
            => new Rectangle(
                Math.Min(X, X + Width),
                Math.Min(Y, Y + Height),
                Math.Abs(Width),
                Math.Abs(Height));

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"[{X}, {Y}, {Width}, {Height}]";

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) 
            => (obj is Rectangle) && this == (Rectangle)obj;

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
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                return hash;
            }
        }
    }
}
