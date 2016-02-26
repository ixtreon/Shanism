using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Assets
{
    class CircleDict
    {
        readonly GraphicsDevice graphicsDevice;

        readonly Texture2D[] circles;

        public int MinimumSize { get; }

        public int MaximumSize { get; }


        public CircleDict(int minSize, int maxSize, GraphicsDevice gd)
        {
            if (gd == null) throw new ArgumentNullException(nameof(gd));
            if (minSize < 1) throw new ArgumentOutOfRangeException(nameof(minSize));
            if (maxSize < minSize) throw new ArgumentOutOfRangeException(nameof(maxSize));

            graphicsDevice = gd;

            var minBytes = (int)Math.Log(minSize, 2);
            var maxBytes = (int)Math.Log(maxSize, 2);

            MinimumSize = 1 << minBytes;
            MaximumSize = 1 << maxBytes;

            circles = new Texture2D[maxBytes + 1];
        }

        public Texture2D GetTexture(int sz)
        {
            if (sz < MinimumSize) throw new ArgumentOutOfRangeException(nameof(sz), $"Size must be greater than or equal to {MinimumSize}");
            if (sz > MaximumSize) throw new ArgumentOutOfRangeException(nameof(sz), $"Size must be less than or equal to {MaximumSize}");

            var id = (int)Math.Round(Math.Log(sz, 2));
            if (circles[id] == null)
            {
                var pow2sz = 1 << id;
                circles[id] = drawCircle(pow2sz);
            }
            return circles[id];
        }

        Texture2D drawCircle(int texSz)
        {
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
