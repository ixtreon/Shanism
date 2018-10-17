using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client.Text
{
    class PixelFont
    {
        public Texture2D Texture { get; }

        public GlyphMap Glyphs { get; }

        public StringSplitter Splitter { get; }

        public PixelFont(Texture2D texture, GlyphMap glyphs)
        {
            Texture = texture;
            Glyphs = glyphs;
            Splitter = new StringSplitter(Glyphs);
        }

        internal float LineSpacing => Glyphs.LineSpacing;
    }

}
