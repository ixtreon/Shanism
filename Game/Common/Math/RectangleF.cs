using ProtoBuf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IxSerializer.Modules;

namespace Shanism.Common
{
    /// <summary>
    /// Represents a rectangle in the continuous 2D plane. 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [JsonObject(MemberSerialization.Fields)]
    public struct RectangleF : IxSerializable
    {
        /// <summary>
        /// Gets the empty rectangle that lies at the origin. 
        /// </summary>
        public static readonly RectangleF Empty = new RectangleF();


        /// <summary>
        /// Deserializes the data from the specified reader into this object.
        /// </summary>
        /// <param name="r"></param>
        public void Deserialize(BinaryReader r)
        {
            _x = r.ReadDouble();
            _y = r.ReadDouble();
            _width = r.ReadDouble();
            _height = r.ReadDouble();
        }

        /// <summary>
        /// Serializes this object to the given writer.
        /// </summary>
        /// <param name="w"></param>
        public void Serialize(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Width);
            w.Write(Height);
        }


        double _x;
        double _y;
        double _width;
        double _height;

        /// <summary>
        /// Gets the position of the bottom-left (low) corner of the rectangle. 
        /// </summary>
        public Vector Position => new Vector(X, Y);

        /// <summary>
        /// Gets the size of the rectangle. 
        /// </summary>
        public Vector Size => new Vector(Width, Height);

        /// <summary>
        /// Gets the low X coordinate of this rectangle. 
        /// </summary>
        public double X => _x;

        /// <summary>
        /// Gets the low Y coordinate of this rectangle. 
        /// </summary>
        public double Y => _y;

        /// <summary>
        /// Gets the width of the rectangle. 
        /// </summary>
        public double Width => _width;

        /// <summary>
        /// Gets the height of the rectangle. 
        /// </summary>
        public double Height => _height;

        /// <summary>
        /// Gets the left (low X) edge of the rectangle. 
        /// </summary>
        public double Left => _x;

        /// <summary>
        /// Gets the right (high X) edge of the rectangle. 
        /// </summary>
        public double Right => _x + _width;

        /// <summary>
        /// Gets the bottom (low Y) edge of the rectangle. 
        /// </summary>
        public double Bottom => _y;

        /// <summary>
        /// Gets the top (high Y) edge of the rectangle. 
        /// </summary>
        public double Top => _y + _height;


        /// <summary>
        /// Gets the top left (low X, low Y) corner of this rectangle. 
        /// </summary>
        public Vector TopLeft => new Vector(Left, Bottom);

        /// <summary>
        /// Gets the top right (high X, low Y) corner of this rectangle. 
        /// </summary>
        public Vector TopRight => new Vector(Right, Bottom);

        /// <summary>
        /// Gets the bottom left (low X, high Y) corner of this rectangle. 
        /// </summary>
        public Vector BottomLeft => new Vector(Left, Top);

        /// <summary>
        /// Gets the bottom right (high X, high Y) corner of this rectangle. 
        /// </summary>
        public Vector BottomRight => new Vector(Right, Top);

        /// <summary>
        /// Gets the top right (high X, high Y) corner of this rectangle. 
        /// </summary>
        public Vector FarPosition => BottomRight;

        /// <summary>
        /// Gets the point at the center of this rectangle. 
        /// </summary>
        public Vector Center => Position + Size / 2.0;

        /// <summary>
        /// Gets the area of this rectangle. 
        /// </summary>
        public double Area => Width * Height;


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

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleF(double x, double y, double width, double height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct
        /// at the specified position and size. 
        /// </summary>
        /// <param name="position">The position of the rectangle. .</param>
        /// <param name="size">The size of the rectangle. .</param>
        public RectangleF(Vector position, Vector size)
        {
            _x = position.X;
            _y = position.Y;
            _width = size.X;
            _height = size.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleF"/> struct 
        /// that is a copy of the given rectangle. 
        /// </summary>
        /// <param name="src">The source rectangle.</param>
        public RectangleF(RectangleF src)
        {
            _x = src._x;
            _y = src._y;
            _width = src._width;
            _height = src._height;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)_x, (int)_y, (int)_width, (int)_height);
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
            var farX = Math.Min(rectangle.Right, Right);
            var farY = Math.Min(rectangle.Top, Top);

            return new RectangleF(x, y, farX - x, farY - y);
        }

        /// <summary>
        /// Returns a new rectangle 
        /// with positive <see cref="Width"/> and <see cref="Height"/>. 
        /// </summary>
        public RectangleF MakePositive()
        {
            return new RectangleF(
                Math.Min(X, X + Width),
                Math.Min(Y, Y + Height),
                Math.Abs(Width),
                Math.Abs(Height));
        }

        /// <summary>
        /// Inflates the rectangle in all direction by the specified amount. 
        /// </summary>
        public RectangleF Inflate(double v)
        {
            return new RectangleF(_x - v, _y - v, _width + 2 * v, _height + 2 * v);
        }

        /// <summary>
        /// Returns whether the given coordinates lie inside this rectangle. 
        /// </summary>
        public bool Contains(double x, double y)
        {
            return x >= X && y >= Y && x < (X + Width) && y < (Y + Height);
        }

        /// <summary>
        /// Returns whether the given point lies inside this rectangle. 
        /// </summary>
        public bool Contains(Vector p) => Contains(p.X, p.Y);


        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => ToString("0.00");

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
            => $"[{X.ToString(format)}, {Y.ToString(format)}, {Width.ToString(format)}, {Height.ToString(format)}]";
    }
}
