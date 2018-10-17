using Ix.Math;
using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client.Sprites
{
    /// <summary>
    /// A texture or a part of it, which can be rotated and flipped in the 2D plane. 
    /// </summary>
    public class Sprite
    {
        public string Name { get; protected set; }

        public Texture2D Texture { get; protected set; }

        /// <summary>
        /// Gets or sets the source rectangle drawn from this texture, in pixels units.
        /// </summary>
        public Rectangle SourceRectangle { get; protected set; }

        /// <summary>
        /// Gets or sets whether the sprite is flipped horizontally.
        /// </summary>
        public bool FlipHorizontal { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the texture in radians.
        /// </summary>
        public float Orientation { get; set; }


        public SpriteEffects SpriteEffects 
            => FlipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        protected Sprite() { }

        public Sprite(Texture2D texture, Rectangle sourceRect, string name)
        {
            Texture = texture;
            Name = name;
            SourceRectangle = sourceRect;
        }

    }
}
