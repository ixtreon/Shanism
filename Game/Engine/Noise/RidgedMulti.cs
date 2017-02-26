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

        public override float GetValue(float x, float y)
        {
            var pVal = perlin(x, y);
            var outVal = Math.Abs(pVal) / OutputRange;

            return outVal;
        }
    }
}
