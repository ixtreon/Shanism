using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Shanism.Client
{
    public class Font
    {
        PixelFont font;

        internal SixLabors.Fonts.FontFamily Family { get; }

        public string Name => Family.Name;

        /// <summary>
        /// Gets the height of the font in UI units. 
        /// </summary>
        public float Height { get; }

        internal float DrawScale => Height / font.Glyphs.LineSpacing;


        internal GlyphMap Glyphs => font.Glyphs;
        public Texture2D Texture => font.Texture;
        StringSplitter splitter => font.Splitter;


        internal Font(PixelFont font, SixLabors.Fonts.FontFamily family, float uiHeight)
        {
            Family = family;
            Height = uiHeight;

            this.font = font;
        }

        internal void SetPixelFont(PixelFont font)
        {
            this.font = font;
        }

        public Vector2 MeasureString(string text, float? maxWidth = null)
        {
            splitter.GetCharacterOffsets(DrawScale, text, maxWidth);

            var size = new Vector2(0, splitter.Lines.Count * Height);
            if (splitter.Lines.Count != 0)
                size.X = splitter.Lines.Max(l => l.Width);

            return size;
        }

        public float GetWidth(char c)
        {
            return font.Glyphs.Get(c).TotalWidth * DrawScale;
        }

        public string SplitLines(string text, float maxWidth)
        {
            splitter.GetCharacterOffsets(DrawScale, text, maxWidth);
            var lines = splitter.Lines;
            var chars = splitter.Characters;

            var sb = new StringBuilder(lines.Count + chars.Count);
            for (int lineIndex = 0, charIndex = 0; charIndex < chars.Count; charIndex++)
            {
                while (lineIndex < lines.Count && lines[lineIndex].End == charIndex)
                {
                    sb.Append('\n');
                    lineIndex++;
                }
                sb.Append(chars[charIndex].Character);
            }

            return sb.ToString();
        }

        internal (IReadOnlyList<CharInfo> Characters, IReadOnlyList<LineInfo> Lines) GetOffsets(string text, float? maxWidth)
        {
            splitter.GetCharacterOffsets(DrawScale, text, maxWidth);
            return (splitter.Characters, splitter.Lines);
        }


        public override string ToString() => $"{Height:0.00}u {Name}";
    }
}
