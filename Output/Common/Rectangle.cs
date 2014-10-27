using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Common
{
    public struct Rectangle
    {
        public Point Position;

        public Point Size;

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
    }
}
