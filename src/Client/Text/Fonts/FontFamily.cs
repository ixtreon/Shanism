using Shanism.Common;
using Shanism.Common.Util;
using System.Collections.Generic;

namespace Shanism.Client.Text
{
    public class FontFamily
    {

        readonly IComparer<PixelFont> newFontComparer = new GenericComparer<PixelFont>(
            (a, b) => a.LineSpacing.CompareTo(b.LineSpacing)
        );

        readonly List<PixelFont> fonts = new List<PixelFont>();

        /// <summary>
        /// Gets the name of this font family.
        /// </summary>
        public string Name { get; }

        public FontFamily(string name)
        {
            Name = name;
        }

        internal void Add(PixelFont f)
        {
            fonts.InsertSorted(f, newFontComparer);
        }


        /// <summary>
        /// Gets a <see cref="PixelFont"/> object of a suitable bitmap font
        /// for the given destination height in pixels.
        /// </summary>
        internal PixelFont GetFont(float targetHeightPx)
        {
            if (targetHeightPx < fonts[0].LineSpacing)
                return fonts[0];

            for (int i = 1; i < fonts.Count - 1; i++)
                if (fonts[i + 1].LineSpacing > targetHeightPx)
                    return fonts[i];

            return fonts[fonts.Count - 1];
        }

        public override string ToString()
            => $"Font family '{Name}' ({fonts.Count} fonts)";

    }
}
