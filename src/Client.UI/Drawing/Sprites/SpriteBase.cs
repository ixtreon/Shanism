using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Sprites
{
    /// <summary>
    /// The visual representation of a <see cref="IEntity"/>. 
    /// Tracks the model and animation of a single <see cref="IEntity"/> and keeps the corresponding <see cref="Texture2D"/> for drawing. 
    /// </summary>
    abstract public class SpriteBase
    {
        public Texture2D Texture { get; protected set; }

        /// <summary>
        /// Gets or sets the texture source in pixels.
        /// </summary>
        public Rectangle SourceRectangle { get; protected set; }

        public bool FlipHorizontal { get; protected set; }

        public float Orientation { get; protected set; }


        protected SpriteEffects effects => FlipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        

        public void Draw(SpriteBatch sb, RectangleF drawRect, float depth, Color customTint)
        {
            if(Texture != null)
                sb.ShanoDraw(Texture, SourceRectangle, drawRect, customTint, depth, effects, Orientation);
        }

        public void Draw(SpriteBatch sb, RectangleF drawRect, Color tint)
            => Draw(sb, drawRect, 0, tint);

        public void Draw(SpriteBatch sb, RectangleF drawRect, float depth)
            => Draw(sb, drawRect, depth, Color.White);

        public void Draw(SpriteBatch sb, RectangleF drawRect)
            => Draw(sb, drawRect, 0, Color.White);


        public virtual void Update(int msElapsed) { }
    }
}
