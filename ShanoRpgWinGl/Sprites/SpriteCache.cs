using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MapTile = IO.Common.MapTile;

namespace ShanoRpgWinGl.Sprites
{
    /// <summary>
    /// Contains information about the sprites stuff shit stack.
    /// </summary>
    class SpriteCache
    {

        public static Sprite BlankTexture { get; private set; }

        public static class Terrain
        {
            public static Sprite Grass { get; set; }
            public static Sprite Dirt { get; set; }

            public static Sprite GetSprite(MapTile t)
            {
                switch (t)
                {
                    case MapTile.Dirt:
                        return Dirt;
                    case MapTile.Grass:
                        return Grass;
                    default:
                        throw new Exception("Unrecognized MapTile. ");
                }
            }

            internal static void Load()
            {
                Grass = New(TextureType.Terrain, "grass");
                Dirt = New(TextureType.Terrain, "dirt");
            }
        }

        public static class Icon
        {
            public static Sprite Border { get; private set; }
            public static Sprite BorderHover { get; private set; }

            public static Sprite Nothing { get; private set; }


            public static void Load()
            {
                Border = New(TextureType.Icon, "border");
                BorderHover = New(TextureType.Icon, "border_hover");
                Nothing = New(TextureType.Icon, "none");
            }
        }

        private static readonly Point DefaultSize = new Point(1, 1);
        private static Dictionary<Texture2D, Point> defaultSizes = new Dictionary<Texture2D, Point>();

        public static void Load()
        {
            Icon.Load();
            Terrain.Load();
            BlankTexture = new Sprite(TextureCache.Get("1"));
        }

        public static Sprite NewModel(IO.Common.Model m)
        {
            var tex = TextureCache.Get(TextureType.Model, m.Name);
            return new Sprite(tex, m.Size.X, m.Size.Y, m.Period);
        }

        public static Sprite New(TextureType t, string texName)
        {
            var tex = TextureCache.Get(t, texName);

            Point sz = DefaultSize;

            if (defaultSizes.ContainsKey(tex))
                sz = defaultSizes[tex];

            return new Sprite(tex, sz.X, sz.Y);
        }
    }
}
