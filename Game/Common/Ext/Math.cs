using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    public static class MathsExt
    {
        /// <summary>
        /// Clamps the given integer between the two supplied values.
        /// </summary>
        /// <param name="val">The value to clamp.</param>
        /// <param name="minVal">The minimum value.</param>
        /// <param name="maxVal">The maximum value.</param>
        /// <returns></returns>
        public static int Clamp(this int val, int minVal, int maxVal) 
            => Math.Min(maxVal, Math.Max(minVal, val));

        /// <summary>
        /// Clamps the given double-precision float between the two supplied values.
        /// </summary>
        /// <param name="val">The value to clamp.</param>
        /// <param name="minVal">The minimum value.</param>
        /// <param name="maxVal">The maximum value.</param>
        /// <returns></returns>
        public static double Clamp(this double val, double minVal, double maxVal)
            => Math.Min(maxVal, Math.Max(minVal, val));

        /// <summary>
        /// Clamps the given single-precision float between the two supplied values.
        /// </summary>
        /// <param name="val">The value to clamp.</param>
        /// <param name="minVal">The minimum value.</param>
        /// <param name="maxVal">The maximum value.</param>
        /// <returns></returns>
        public static float Clamp(this float val, float minVal, float maxVal)
            => Math.Min(maxVal, Math.Max(minVal, val));


        public static bool AlmostEqualTo(this double a, double b, double epsilon = 1E-3)
            => Math.Abs(a - b) <= epsilon;
    }
}
