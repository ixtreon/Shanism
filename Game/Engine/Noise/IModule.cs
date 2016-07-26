using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Noise
{
    public abstract class IModule
    {
        protected abstract void generate(int width, int height, byte[,] arr, int seed);

        public void Generate(int width, int height, byte[,] arr, int? seed = null)
        {
            ensureArraySize(width, height, arr);
            generate(width, height, arr, seed ?? Rnd.Next());
        }

        public byte[,] Generate(int width, int height, int? seed = null)
        {
            var arr = new byte[width, height];
            generate(width, height, arr, seed ?? Rnd.Next());
            return arr;
        }

        protected void ensureArraySize(int width, int height, byte[,] arr)
        {
            if (arr == null)
                throw new ArgumentNullException(nameof(arr));
            if (arr.GetLength(0) < width)
                throw new ArgumentException($"Array must be at least {width} elements wide.", nameof(arr));
            if (arr.GetLength(1) < height)
                throw new ArgumentException($"Array must be at least {height} elements high.", nameof(arr));
        }
    }
}
