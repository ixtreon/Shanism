using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Noise
{
    public class WhiteNoise : IModule
    {
        readonly int seed;

        public WhiteNoise(int seed)
        {
            this.seed = seed;
        }

        public override float GetValue(float x, float y)
        {
            unchecked
            {
                uint hash = 23;
                hash = hash * 31 + (uint)x.GetHashCode();
                hash = hash * 31 + (uint)y.GetHashCode();
                hash = hash * 31 + (uint)seed;

                return (float)((double)hash / uint.MaxValue);
            }
        }

    }
}
