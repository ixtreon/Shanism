using Ix.Math;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.Text
{
    /// <summary>
    /// Contains information about the sizing of characters in a font.
    /// </summary>
    class GlyphMap
    {

        readonly Glyph[] glyphs;
        readonly Dictionary<char, int> lookup;

        Glyph defaultGlyph;


        /// <summary>
        /// Gets the spacing between consecutive characters, in pixels.
        /// </summary>
        public float CharSpacing { get; }

        /// <summary>
        /// Gets the spacing between text lines, in pixels.
        /// </summary>
        public float LineSpacing { get; }

        public GlyphMap(IEnumerable<Glyph> glyphs, float charSpacing, float lineSpacing, char defaultCharacter = '?')
        {
            this.glyphs = glyphs.ToArray();
            lookup = this.glyphs
                .Select((g, i) => (Char: g.Character, Index: i))
                .ToDictionary(x => x.Char, x => x.Index);

            CharSpacing = charSpacing;
            LineSpacing = lineSpacing;
            defaultGlyph = this.glyphs[lookup[defaultCharacter]];
        }

        public ref Glyph Get(char c)
        {
            if (lookup.TryGetValue(c, out var id))
                return ref glyphs[id];
            return ref defaultGlyph;
        }
    }

    struct Glyph
    {

        /// <summary>
        /// The character being drawed.
        /// </summary>
        public char Character;

        /// <summary>
        /// The position of this character in the font bitmap.
        /// </summary>
        public RectangleF SourceRectangle;

        /// <summary>
        /// The left/top offset when drawing. 
        /// </summary>
        public RectangleF DestinationRectangle;

        /// <summary>
        /// The width including left/right offsets.
        /// </summary>
        public float TotalWidth;

        public float RawAscent;
        public float RawDescent;
    }
}
