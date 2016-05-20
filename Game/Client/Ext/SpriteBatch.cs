using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Objects;
using Shanism.Client.Textures;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Common.Game;

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
            Rectangle sourceRect, Vector destPos, Vector destSz, Color c, 
            float depth = 0,
            float rotation = 0)
        {
            var srcOrigin = ((Vector)sourceRect.Size / 2).ToVector2();
            var destOrigin = (destSz / 2).ToVector2();
            sb.Draw(tex,
                destPos.ToVector2() + destOrigin,
                sourceRectangle: sourceRect.ToXnaRect(),
                color: c,
                scale: (destSz / sourceRect.Size).ToVector2(),
                rotation: rotation,
                origin: srcOrigin);
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
                color: c);
        }
    }
}
