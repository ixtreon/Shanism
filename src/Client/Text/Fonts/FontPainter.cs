using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Numerics;

using static System.Math;

namespace Shanism.Client.Text
{
    public class FontPainter
    {

        // the printable ASCII characters
        static readonly char[] CharMap = Enumerable.Range(32, 126 - 32)
            .Select(i => (char)i)
            .ToArray();

        const float CharSpacing = 0;
        const char DefaultChar = '?';

        readonly GraphicsDevice device;


        public FontPainter(GraphicsDevice device)
        {
            this.device = device;
        }


        /// <summary>
        /// Grabs some font using ImageSharp. 
        /// Draws all chars we need to an in-memory bitmap. 
        /// Creates a GPU texture and a gyph map.
        /// Finally returns the newly created font. 
        /// </summary>
        internal PixelFont CreateFont(SixLabors.Fonts.Font font, float lineSpacing, FontStyle style = FontStyle.Regular)
        {
            // obtain glyph sizes
            var fontRenderOptions = new RendererOptions(font);
            var glyphBounds = CharMap.Distinct()
                .ToDictionary(c => c, 
                    c => ToIxRect(TextMeasurer.MeasureBounds(c.ToString(), fontRenderOptions))
                );

            // prepare & measure the text to draw
            var textToDraw = new string(CharMap.SelectMany(c => new[] { c, ' ' }).ToArray());
            if (!TextMeasurer.TryMeasureCharacterBounds(textToDraw, fontRenderOptions, out var charBounds))
                throw new Exception();

            var imageSize = Point.Zero;
            foreach (var sz in charBounds)
                imageSize = Point.Max(imageSize, ToIxRect(sz.Bounds).BottomRight.Ceiling());

            // draw the image in-memory
            var image = new Image<Rgba32>(imageSize.X, imageSize.Y);
            image.Mutate(t =>
                t.DrawText(textToDraw, font, Rgba32.White, SixLabors.Primitives.PointF.Empty)
            );


            // transfer to the GPU
            var imageBuffer = image.GetPixelSpan().ToArray();
            var gpuTexture = new Texture2D(device, image.Width, image.Height, false, SurfaceFormat.Color);
            gpuTexture.SetData(imageBuffer);

            // calculate the glyph metrics
            var glyphsForReal = charBounds
                .Where((_, i) => i % 2 == 0)
                .Select((x, i) =>
                {
                    if (x.Character.Length != 1)
                        throw new Exception("No UTF16 support yet...");

                    var c = x.Character[0];
                    var rawBounds = glyphBounds[c];
                    var paintedBounds = ToIxRect(x.Bounds);

                    var foo = font.LineHeight;

                    return new Glyph
                    {
                        Character = c,
                        SourceRectangle = paintedBounds,
                        DestinationRectangle = rawBounds,
                        TotalWidth = paintedBounds.Width + rawBounds.X,

                        RawAscent = (float)font.Ascender / font.LineHeight,
                        RawDescent = (float)font.Descender / font.LineHeight,
                    };
                });


            var glyphMap = new GlyphMap(glyphsForReal, CharSpacing, lineSpacing, DefaultChar);

            return new PixelFont(gpuTexture, glyphMap);
        }


        Ix.Math.RectangleF ToIxRect(SixLabors.Primitives.RectangleF source) => new Ix.Math.RectangleF(
            (source.X),
            (source.Y),
            (source.Width),
            (source.Height)
        );
    }
}
