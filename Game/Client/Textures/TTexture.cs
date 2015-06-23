using Client.Sprites;
using IO.Common;
using IO.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Textures
{
    /// <summary>
    /// An instance of a <see cref="IO.Content.TextureDef"/>. Provides methods to access amd draw the whole texture or parts of it. 
    /// </summary>
    class TTexture
    {
        /// <summary>
        /// Gets the texture definition for this texture. 
        /// </summary>
        public readonly TextureDef File;

        /// <summary>
        /// Gets the underlying XNA Texture2D object used to draw the texture to the screen. 
        /// </summary>
        public readonly Texture2D Texture;

        /// <summary>
        /// Gets the total amount of segments in this texture. 
        /// </summary>
        public readonly int Count;

        /// <summary>
        /// Gets the width of each segment in this texture. 
        /// </summary>
        public readonly int PieceWidth;

        /// <summary>
        /// Gets the height of each segment in this texture. 
        /// </summary>
        public readonly int PieceHeight;

        /// <summary>
        /// Gets the number of segments in each dimension. 
        /// </summary>
        public Point Size
        {
            get { return File.Splits; }
        }


        /// <summary>
        /// Creates a new TTexture from the given texture definition. 
        /// </summary>
        public TTexture(TextureDef f)
        {
            File = f;
            Texture = TextureCache.Get(f.Name);

            this.Count = Size.X * Size.Y;

            this.PieceWidth = Texture.Width / Size.X;
            this.PieceHeight = Texture.Height / Size.Y;
        }
        

        /// <summary>
        /// Gets the rectangle in pixels that corresponds to the given texture rectangle in texture cuts / ids. 
        /// </summary>
        /// <param name="textureSpan"></param>
        /// <returns></returns>
        public Rectangle GetTileRect(Rectangle textureSpan)
        {
            return new Rectangle(PieceWidth * textureSpan.X, PieceHeight * textureSpan.Y, PieceWidth * textureSpan.Width, PieceHeight * textureSpan.Height);
        }

        public Rectangle GetTileRect(Point p)
        {
            return GetTileRect(new Rectangle(p.X, p.Y, 1, 1));
        }

        public Rectangle GetTileRect(int id)
        {
            var p = GetTile(id);
            return GetTileRect(p);
        }

        public Point GetTile(int id)
        {
            return new Point(id % Size.X, id / Size.X);
        }

        /// <summary>
        /// Gets a rectangle spanning the whole texture. 
        /// </summary>
        /// <returns></returns>
        public Rectangle GetWholeRect()
        {
            return GetTileRect(new Rectangle(Point.Empty, Size));
        }
    }
}
