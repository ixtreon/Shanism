using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Content;
using Shanism.Common.Util;
using System;
using System.Numerics;

namespace Shanism.Client
{
    /// <summary>
    /// A GPU texture optionally divided into a number of equally-sized cells.
    /// </summary>
    public class ShanoTexture
    {
        public string Name { get; set; }

        /// <summary>
        /// Gets the underlying texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets the number of cells (logical divisions) 
        /// along each axis of the texture.
        /// </summary>
        public Point CellCount { get; set; }

        /// <summary>
        /// Gets the size of the texture in pixels.
        /// </summary>
        public Vector2 Size => new Vector2(Texture.Width, Texture.Height);

        /// <summary>
        /// Gets the size of each cell (logical division) in pixels.
        /// </summary>
        public Vector2 CellSize => Size / Vector2.Max(Vector2.One, CellCount);


        public ShanoTexture(Texture2D tex, TextureDef def)
        {
            Name = ShanoPath.NormalizeTexture(def.Name);
            Texture = tex ?? throw new ArgumentNullException(nameof(tex));

            CellCount = def.Cells;
        }

        public Rectangle Bounds => new Rectangle(0, 0, Texture.Width, Texture.Height);

        //public static implicit operator Texture2D(ShanoTexture tex)
        //    => tex?.Texture;
    }
}
