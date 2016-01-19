using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public static class MathsExt
    {
        public static long MakeLong(int a, int b)
        {
            return (((long)a << 32) | (uint)b);
        }


        public static int Clamp(this int i, int min, int max)
        {
            return Math.Min(max, Math.Max(min, i));
        }

        public static double Clamp(this double i, double min, double max)
        {
            return Math.Min(max, Math.Max(min, i));
        }


        public static bool AlmostEqualTo(this double a, double b, double epsilon = 1E-6)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        #region System.Random Extensions
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        /// <summary>
        /// Returns a vector with random X and Y values in the range [0;1]
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Vector NextVector(this Random r)
        {
            return new Vector(r.NextDouble(), r.NextDouble());
        }
        #endregion
    }
}
