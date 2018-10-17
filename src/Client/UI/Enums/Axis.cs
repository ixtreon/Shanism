using System;
using System.Numerics;

namespace Shanism.Client
{
    public enum Axis
    {
        Vertical, Horizontal
    }

    public static class AxisExt
    {
        /// <summary>
        /// Gets the value of this vector along the given axis.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static float Get(this Vector2 vec, Axis axis)
        {
            switch (axis)
            {
                case Axis.Horizontal:
                    return vec.X;

                case Axis.Vertical:
                    return vec.Y;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Flips this axis by 90 degrees.
        /// </summary>
        /// <returns></returns>
        public static Axis Flip(this Axis axis)
        {
            switch (axis)
            {
                case Axis.Vertical:
                    return Axis.Horizontal;

                case Axis.Horizontal:
                    return Axis.Vertical;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Flips this axis by 90 degrees.
        /// </summary>
        /// <returns></returns>
        public static Vector2 Unit(this Axis axis)
        {
            switch (axis)
            {
                case Axis.Vertical:
                    return Vector2.UnitY;

                case Axis.Horizontal:
                    return Vector2.UnitX;
            }
            throw new NotImplementedException();
        }
    }
}
