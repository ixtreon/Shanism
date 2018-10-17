using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client.Systems
{

    /// <summary>
    /// A lookup of circle textures of various sizes.
    /// </summary>
    public class DiskCache : MipMapCache
    {

        /// <summary>
        /// Creates a new cache that draws circles up to a specified size (diameter). 
        /// </summary>
        /// <param name="graphics">The graphics device on which circle textures are to be created. </param>
        /// <param name="maxSize">The maximum size (diameter) of a circle. 
        /// If this value is not a power-of-two it is replaced with the closest power of two larger than it. </param>
        public DiskCache(GraphicsDevice graphics, int maxSize) : base(graphics, maxSize) { }

        protected override void DrawTexture(Texture2D tex)
        {
            if(tex.Width <= 1)
                return;

            var sz = tex.Width;
            var r = sz / 2;
            var texGen = new TextureMaker(sz, sz);
            var walker = new CircleWalker(r - 1);

            do
            {
                var xMin = r - walker.X - 1;
                var xMax = r + walker.X;
                var yMin = r - walker.Y - 1;
                var yMax = r + walker.Y;

                var xCount = xMax - xMin + 1;
                var yCount = yMax - yMin + 1;

                texGen.HLine(xMin, yMin, xCount);
                texGen.HLine(xMin, yMax, xCount);
                texGen.HLine(yMin, xMin, yCount);
                texGen.HLine(yMin, xMax, yCount);
            }
            while(walker.Step());

            texGen.WriteTo(tex);
        }
    }
}
