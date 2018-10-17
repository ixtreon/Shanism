using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Text;
using Shanism.Common;
using System.Linq;
using System.Numerics;

namespace Shanism.Client
{

    public static class XnaExtensions
    {
        /// <summary>
        /// Draws (a part of) the given texture at the specified screen position.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="tex">The texture to be drawn.</param>
        /// <param name="sourceRect">The source rect.</param>
        /// <param name="destRect">The destination position on screen.</param>
        /// <param name="c">The color to tint the texture in.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="effects">The rotational effects to apply.</param>
        /// <param name="rotation">The angle of rotation in radians.</param>
        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex,
            Rectangle sourceRect, RectangleF destRect, Color c,
            float depth = 0,
            SpriteEffects effects = SpriteEffects.None,
            float rotation = 0)
        {
            var srcOrigin = (Vector2)sourceRect.Size / 2;    //anchor (rotate & draw) around the center
            var drawPos = destRect.Center.ToXnaVector();      //draw at the center, too
            var drawScale = destRect.Size / sourceRect.Size;    //scale instead of drawrect

            sb.Draw(tex,
                position: drawPos,
                sourceRectangle: sourceRect.ToXnaRect(),
                color: c.ToXnaColor(),
                rotation: rotation,
                origin: srcOrigin.ToXnaVector(),
                scale: drawScale.ToXnaVector(),
                effects: effects,
                layerDepth: depth);
        }

        public static Point GetSize(this Texture2D renderTarget)
            => new Point(renderTarget.Width, renderTarget.Height);


    }
}
