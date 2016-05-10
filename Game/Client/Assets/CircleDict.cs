using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Assets
{
    class CircleDict
    {
        readonly GraphicsDevice graphicsDevice;

        readonly Texture2D[] circles;

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
        public CircleDict(GraphicsDevice gd, int maxSize)
        {
            if (gd == null) throw new ArgumentNullException(nameof(gd));
            if (maxSize < 1) throw new ArgumentOutOfRangeException(nameof(maxSize));

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
                circles[id] = drawCircle(id);

            return circles[id];
        }

        Texture2D drawCircle(int id)
        {
            var texSz = getSz(id);

            var radius = texSz / 2 - 1;
            var data = new Color[texSz * texSz];
            var texture = new Texture2D(graphicsDevice, texSz, texSz);

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            var angleStep = 1.0 / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * texSz + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }
    }
}
