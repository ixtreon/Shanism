using Ix.Math;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using static System.Math;

namespace IconPacker
{

    readonly struct IconPackage
    {
        public readonly IReadOnlyList<GameIcon> Icons;
        public readonly Point LogicalSize;

        public IconPackage(IReadOnlyList<GameIcon> icons)
        {
            Icons = icons;
            LogicalSize = GetSheetSize(icons.Count);
        }

        static Point GetSheetSize(int nIcons)
        {
            var nIconsX = (int)Ceiling(Sqrt(nIcons));
            var nIconsY = (int)Ceiling((double)nIcons / nIconsX);

            return new Point(nIconsX, nIconsY);
        }
    }

    readonly struct GameIcon
    {
        public readonly string Name;
        public readonly Image<Rgba32> Image;

        public GameIcon(string name, Image<Rgba32> image)
        {
            Image = image;
            Name = name;
        }

        public void Deconstruct(out string name, out Image<Rgba32> image)
        {
            image = Image;
            name = Name;
        }
    }

    public static class ImageExtensions
    {
        public static void SaveAsTransparentscale(this Image<Rgba32> image, string path, int bitsPerPixel)
        {
            image.Save(path, new PngEncoder
            {
                ColorType = PngColorType.Palette,
                Quantizer = new CustomPngQuantizer(bitsPerPixel),
            });
        }
    }
}
