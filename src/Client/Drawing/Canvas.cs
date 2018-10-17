using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Sprites;
using Shanism.Common;
using System;
using System.Linq;
using System.Numerics;
using XnaVector = Microsoft.Xna.Framework.Vector2;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

namespace Shanism.Client
{

    // assumes no boxing occurs and try/finally is free
    // see https://stackoverflow.com/q/1330571
    public struct Canvas : IDisposable
    {
        readonly CanvasStarter starter;

        /// <summary>
        /// The offset used when drawing things.
        /// </summary>
        readonly Vector2 offset;

        /// <summary>
        /// The scissors rect, expressed in absolute co-ordinates.
        /// </summary>
        readonly RectangleF scissors;

        ShanoTexture BlankTexture => starter.BlankTexture;

        public Canvas(CanvasStarter starter, Vector2 drawOffset, RectangleF window)
        {
            this.starter = starter;
            this.offset = drawOffset;
            this.scissors = window;
        }

        public Canvas ZoomIn(RectangleF bounds)
        {
            // clamp size
            var maxSize = Vector2.Max(scissors.FarPosition - bounds.Position, Vector2.Zero);

            // offset position
            var newOffset = offset + bounds.Position;
            var newWindowSize = bounds.Size.Clamp(Vector2.Zero, maxSize);
            var newWindow = new RectangleF(Vector2.Zero, newWindowSize);
            
            return starter.Push(newOffset, newWindow);
        }

        public void Dispose()
        {
            starter.Pop();
        }

        public void Clear(Color c)
            => starter.SpriteBatch.GraphicsDevice.Clear(c.ToXnaColor());

        void Draw
        (
            Texture2D tex,
            RectangleF source,
            RectangleF dest,
            Color color,
            float depth = 0,
            SpriteEffects effects = SpriteEffects.None,
            float rotation = 0
        )
        {
            if (!scissors.Intersects(dest))
                return;

            if (!scissors.FullyContains(dest))
                contain(ref source, ref dest);

            starter.SpriteBatch.Draw(tex,
                position: (offset + dest.Position).ToXnaVector(),
                sourceRectangle: ToRect(source),
                color: color.ToXnaColor(),
                rotation: rotation,
                origin: XnaVector.Zero,
                scale: (dest.Size / source.Size).ToXnaVector(),
                effects: effects,
                layerDepth: depth);


            XnaRectangle ToRect(RectangleF rect)
            {
                var pos = rect.Position.Round();
                var sz = rect.FarPosition.Round() - pos;

                return new XnaRectangle(pos.X, pos.Y, sz.X, sz.Y);
            }
        }


        public void DrawString
        (
            Font font,
            string text,
            Vector2 position,
            Color color,
            float? maxWidth = null,
            AnchorPoint anchor = AnchorPoint.TopLeft
        )
        {
            if (string.IsNullOrEmpty(text))
                return;

            // grab text drawing details
            var (chars, lines) = font.GetOffsets(text, maxWidth);

            // determine bounding box
            var w = lines.Max(l => l.Width);
            var h = lines.Count * font.Height;
            var size = new Vector2(w, h);
            var anchorOffset = anchor.GetOffset();

            // adjust origin based on anchor
            if (anchor != AnchorPoint.TopLeft)
                position -= size * anchorOffset;

            // early exit if text won't be visible
            if (!scissors.Intersects(position, size))
                return;

            var tex = font.Texture;
            var glyphs = font.Glyphs;
            var offset = position;
            for (int i = 0; i < lines.Count; i++)
            {
                offset.X = position.X + (w - lines[i].Width) * anchorOffset.X;

                var end = lines[i].End;
                for (var j = lines[i].Start; j < end; j++)
                {
                    ref var glyph = ref glyphs.Get(chars[j].Character);

                    //var drawBounds = new RectangleF(glyph.DrawOffset.Position, glyph.SourceRectangle.Size) * font.DrawScale;
                    var drawBounds = glyph.DestinationRectangle * font.DrawScale;
                    var destBounds = drawBounds + offset;

                    Draw(tex, glyph.SourceRectangle, destBounds, color);

                    //var ascBounds = new RectangleF(offset.X, offset.Y, 
                    //    destBounds.Width,
                    //    font.DrawScale * font.Height * glyph.RawAscent
                    //);

                    //FillRectangle(ascBounds, rnd.NextRgbColor());


                    offset.X += chars[j].Width + glyphs.CharSpacing;
                }

                offset.Y += font.Height;
            }
        }

        static readonly Random rnd = new Random();

        void contain(ref RectangleF source, ref RectangleF dest)
        {
            // get the intersection
            var near = dest.Position.Clamp(scissors);
            var far = dest.FarPosition.Clamp(scissors);
            var outDest = new RectangleF(near, far - near);

            // calculate source rect
            var (srcPos, srcSize) = (outDest - dest.Position) * source.Size / dest.Size;
            var outSource = new RectangleF(
                source.Position + srcPos,
                Vector2.Max(srcSize, Vector2.One)
            );

            dest = outDest;
            source = outSource;
        }


        public void Draw(ShanoTexture tex, RectangleF destination, Color colorTint)
            => Draw(tex.Texture, tex.Bounds, destination, colorTint);

        /// <summary>
        /// Draws a rectangle as a combination of 4 lines (quads).
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="lineWidth"></param>
        /// <param name="color"></param>
        /// <param name="lineTexture"></param>
        public void DrawRectangle(RectangleF bounds, float lineWidth, Color color, ShanoTexture lineTexture = null)
        {
            if (lineWidth <= 0)
                return;     // TODO: throw a console warning?

            var tex = lineTexture ?? BlankTexture;

            var near = bounds.Position;
            var far = bounds.FarPosition;
            var h = new Vector2(bounds.Size.X, lineWidth);
            var v = new Vector2(lineWidth, bounds.Size.Y);

            Draw(tex, new RectangleF(near, h), color);
            Draw(tex, new RectangleF(near, v), color);
            Draw(tex, new RectangleF(far - h, h), color);
            Draw(tex, new RectangleF(far - v, v), color);
        }

        public void FillRectangle(RectangleF bounds, Color color)
            => Draw(BlankTexture, bounds, color);

        public void FillRectangle(Vector2 position, Vector2 size, Color color)
            => FillRectangle(RectangleF.Normalise(position, size), color);

        public void DrawSprite(Sprite s, RectangleF destination, Color colorTint, float depth)
        {
            if (s.Texture == null)
                return;

            Draw(s.Texture, s.SourceRectangle, destination, colorTint, depth, s.SpriteEffects, s.Orientation);
        }

        public void DrawString(Font font, string text, Color color,
            Vector2 destPos, float maxWidth,
            AnchorPoint anchor = AnchorPoint.TopLeft)
        {
            DrawString(font, text, destPos, color, maxWidth, anchor);
        }

        public void DrawString(Font font, string text, Color color,
            RectangleF destRect,
            AnchorPoint anchor = AnchorPoint.TopLeft)
        {
            var textPos = destRect.Position + destRect.Size * anchor.GetOffset();
            DrawString(font, text, color, textPos, destRect.Width, anchor);
        }


        public void DrawEntitySprite(EntitySprite s, Vector2 pos, Vector2 size)
            => DrawSprite(s, new RectangleF(pos, size), s.Tint, s.DrawDepth);

        public void DrawSprite(Sprite s, Vector2 pos, Vector2 size, Color tint)
            => DrawSprite(s, new RectangleF(pos, size), tint, 0);

        public void DrawSprite(Sprite s, RectangleF destination, Color tint)
            => DrawSprite(s, destination, tint, 0);


    }
}
