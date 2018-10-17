using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shanism.Client.Systems
{

    /// <summary>
    /// A lookup of textures of various sizes.
    /// </summary>
    public abstract class MipMapCache
    {
        protected static int GetID(int sz) => (int)Math.Round(Math.Log(sz, 2));
        protected static int GetSize(int id) => 1 << id;


        readonly GraphicsDevice graphics;
        readonly Texture2D[] cache;

        /// <summary>
        /// Gets the maximum width of a texture. 
        /// </summary>
        public int MaximumSize { get; }

        /// <summary>
        /// Creates a new cache that draws circles up to a specified size (diameter). 
        /// </summary>
        /// <param name="graphics">The graphics device on which circle textures are to be created. </param>
        /// <param name="maxSize">The maximum size (diameter) of a circle. 
        /// If this value is not a power-of-two it is replaced with the closest power of two larger than it. </param>
        public MipMapCache(GraphicsDevice graphics, int maxSize)
        {
            if(maxSize < 1)
                throw new ArgumentOutOfRangeException(nameof(maxSize));

            this.graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));

            var lastID = GetID(maxSize);

            MaximumSize = GetSize(lastID);
            cache = new Texture2D[lastID + 1];
        }

        public void PreloadTextures()
        {
            for(int id = 0; id < cache.Length; id++)
                if(cache[id] == null)
                    cache[id] = CreateTexture(id);
        }


        public Texture2D GetTexture(float requestedSize)
        {
            var id = GetID((int)Math.Ceiling(requestedSize));

            if(cache[id] == null)
                cache[id] = CreateTexture(id);

            return cache[id];
        }

        Texture2D CreateTexture(int id)
        {
            var size = GetSize(id);
            var tex = new Texture2D(graphics, size, size, false, SurfaceFormat.Color);

            DrawTexture(tex);

            return tex;
        }

        protected abstract void DrawTexture(Texture2D tex);

    }
}
