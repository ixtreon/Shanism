using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Noise
{
    public abstract class IModule
    {

        public void Generate(int width, int height, byte[,] arr)
        {
            ensureArraySize(width, height, arr);

            for(int ix = 0; ix < width; ix++)
                for(int iy = 0; iy < height; iy++)
                    arr[ix, iy] = (byte)(GetValue(ix, iy) * 255);
        }

        public byte[,] Generate(int width, int height)
        {
            var arr = new byte[width, height];
            Generate(width, height, arr);

            return arr;
        }

        public abstract float GetValue(float x, float y);

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
