using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using IxSerializer.Attributes;

namespace IO.Common
{
    [SerialKiller]
    public struct Vector
    {
        /// <summary>
        /// Gets the vector with both coordinates set to zero. 
        /// </summary>
        public static readonly Vector Zero = new Vector();

        /// <summary>
        /// Gets the vector with both coordinates set to <see cref="double.NaN"/>. 
        /// </summary>
        public static readonly Vector NaN = new Vector(double.NaN);

        [SerialMember]
        double x;
        [SerialMember]
        double y;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Angle
        {
            get
            {
                return Zero.AngleTo(this);
            }
        }


        public Vector(double v)
        {
            x = y = v;
        }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }


        #region Operator Overloads
        /// <summary>
        /// Performs an element-wise addition on the two vectors. 
        /// </summary>
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Performs an element-wise subtraction on the two vectors. 
        /// </summary>
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Performs an element-wise division on the two vectors. 
        /// </summary>
        public static Vector operator /(Vector a, Vector b)
        {
            return new Vector(a.X / b.X, a.Y / b.Y);
        }

        /// <summary>
        /// Performs an element-wise multiplication on the two vectors. 
        /// </summary>
        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector(a.X * b.X, a.Y * b.Y);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.X.Equals(b.X) && a.Y.Equals(b.Y);
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !a.X.Equals(b.X) || !a.Y.Equals(b.Y);
        }

        /// <summary>
        /// Performs element-wise subtraction from the given vector with the provided value. 
        /// </summary>
        public static Vector operator -(Vector a, double v)
        {
            return new Vector(a.X - v, a.Y - v);
        }

        /// <summary>
        /// Performs element-wise addition from the given vector with the provided value. 
        /// </summary>
        public static Vector operator +(Vector a, double v)
        {
            return new Vector(a.X + v, a.Y + v);
        }

        public static Vector operator *(Vector a, double mult)
        {
            return new Vector(a.X * mult, a.Y * mult);
        }

        public static Vector operator /(Vector a, double mult)
        {
            return new Vector(a.X / mult, a.Y / mult);
        }


        public static implicit operator Vector(Point p)
        {
            return new Vector(p.X, p.Y);
        }
        #endregion


        #region Unary ops

        /// <summary>
        /// Returns whether any of the components of this vector is <see cref="double.NaN"/>. 
        /// </summary>
        /// <returns></returns>
        public bool IsNan()
        {
            return double.IsNaN(x) || double.IsNaN(y);
        }


        /// <summary>
        /// Returns the squared length (L2 norm) of this vector. 
        /// </summary>
        public double LengthSquared()
        {
            return X * X + Y * Y;
        }

        /// <summary>
        /// Returns the length (L2 norm) of this vector. 
        /// </summary>
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        /// <summary>
        /// Returns a new vector of the same angle as this one, and length one. 
        /// </summary>
        public Vector Normalize()
        {
            var l = this.Length();
            return (l != 0) ? (this / this.Length()) : this;
        }

        /// <summary>
        /// Uses raw conversion to <see cref="int"/> to convert this vector to a point. 
        /// </summary>
        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }

        /// <summary>
        /// Uses <see cref="Math.Round(double)"/> to convert this vector to a point. 
        /// </summary>
        public Point Round()
        {
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        /// <summary>
        /// Uses <see cref="Math.Floor(double)"/> to convert this vector to a point. 
        /// </summary>
        public Point Floor()
        {
            return new Point((int)Math.Floor(x), (int)Math.Floor(y));
        }

        /// <summary>
        /// Uses <see cref="Math.Ceiling(double)"/> to convert this vector to a point. 
        /// </summary>
        public Point Ceiling()
        {
            return new Point((int)Math.Ceiling(x), (int)Math.Ceiling(y));
        }

        #endregion


        #region Binary ops

        public double DistanceToSquared(Vector other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return dx * dx + dy * dy;
        }

        public double DistanceTo(Vector other)
        {
            return Math.Sqrt(DistanceToSquared(other));
        }

        public double AngleTo(Vector pos)
        {
            return Math.Atan2(pos.y - y, pos.x - x);
        }

        #endregion


        public bool Inside(Vector pos, Vector size)
        {
            return X >= pos.x && y >= pos.y && x <= pos.x + size.x && y <= pos.y + size.y;
        }

        public Vector PolarProjection(double angle, double distance)
        {
            return new Vector(x + Math.Cos(angle) * distance, y + Math.Sin(angle) * distance);
        }

        /// <summary>
        /// Clamps this vector's X and Y values between the X/Y values of the given vectors. 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Vector Clamp(Vector min, Vector max)
        {
            return new Vector(
                Math.Min(max.x, Math.Max(min.x, x)),
                Math.Min(max.y, Math.Max(min.y, y)));
        }


        #region Object Overrides
        public override string ToString()
        {
            return string.Format("[{0}, {1}", x.ToString("0.00"), y.ToString("0.00"));
        }

        public override int GetHashCode()
        {
            unchecked       // http://stackoverflow.com/questions/5221396/what-is-an-appropriate-gethashcode-algorithm-for-a-2d-point-struct-avoiding
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector))
                return false;
            return (Vector)obj == this;
        }

        #endregion

    }
}
