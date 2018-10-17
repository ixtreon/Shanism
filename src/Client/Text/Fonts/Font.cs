using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.IO;
using Shanism.Client.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Shanism.Client
{

    /// <summary>
    /// A font in the Shanism engine.
    /// Basically a font family plus the desired text size.
    /// </summary>
    [Obsolete("not needed once we can scale fonts in-memory")]
    public class Font_OLD
    {

        readonly ScreenSystem screen;

        readonly TextRenderInfo splitResult = new TextRenderInfo(new List<CharInfo>(), new List<LineInfo>());

        /// <summary>
        /// Gets the font family of this font instance.
        /// </summary>
        FontFamily Family { get; }

        /// <summary>
        /// Gets the height of the font in UI units. 
        /// </summary>
        public float Height { get; }

        internal float DrawScale { get; private set; }

        float uiScale;
        PixelFont font;

        public Font_OLD(ScreenSystem screen, FontFamily fontFamily, float uiSize)
        {
            this.screen = screen;
            Family = fontFamily;
            Height = uiSize;
        }

        public Vector2 MeasureString(string text, float? maxWidth = null)
        {
            RefreshScale();

            font.Splitter.GetCharacterOffsets(DrawScale, text, maxWidth);

            var w = font.Splitter.Lines.Any() ? font.Splitter.Lines.Max(l => l.Width) : 0;
            var h = font.Splitter.Lines.Count * Height;
            return new Vector2(w, h);
        }

        public float GetWidth(char c)
        {
            RefreshScale();

            return Glyphs.Get(c).TotalWidth * DrawScale;
        }

        public string SplitLines(string text, float maxWidth)
        {
            RefreshScale();

            font.Splitter.GetCharacterOffsets(DrawScale, text, maxWidth);
            var lines = font.Splitter.Lines;
            var chars = font.Splitter.Characters;

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
            RefreshScale();

            font.Splitter.GetCharacterOffsets(DrawScale, text, maxWidth);

            return (font.Splitter.Characters, font.Splitter.Lines);
        }


        internal GlyphMap Glyphs => font.Glyphs;

        public Texture2D Texture => font.Texture;


        [Obsolete("not needed once we can scale fonts in-memory")]
        void RefreshScale()
        {
            if (uiScale.Equals(screen.UI.Scale))
                return;

            uiScale = screen.UI.Scale;
            font = Family.GetFont(Height * uiScale);
            DrawScale = Height / Glyphs.LineSpacing;
        }

        public override string ToString()
            => $"Font {Family.Name} @ {Height:0.00}u";

    }

}
