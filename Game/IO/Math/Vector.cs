using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using ProtoBuf;
using Newtonsoft.Json;
using IxSerializer.Modules;
using System.IO;

namespace IO.Common
{
    /// <summary>
    /// Represents a point in 2D space. 
    /// </summary>
    [ProtoContract]
    [JsonObject(MemberSerialization.OptIn)]
    public struct Vector : IxSerializable
    {
        /// <summary>
        /// Deserializes the data from the specified reader into this object.
        /// </summary>
        /// <param name="r"></param>
        public void Deserialize(BinaryReader r)
        {
            X = r.ReadDouble();
            Y = r.ReadDouble();
        }

        /// <summary>
        /// Serializes this object to the given writer.
        /// </summary>
        /// <param name="w"></param>
        public void Serialize(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
        }

        /// <summary>
        /// Gets the vector with both coordinates set to zero. 
        /// </summary>
        public static readonly Vector Zero = new Vector();

        /// <summary>
        /// Gets the vector with both coordinates set to one. 
        /// </summary>
        public static readonly Vector One = new Vector(1);

        /// <summary>
        /// Gets the vector with both coordinates set to <see cref="double.NaN"/>. 
        /// </summary>
        public static readonly Vector NaN = new Vector(double.NaN);

        /// <summary>
        /// Gets the vector with both coordinates set to <see cref="double.MaxValue"/>. 
        /// </summary>
        public static readonly Vector MaxValue = new Vector(double.MaxValue);

        /// <summary>
        /// Gets the vector with both coordinates set to <see cref="double.MinValue"/>. 
        /// </summary>
        public static readonly Vector MinValue = new Vector(double.MinValue);

        /// <summary>
        /// Calculates the distance from this point to the given rectangle. 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public double DistanceTo(RectangleF rect)
        {
            var insRect = Clamp(rect.BottomLeft, rect.TopRight);
            return DistanceTo(insRect);
        }

        /// <summary>
        /// Gets or sets the X coordinate of this vector. 
        /// </summary>
        [ProtoMember(1)]
        [JsonProperty]
        public double X;

        /// <summary>
        /// Gets or sets the Y coordinate of this vector. 
        /// </summary>

        [ProtoMember(2)]
        [JsonProperty]
        public double Y;

        /// <summary>
        /// Gets the Nth dimension of this 2-dimensional vector. 
        /// </summary>
        public double this[int dimension]
        {
            get
            {
                switch (dimension)
                {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new ArgumentOutOfRangeException(nameof(dimension), "Vector dimension must be 0 or 1!");
                }
            }
        }


        /// <summary>
        /// Gets the angle from origin (also <see cref="Vector.Zero"/>) to this point. 
        /// </summary>
        public double Angle => Math.Atan2(Y, X);


        /// <summary>
        /// Creates a new vector with both its X and Y coordinates set to the given value. 
        /// </summary>
        public Vector(double v)
        {
            X = Y = v;
        }

        /// <summary>
        /// Creates a new vector with the given X and Y coordinates. 
        /// </summary>
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
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
        
        /// <summary>
        /// Performs an element-wise comparison of the two vectors. 
        /// </summary>
        public static bool operator ==(Vector a, Vector b)
        {
            return a.X.Equals(b.X) && a.Y.Equals(b.Y);
        }

        /// <summary>
        /// Performs an element-wise negated comparison of the two vectors. 
        /// </summary>
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

        public static implicit operator Vector(Point p) => 
            new Vector(p.X, p.Y);
        #endregion


        #region Unary ops

        /// <summary>
        /// Returns a vector with both of its components negated. 
        /// </summary>
        public static Vector operator -(Vector a) => 
            new Vector(-a.X, -a.Y);


        /// <summary>
        /// Returns whether any of the components of this vector is <see cref="double.NaN"/>. 
        /// </summary>
        public bool IsNan() => 
            double.IsNaN(X) || double.IsNaN(Y);


        /// <summary>
        /// Returns the squared length (L2 norm) of this vector. 
        /// </summary>
        public double LengthSquared() => 
            X * X + Y * Y;

        /// <summary>
        /// Returns the length (L2 norm) of this vector. 
        /// </summary>
        public double Length() => 
            Math.Sqrt(LengthSquared());

        /// <summary>
        /// Returns a new vector of the same angle as this one, and length one. 
        /// </summary>
        public Vector Normalize()
        {
            if (X == 0 && Y == 0)
                return new Vector(1, 0);
            return this / Length();
        }

        /// <summary>
        /// Uses explicit conversion to <see cref="int"/> to convert this vector to a point. 
        /// </summary>
        public Point ToPoint() => 
            new Point((int)X, (int)Y);

        /// <summary>
        /// Uses <see cref="Math.Round(double)"/> to convert this vector to a point. 
        /// </summary>
        public Point Round() => 
            new Point((int)Math.Round(X), (int)Math.Round(Y));

        /// <summary>
        /// Uses <see cref="Math.Floor(double)"/> to convert this vector to a point. 
        /// </summary>
        public Point Floor() => 
            new Point((int)Math.Floor(X), (int)Math.Floor(Y));

        /// <summary>
        /// Uses <see cref="Math.Ceiling(double)"/> to convert this vector to a point. 
        /// </summary>
        public Point Ceiling() => 
            new Point((int)Math.Ceiling(X), (int)Math.Ceiling(Y));

        #endregion


        #region Binary Methods

        /// <summary>
        /// Returns the squared distance between this point and the given point. 
        /// </summary>
        public double DistanceToSquared(Vector other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Returns the distance between this point and the given point. 
        /// </summary>
        public double DistanceTo(Vector other)
        {
            return Math.Sqrt(DistanceToSquared(other));
        }

        /// <summary>
        /// Returns the angle between this point and the given point. 
        /// </summary>
        public double AngleTo(Vector pos)
            => (pos - this).Angle;

        #endregion

        /// <summary>
        /// Returns whether this point is inside the rectangle at the given position and size. 
        /// </summary>
        /// <param name="pos">The position of the bottom-left corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>Whether this point is inside the rectangle. </returns>
        public bool Inside(Vector pos, Vector size)
        {
            return X >= pos.X && Y >= pos.Y && X <= pos.X + size.X && Y <= pos.Y + size.Y;
        }

        /// <summary>
        /// Returns a new point that lies at the given angle and distance from this point. 
        /// </summary>
        /// <param name="angle">The angle from this point to the new point. </param>
        /// <param name="distance">The distance from this point to the new point. </param>
        /// <returns>A new point. </returns>
        public Vector PolarProjection(double angle, double distance)
        {
            return new Vector(X + Math.Cos(angle) * distance, Y + Math.Sin(angle) * distance);
        }

        /// <summary>
        /// Clamps this vector's X and Y values between the X/Y values of the given vectors. 
        /// </summary>
        /// <param name="min">A vector with the minimum X and Y values the vector can take. </param>
        /// <param name="max">A vector with the maximum X and Y values the vector can take. </param>
        /// <returns></returns>
        public Vector Clamp(Vector min, Vector max)
        {
            return new Vector(
                Math.Min(max.X, Math.Max(min.X, X)),
                Math.Min(max.Y, Math.Max(min.Y, Y)));
        }


        #region Object Overrides

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
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

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is Vector) && (Vector)obj == this;
        }

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
        /// <param name="format">The string used to format the X and Y values of the vector.</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(string format) => 
            $"[{X.ToString(format)}, {Y.ToString(format)}]";

        #endregion

    }
}
