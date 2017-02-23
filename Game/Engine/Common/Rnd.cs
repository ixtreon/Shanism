using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Engine
{
    /// <summary>
    /// Represents a static instance of a (simple) pseudo-random number generator, 
    /// a device that produces a sequence of numbers 
    /// that meet certain (weak) statistical requirements for randomness.
    /// </summary>
    public static class Rnd
    {
        static Random rnd = new Random();


        /// <summary>
        /// Returns a random number within a specified range. 
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. If minValue
        /// equals maxValue, minValue is returned.</returns>
        public static int Next(int minValue = 0, int maxValue = int.MaxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random number within a specified range. 
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        /// A double-precision value greater than or equal to minValue and less than maxValue;
        /// </returns>
        public static double NextDouble(double minValue = 0, double maxValue = 1)
            => rnd.NextDouble() * (maxValue - minValue) + minValue;


        /// <summary>
        /// Returns a random number within a specified range. 
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        /// A single-precision value greater than or equal to minValue and less than maxValue;
        /// </returns>
        public static float NextFloat(float minValue = 0, float maxValue = 1)
            => (float)rnd.NextDouble() * (maxValue - minValue) + minValue;

        /// <summary>
        /// Generates a random angle between 0 and 2*PI. 
        /// </summary>
        public static double NextAngle()
        {
            return rnd.NextDouble() * Math.PI * 2;
        }

        /// <summary>
        /// Generates a point drawn uniformly from the given rectangle. 
        /// </summary>
        public static Vector PointInside(Rectangle rect)
        {
            return new Vector(NextDouble(rect.Left, rect.Right), NextDouble(rect.Bottom, rect.Top));
        }

        /// <summary>
        /// Generates a point drawn uniformly from the given rectangle. 
        /// </summary>
        public static Vector PointInside(RectangleF rect)
        {
            return new Vector(NextDouble(rect.Left, rect.Right), NextDouble(rect.Bottom, rect.Top));
        }

        /// <summary>
        /// Generates a value drawn from the specified normal distribution. 
        /// Uses a box-muller transform to generate the output.
        /// </summary>
        /// <param name="mean">The mean of the normal distribution.</param>
        /// <param name="sigma">The standard deviation of the normal distribution.</param>
        public static double NextGaussian(double mean, double sigma)
        {
            var u1 = 1.0 - NextDouble(); //uniform(0,1] random doubles
            var u2 = 1.0 - NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
             Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            return mean + sigma * randStdNormal; //random normal(mean,stdDev^2)
        }

        /// <summary>
        /// Generates a uniformly drawn point lying inside the given circle. 
        /// </summary>
        /// <param name="origin">The origin of the circle. </param>
        /// <param name="radius">The radius of the circle. </param>
        /// <returns>A point, drawn at uniform, inside the circle. </returns>
        public static Vector PointInCircle(Vector origin, double radius)
        {
            // http://stackoverflow.com/questions/5837572/generate-a-random-point-within-a-circle-uniformly
            var ang = NextAngle();
            var dist = NextDouble() + NextDouble();
            if (dist > 1) dist = 2 - dist;

            return origin + new Vector(dist * Math.Cos(ang) * radius, dist * Math.Sin(ang) * radius);
        }
    }
}
