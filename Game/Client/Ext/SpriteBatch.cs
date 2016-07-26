using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Util;

namespace Shanism.Client
{

    public static class SpriteBatchExt
    {
        /// <summary>
        /// Draws (a part of) the given texture at the specified screen position. 
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="tex">The texture to be drawn.</param>
        /// <param name="sourceRect">The source rect.</param>
        /// <param name="destPos">The destination position on screen.</param>
        /// <param name="destSz">The destination size on screen.</param>
        /// <param name="c">The tint color.</param>
        /// <param name="depth">The depth. NYI.</param>
        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex,
            Rectangle sourceRect, RectangleF destRect, Color c,
            float depth = 0,
            SpriteEffects effects = SpriteEffects.None,
            float rotation = 0)
        {
            var srcOrigin = (Vector)sourceRect.Size / 2;
            var drawPos = destRect.Center.ToVector2();
            var drawScale = destRect.Size / sourceRect.Size;

            sb.Draw(tex, 
                drawPos,
                sourceRectangle: sourceRect.ToXnaRect(),
                color: c.ToXnaColor(),
                rotation: rotation,
                origin: srcOrigin.ToVector2(),
                scale: drawScale.ToVector2(),
                effects: effects,
                layerDepth: depth);
        }

        /// <summary>
        /// Draws the given texture at the specified screen position. 
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="tex">The texture to be drawn.</param>
        /// <param name="dest">The destination rectangle on screen.</param>
        /// <param name="c">The tint color.</param>
        /// <param name="depth">The depth. NYI.</param>
        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex, RectangleF dest, Color c, float depth = 0)
        {
            sb.Draw(tex,
                sourceRectangle: tex.Bounds,
                position: dest.Position.ToVector2(),
                scale: (dest.Size / new Vector(tex.Width, tex.Height)).ToVector2(),
                color: c.ToXnaColor());
        }
    }
}
