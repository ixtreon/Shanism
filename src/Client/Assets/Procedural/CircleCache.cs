using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shanism.Client.Systems
{

    /// <summary>
    /// A lookup of circle textures of various sizes.
    /// </summary>
    public class CircleCache : MipMapCache
    {

        readonly Func<int, int> borderWidthFunc;

        /// <summary>
        /// Creates a new cache that draws circles up to a specified size (diameter). 
        /// </summary>
        /// <param name="graphics">The graphics device on which circle textures are to be created. </param>
        /// <param name="maxSize">The maximum size (diameter) of a circle. 
        /// If this value is not a power-of-two it is replaced with the closest power of two larger than it. </param>
        public CircleCache(GraphicsDevice graphics, int maxSize, Func<int, int> borderWidthFunc, bool preload = false)
            : base(graphics, maxSize)
        {
            this.borderWidthFunc = borderWidthFunc ?? throw new Exception();
            if (preload)
                PreloadTextures();
        }


        protected override void DrawTexture(Texture2D tex)
        {
            if(tex.Width <= 1)
                return;

            var sz = tex.Width;
            var r = sz / 2;
            var texMaker = new TextureMaker(sz, sz);
            var outer = new CircleWalker(r - 1);
            var inner = new CircleWalker(Math.Max(0, r - borderWidthFunc(sz) - 1));

            do
            {
                var c = outer.Y - inner.Y;

                int xMin = r - (outer.X + 1);
                int xMax = r + outer.X;
                int yMin = r - (outer.Y + 1);
                int yMax = r + (inner.Y + 1);

                texMaker.VLine(xMax, yMax, c);
                texMaker.VLine(xMin, yMax, c);
                texMaker.VLine(xMax, yMin, c);
                texMaker.VLine(xMin, yMin, c);

                texMaker.HLine(yMax, xMax, c);
                texMaker.HLine(yMax, xMin, c);
                texMaker.HLine(yMin, xMax, c);
                texMaker.HLine(yMin, xMin, c);

                inner.Step();
            }
            while(outer.Step());

            texMaker.WriteTo(tex);
        }
    }
}
