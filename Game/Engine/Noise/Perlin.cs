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
        static readonly float Range = (float)Math.Sqrt(2) / 2;

        static readonly float[,,] gradient = new float[tableSize, tableSize, 2];

        static PerlinNoise()
        {
            var rnd = new Random(123);
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



        protected override void generate(int width, int height, byte[,] arr, int seed)
        {
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    var x = (float)i / width / scale;
                    var y = (float)j / height / scale;

                    var fVal = perlin(x, y);
                    var bVal = (fVal + Range) * 256 / (2 * Range);

                    arr[i, j] = (byte)bVal.Clamp(0, 255);
                }
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

        // Compute Perlin noise at coordinates x, y
        protected float perlin(float x, float y)
        {

            // Determine grid cell coordinates
            int x0 = (x > 0.0 ? (int)x : (int)x - 1);
            int x1 = x0 + 1;
            int y0 = (y > 0.0 ? (int)y : (int)y - 1);
            int y1 = y0 + 1;

            // Determine interpolation weights
            // Could also use higher order polynomial/s-curve here
            float sx = x - x0;
            float sy = y - y0;

            // Interpolate between grid point gradients
            float n0, n1, ix0, ix1, value;
            n0 = dotGridGradient(x0, y0, x, y);
            n1 = dotGridGradient(x1, y0, x, y);
            ix0 = lerp(n0, n1, sx);
            n0 = dotGridGradient(x0, y1, x, y);
            n1 = dotGridGradient(x1, y1, x, y);
            ix1 = lerp(n0, n1, sx);
            value = lerp(ix0, ix1, sy);

            return value;
        }

        static float lerp(float a, float b, float t) => a + t * (b - a);
    }
}
