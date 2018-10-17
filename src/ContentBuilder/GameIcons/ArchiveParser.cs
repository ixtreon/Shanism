using Ix.Math;
using Shanism.Common.Content;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace IconPacker.GameIcons
{
    class ArchiveParser
    {
        const string IconPrefix = "icons";

        static readonly GraphicsOptions drawOptions = new GraphicsOptions(true)
        {
            AntialiasSubpixelDepth = 16
        };

        readonly string textureName;
        readonly Point resizeIconsTo;

        public ArchiveParser(string textureName, Point resizeIconsTo)
        {
            this.textureName = textureName;
            this.resizeIconsTo = resizeIconsTo;
        }

        public (Image<Rgba32>, ContentBlob) ParseArchive(Stream iconArchive)
        {
            var extractedIcons = ReadArchive(iconArchive);
            var spriteSheet = CreateSpriteSheet(extractedIcons);

            var texture = new TextureDef(textureName, extractedIcons.LogicalSize);
            var animations = CreateIconDefinitions(extractedIcons);
            var blob = new ContentBlob(texture, animations);

            return (spriteSheet, blob);
        }

        IconPackage ReadArchive(Stream iconArchive)
        {
            using (var archive = new ZipArchive(iconArchive, ZipArchiveMode.Read))
            {
                var iconList = archive.Entries
                    .Where(e => e.Name.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                    .Select(e => ReadIcon(e))
                    .ToList();

                return new IconPackage(iconList);
            }
        }

        GameIcon ReadIcon(ZipArchiveEntry e)
        {
            using (var entryStream = e.Open())
            {
                var img = Image.Load(entryStream);
                img.Mutate(x => x.Resize(AsSize(resizeIconsTo)));

                return new GameIcon(e.FullName, img);
            }
        }

        static SixLabors.Primitives.Size AsSize(Point p) => new SixLabors.Primitives.Size(p.X, p.Y);

        List<AnimationDef> CreateIconDefinitions(IconPackage icons)
        {
            var defs = new List<AnimationDef>();

            foreach (var (data, pos) in EnumerateIcons(icons))
            {
                var iconName = Path.GetFileNameWithoutExtension(data.Name);
                var animName = Path.Combine(IconPrefix, iconName);
                var animDef = new AnimationDef(animName, textureName, new Rectangle(pos, Point.One));

                defs.Add(animDef);
            }

            return defs;
        }

        Image<Rgba32> CreateSpriteSheet(IconPackage icons)
        {
            // stitch the sheet
            var sheetPixelSize = resizeIconsTo * icons.LogicalSize;
            var image = new Image<Rgba32>(sheetPixelSize.X, sheetPixelSize.Y);

            image.Mutate(c =>
            {
                foreach (var (data, pos) in EnumerateIcons(icons))
                {
                    var drawPos = pos * resizeIconsTo;
                    c.DrawImage(drawOptions, data.Image,
                        new SixLabors.Primitives.Point(drawPos.X, drawPos.Y)
                    );
                }
            });

            return image;
        }

        static IEnumerable<(GameIcon, Point Position)> EnumerateIcons(IconPackage icons)
        {
            int x = 0, y = 0;
            foreach (var data in icons.Icons)
            {
                yield return (data, new Point(x, y));

                if (++x == icons.LogicalSize.X)
                    (x, y) = (0, y + 1);
            }
        }
    }
}
