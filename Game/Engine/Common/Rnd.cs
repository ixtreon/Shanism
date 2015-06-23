using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Common
{
    public class Rnd
    {
        private static Random rnd = new Random();

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

        public static double NextDouble(double minValue = 0, double maxValue = 1)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
        }
        public static double NextAngle()
        {
            return rnd.NextDouble() * Math.PI * 2;
        }
    }
}
