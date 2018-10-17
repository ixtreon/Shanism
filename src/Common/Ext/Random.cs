using Ix.Math;
using System;
using System.Numerics;

namespace Shanism.Common
{
    public static class RandomExt
    {

        public static byte NextByte(this Random rnd)
            => (byte)rnd.Next(0, 256);

        public static float NextFloat(this Random rnd)
            => (float)rnd.NextDouble();

        public static float NextFloat(this Random rnd, float maxValue)
            => (float)rnd.NextDouble() * maxValue;

        public static float NextFloat(this Random rnd, float minValue, float maxValue)
            => minValue + (float)rnd.NextDouble() * (maxValue - minValue);

        public static Color NextRgbColor(this Random rnd)
            => new Color(rnd.NextByte(), rnd.NextByte(), rnd.NextByte());


        /// <summary>
        /// Returns a random vector whose elements lie in the range from 0 to 1.
        /// </summary>
        public static Vector2 NextVector(this Random rnd)
            => new Vector2(rnd.NextFloat(), rnd.NextFloat());

        /// <summary>
        /// Returns a random vector which lies in the area between the origin and the given vector.
        /// </summary>
        public static Vector2 NextVector(this Random rnd, Vector2 max)
            => new Vector2(rnd.NextFloat(max.X), rnd.NextFloat(max.Y));

        /// <summary>
        /// Returns a random vector inside the given rectangle.
        /// </summary>
        public static Vector2 NextVector(this Random rnd, RectangleF rect)
            => rect.Position + rnd.NextVector() * rect.Size;

    }
}
