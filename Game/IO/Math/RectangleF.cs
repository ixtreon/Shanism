using ProtoBuf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Common
{
    /// <summary>
    /// Represents a rectangle in the continuous 2D plane. 
    /// </summary>
    [ProtoContract]
    [JsonObject(MemberSerialization.OptIn)]
    public struct RectangleF
    {
        public static readonly RectangleF Empty = new RectangleF();

        /// <summary>
        /// Gets or sets the position of the bottom-left (low) corner of the rectangle. 
        /// </summary>
        [ProtoMember(1)]
        public Vector Position;


        /// <summary>
        /// Gets or sets the size of the rectangle. 
        /// </summary>
        [ProtoMember(2)]
        public Vector Size;

        [JsonProperty]
        public double X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        [JsonProperty]
        public double Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        [JsonProperty]
        public double Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        [JsonProperty]
        public double Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }

        /// <summary>
        /// Gets the left (low X) edge of the rectangle. 
        /// </summary>
        public double Left
        {
            get { return Position.X; }
        }

        /// <summary>
        /// Gets the right (high X) edge of the rectangle. 
        /// </summary>
        public double Right
        {
            get { return Position.X + Size.X; }
        }

        /// <summary>
        /// Gets the bottom (low Y) edge of the rectangle. 
        /// </summary>
        public double Bottom
        {
            get { return Position.Y; }
        }

        /// <summary>
        /// Gets the top (high Y) edge of the rectangle. 
        /// </summary>
        public double Top
        {
            get { return Position.Y + Size.Y; }
        }


        /// <summary>
        /// Gets the bottom left (low X, low Y) corner of this rectangle. 
        /// </summary>
        public Vector BottomLeft { get { return new Vector(Left, Bottom); } }

        /// <summary>
        /// Gets the top left (low X, high Y) corner of this rectangle. 
        /// </summary>
        public Vector TopLeft {  get { return new Vector(Left, Top); } }

        /// <summary>
        /// Gets the bottom right (high X, low Y) corner of this rectangle. 
        /// </summary>
        public Vector BottomRight { get { return new Vector(Right, Bottom); } }

        /// <summary>
        /// Gets the top right (high X, high Y) corner of this rectangle. 
        /// </summary>
        public Vector TopRight {  get { return new Vector(Right, Top); } }


        public Vector FarPosition { get { return Position + Size; } }

        /// <summary>
        /// Gets the point that lies at the center of this rectangle. 
        /// </summary>
        public Vector Center { get { return Position + Size / 2.0; } }

        public static RectangleF operator *(RectangleF r, Vector p)
        {
            return new RectangleF(r.X * p.X, r.Y * p.Y, r.Width * p.X, r.Height * p.Y);
        }

        public static RectangleF operator /(RectangleF r, Vector p)
        {
            return new RectangleF(r.X / p.X, r.Y / p.Y, r.Width / p.X, r.Height / p.Y);
        }

        public static RectangleF operator +(RectangleF r, Vector p)
        {
            return new RectangleF(r.X + p.X, r.Y + p.Y, r.Width, r.Height);
        }


        public static RectangleF operator *(RectangleF r, double f)
        {
            return new RectangleF(r.X * f, r.Y * f, r.Width * f, r.Height * f);
        }

        public static RectangleF operator -(RectangleF r, double f)
        {
            return new RectangleF(r.X - f, r.Y - f, r.Width, r.Height);
        }

        public static RectangleF operator +(RectangleF r, double f)
        {
            return new RectangleF(r.X + f, r.Y + f, r.Width, r.Height);
        }


        public static implicit operator RectangleF(Rectangle r)
        {
            return new RectangleF(r.Position, r.Size);
        }

        public double Area { get { return Width * Height; } }

        public RectangleF(Vector position, Vector size)
        {
            this.Position = position;
            this.Size = size;
        }

        public RectangleF(double x, double y, double width, double height)
        {
            this.Position = new Vector(x, y);
            this.Size = new Vector(width, height);
        }
        

        /// <summary>
        /// Gets the intersection (common area) of the two rectangles. 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public RectangleF IntersectWith(RectangleF rectangle)
        {
            var x = Math.Max(rectangle.X, X);
            var y = Math.Max(rectangle.Y, Y);
            var w = Math.Min(rectangle.Width, Width);
            var h = Math.Min(rectangle.Height, Height);
            return new RectangleF(x, y, w, h);
        }

        /// <summary>
        /// Returns whether the given point lies inside this rectangle. 
        /// </summary>
        public bool Contains(Vector p)
        {
            return Contains(p.X, p.Y);
        }

        /// <summary>
        /// Returns whether the given coordinates lie inside this rectangle. 
        /// </summary>
        public bool Contains(double x, double y)
        {
            return x >= X && y >= Y && x < (X + Width) && y < (Y + Height);
        }


        public override string ToString()
        {
            return ToString("0.00");
        }

        public string ToString(string format)
        {
            return "[ {0}, {1}, {2}, {3} ]".F(X.ToString(format), Y.ToString(format), Width.ToString(format), Height.ToString(format));
        }
    }
}
