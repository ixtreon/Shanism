using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Noise
{
    public class RidgedMultiNoise : PerlinNoise
    {
        public RidgedMultiNoise(float scale = 1f)
            : base(scale)
        {
        }


        protected override void generate(int width, int height, byte[,] arr, int seed)
        {
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    var x = (float)i / width / scale;
                    var y = (float)j / height / scale;

                    var fVal = Math.Abs(perlin(x, y));
                    var bVal = (fVal) * 256;

                    arr[i, j] = (byte)bVal.Clamp(0, 255);
                }
        }
    }
}
