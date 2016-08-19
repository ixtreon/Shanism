﻿using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Util;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client.Drawing
{
    /// <summary>
    /// The visual representation of a <see cref="IEntity"/>. 
    /// Tracks the model and animation of a single <see cref="IEntity"/> and keeps the corresponding <see cref="Texture2D"/> for drawing. 
    /// </summary>
    abstract class SpriteBase
    {
        public Texture2D Texture { get; protected set; }

        /// <summary>
        /// Gets or sets the texture source in pixels.
        /// </summary>
        public Rectangle SourceRectangle { get; protected set; }

        public bool FlipHorizontal { get; protected set; }

        public float Orientation { get; protected set; }

        public Color Tint { get; protected set; }


        protected SpriteEffects effects => FlipHorizontal ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


        public void Draw(SpriteBatch sb, RectangleF drawRect, float depth)
        {
            if (Texture != null)
                sb.ShanoDraw(Texture, SourceRectangle, drawRect, Tint, depth, effects, Orientation);
        }

        public virtual void Update(int msElapsed) { }
    }
}
