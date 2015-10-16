using IxSerializer.Attributes;
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
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle();

        /// <summary>
        /// Gets or sets the position of the top-left corner of the rectangle. 
        /// </summary>
        [SerialMember]
        public Point Position;

        /// <summary>
        /// Gets or sets the size of the rectangle. 
        /// </summary>
        [SerialMember]
        public Point Size;

        public int X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public int Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public int Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
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

        public static Rectangle operator *(Rectangle r, Point p)
        {
            return new Rectangle(r.X * p.X, r.Y * p.Y, r.Width * p.X, r.Height * p.Y);
        }

        public static Rectangle operator /(Rectangle r, Point p)
        {
            return new Rectangle(r.X / p.X, r.Y / p.Y, r.Width / p.X, r.Height / p.Y);
        }

        public Point FarPosition
        {
            get { return Position + Size; }
        }

        public Point Center
        {
            get { return Position + Size / 2; }
        }

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

        public IEnumerable<Point> Iterate()
        {
            for (int ix = 0; ix < Size.X; ix++)
                for (int iy = 0; iy < Size.Y; iy++)
                    yield return new Point(X + ix, Y + iy);
        }

        public bool Contains(Point p)
        {
            return p.X >= X && p.Y >= Y && p.X < (X + Width) && p.Y < (Y + Height);
        }
    }
}
