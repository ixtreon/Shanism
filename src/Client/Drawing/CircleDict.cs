using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Drawing
{
    class CircleDict
    {
        readonly GraphicsDevice graphicsDevice;

        readonly Texture2D[] circles;

        readonly int CircleWidth;

        /// <summary>
        /// Gets the maximum diameter of a circle. 
        /// </summary>
        public int MaximumSize { get; }

        /// <summary>
        /// Creates a new <see cref="CircleDict"/> that can draw and cache circles up to a specified size (diameter). 
        /// 
        /// </summary>
        /// <param name="gd">The graphics device on which circle textures are to be created. </param>
        /// <param name="maxSize">The maximum size (diameter) of a circle. 
        /// If this value is not a power-of-two it is replaced with the closest power of two larger than it. </param>
        public CircleDict(GraphicsDevice gd, int maxSize, int circleWidth)
        {
            if (gd == null) throw new ArgumentNullException(nameof(gd));
            if (maxSize < 1) throw new ArgumentOutOfRangeException(nameof(maxSize));

            CircleWidth = circleWidth;

            var lastId = getId(maxSize);
            MaximumSize = 1 << lastId;

            graphicsDevice = gd;
            circles = new Texture2D[lastId + 1];
        }

        static int getId(int sz) => (int)Math.Round(Math.Log(sz, 2));

        static int getSz(int id) => 1 << id;

        public Texture2D GetTexture(int sz)
        {
            if (sz < 1 || sz > MaximumSize)
                throw new ArgumentOutOfRangeException(nameof(sz), $"Size must be between 1 and {nameof(MaximumSize)} ({MaximumSize}). ");

            var id = getId(sz);
            if (circles[id] == null)
            {
                var rad = getSz(id) / 2;
                circles[id] = fillCircle(rad);
            }

            return circles[id];
        }

        Texture2D fillCircle(int rad)
        {
            var sz = rad * 2 + 1;
            var data = new Color[sz * sz];
            var texture = new Texture2D(graphicsDevice, sz, sz);

            int x = 0, y = rad, dp = 1 - rad;

            do
            {
                for (int iy = 0; iy <= y; iy++)
                    putPixel(data, x, iy, rad, sz);

                circleStep(ref x, ref y, ref dp);
            } while (x <= y);

            texture.SetData(data);
            return texture;
        }

        Texture2D drawCircle(int rad, int w)
        {
            var sz = rad * 2 + 1;
            var data = new Color[sz * sz];
            var texture = new Texture2D(graphicsDevice, sz, sz);

            int xBig = 0, yBig = rad, dpBig = 1 - rad;
            int xSmall = 0, ySmall = rad - w, dpSmall = 1 - (rad - w);

            do
            {
                for (int iy = ySmall; iy <= yBig; iy++)
                    putPixel(data, xBig, iy, rad, sz);

                circleStep(ref xBig, ref yBig, ref dpBig);
                circleStep(ref xSmall, ref ySmall, ref dpSmall);
            } while (xBig <= yBig);

            texture.SetData(data);
            return texture;
        }

        static void circleStep(ref int x, ref int y, ref int dp)
        {
            if (dp < 0)
                dp = dp + 2 * (++x) + 3;
            else
                dp = dp + 2 * (++x) - 2 * (--y) + 5;
        }

        void putPixel(Color[] data, int x, int y, int offset, int sz)
        {
            var x1 = (offset + x);
            var x2 = (offset - x);
            var y1 = (offset + y);
            var y2 = (offset - y);

            data[x1 * sz + y1] = Color.White;
            data[x1 * sz + y2] = Color.White;
            data[x2 * sz + y1] = Color.White;
            data[x2 * sz + y2] = Color.White;
            data[y1 * sz + x1] = Color.White;
            data[y1 * sz + x2] = Color.White;
            data[y2 * sz + x1] = Color.White;
            data[y2 * sz + x2] = Color.White;
        }
    }
}
