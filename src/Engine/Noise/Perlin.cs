using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Noise
{
    public class PerlinNoise : IModule
    {
        const int tableSize = 256;
        protected const float OutputRange = 0.70710678118f; //sqrt(2) / 2

        static readonly float[,,] gradient = new float[tableSize, tableSize, 2];

        static PerlinNoise()
        {
            var rnd = new Random(0xC0FFEE);
            for (int i = 0; i < tableSize; i++)
                for (int j = 0; j < tableSize; j++)
                {
                    var ang = rnd.NextDouble() * Math.PI * 2;
                    gradient[i, j, 0] = (float)Math.Cos(ang);
                    gradient[i, j, 1] = (float)Math.Sin(ang);
                }
        }


        protected readonly float scale;

        public PerlinNoise(float scale = 1f)
        {
            this.scale = scale;
        }

        public override float GetValue(float x, float y)
        {
            var pVal = perlin(x, y);
            var outVal = (pVal + OutputRange) / (2 * OutputRange);

            return outVal;
        }

        // Compute Perlin noise at coordinates x, y
        protected float perlin(float x, float y)
        {
            x /= scale;
            y /= scale;

            // Determine grid cell coordinates
            int xMin = (int)Math.Floor(x);
            int yMin = (int)Math.Floor(y);
            int xMax = xMin + 1;
            int yMax = yMin + 1;

            // Determine interpolation weights
            // Could also use higher order polynomial/s-curve here
            float sx = x - xMin;
            float sy = y - yMin;

            // Interpolate between grid point gradients
            float n0, n1, ix0, ix1, value;
            n0 = dotGridGradient(xMin, yMin, x, y);
            n1 = dotGridGradient(xMax, yMin, x, y);
            ix0 = lerp(n0, n1, sx);
            n0 = dotGridGradient(xMin, yMax, x, y);
            n1 = dotGridGradient(xMax, yMax, x, y);
            ix1 = lerp(n0, n1, sx);
            value = lerp(ix0, ix1, sy);

            return value;
        }

        float dotGridGradient(int ix, int iy, float x, float y)
        {
            ix &= 255;
            iy &= 255;

            // Compute the distance vector
            var dx = x - ix;
            var dy = y - iy;

            // Compute the dot-product
            return (dx * gradient[iy, ix, 0] + dy * gradient[iy, ix, 1]);
        }

        static float lerp(float a, float b, float t) => a + t * (b - a);
    }
}
