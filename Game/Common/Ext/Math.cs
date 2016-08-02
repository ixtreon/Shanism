using Shanism.Common.Game;
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
        /// <param name="i">The value to clamp.</param>
        /// <param name="minVal">The minimum value.</param>
        /// <param name="maxVal">The maximum value.</param>
        /// <returns></returns>
        public static int Clamp(this int i, int minVal, int maxVal) 
            => Math.Min(maxVal, Math.Max(minVal, i));

        /// <summary>
        /// Clamps the specified minimum value.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="minVal">The minimum value.</param>
        /// <param name="maxVal">The maximum value.</param>
        /// <returns></returns>
        public static double Clamp(this double i, double minVal, double maxVal) 
            => Math.Min(maxVal, Math.Max(minVal, i));


        public static bool AlmostEqualTo(this double a, double b, double epsilon = 1E-3)
            => Math.Abs(a - b) <= epsilon;
    }
}
