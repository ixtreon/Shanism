using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Noise
{
    public class WhiteNoise : IModule
    {

        protected override void generate(int width, int height, byte[,] arr, int seed)
        {
            var rnd = new Random(seed);
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    arr[i, j] = (byte)rnd.Next(0, 256);
        }
    }
}
