using IxSerializer.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Common
{
    /// <summary>
    /// Represents a rectangle in the 2D plane. 
    /// </summary>
    [SerialKiller]
    [JsonObject(MemberSerialization.OptIn)]
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle();

        /// <summary>
        /// Gets or sets the position of the bottom-left (low) corner of the rectangle. 
        /// </summary>
        [SerialMember]
        public Point Position;


        /// <summary>
        /// Gets or sets the size of the rectangle. 
        /// </summary>
        [SerialMember]
        public Point Size;

        [JsonProperty]
        public int X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        [JsonProperty]
        public int Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        [JsonProperty]
        public int Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        [JsonProperty]
        public int Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        /// <summary>
        /// Gets the left (low X) edge of the rectangle. 
        /// </summary>
        public int Left
        {
            get { return Position.X; }
        }

        /// <summary>
        /// Gets the right (high X) edge of the rectangle. 
        /// </summary>
        public int Right
        {
            get { return Position.X + Size.X; }
        }

        /// <summary>
        /// Gets the bottom (low Y) edge of the rectangle. 
        /// </summary>
        public int Bottom
        {
            get { return Position.Y; }
        }

        /// <summary>
        /// Gets the top (high Y) edge of the rectangle. 
        /// </summary>
        public int Top
        {
            get { return Position.Y + Size.Y; }
        }
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

        public static Rectangle operator *(Rectangle r, Point p)
        {
            return new Rectangle(r.X * p.X, r.Y * p.Y, r.Width * p.X, r.Height * p.Y);
        }


        public static Rectangle operator /(Rectangle r, Point p)
        {
            return new Rectangle(r.X / p.X, r.Y / p.Y, r.Width / p.X, r.Height / p.Y);
        }

        public static bool operator ==(Rectangle a, Rectangle b)
        {
            return a.Position == b.Position && a.Size == b.Size;
        }

        public static bool operator !=(Rectangle a, Rectangle b)
        {
            return !(a == b);
        }

        public Point FarPosition
        {
            get { return Position + Size; }
        }

        public Vector Center
        {
            get { return Position + (Vector)Size / 2.0; }
        }

        public int Area { get { return Width * Height; } }

        public Rectangle(Point position, Point size)
        {
            this.Position = position;
            this.Size = size;
        }

        public Rectangle(int x, int y, int width, int height)
        {
            this.Position = new Point(x, y);
            this.Size = new Point(width, height);
        }

        /// <summary>
        /// Inflates this rectangle to the right. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Rectangle Inflate(int x, int y)
        {
            return new Rectangle(X, Y, Width + x, Height + y);
        }

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
            var w = Math.Min(rectangle.Width, Width);
            var h = Math.Min(rectangle.Height, Height);
            return new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Returns whether the given point lies inside this rectangle. 
        /// </summary>
        public bool Contains(Point p)
        {
            return Contains(p.X, p.Y);
        }

        /// <summary>
        /// Returns whether the given vector lies inside this rectangle. 
        /// </summary>
        public bool Contains(Vector v)
        {
            return Contains(v.X, v.Y);
        }

        /// <summary>
        /// Returns whether the given coordinates lie inside this rectangle. 
        /// </summary>
        public bool Contains(double x, double y)
        {
            return x >= X && y >= Y && x < (X + Width) && y < (Y + Height);
        }

        public Rectangle MakePositive()
        {
            return new Rectangle(
                Math.Min(X, X + Width),
                Math.Min(Y, Y + Height),
                Math.Abs(Width),
                Math.Abs(Height));
        }

        public override string ToString()
        {
            return "[ {0}, {1}, {2}, {3} ]".F(X, Y, Width, Height);
        }

        public override bool Equals(object obj)
        {
            return (obj is Rectangle) && this == (Rectangle)obj;
        }

        public override int GetHashCode()
        {
            unchecked       // http://stackoverflow.com/questions/5221396/what-is-an-appropriate-gethashcode-algorithm-for-a-2d-point-struct-avoiding
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
