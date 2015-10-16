using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Sprites;
using IO.Content;
using Client.Textures;
using IO.Common;

namespace Client.Sprites
{
    /// <summary>
    /// A static class which manages the creation and handling of game sprites. 
    /// </summary>
    static class SpriteFactory
    {
        /// <summary>
        /// Gets the blank texture, as a sprite. 
        /// </summary>
        public static Sprite Blank { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static class Terrain
        {
            public static readonly TextureDef TerrainDef = new TextureDef("terrain", 8, 8);

            public static TTexture TerrainAtlas { get; private set; }
            public static TTexture TerrainAtlas2 { get; private set; }

            private static Sprite[] terrainSprites = new Sprite[255];

            public static Sprite GetSprite(TerrainType t)
            {
                return terrainSprites[(int)t];
            }

            internal static void Load()
            {
                TerrainAtlas = new TTexture(TerrainDef);
                foreach(TerrainType t in Enum.GetValues(typeof(TerrainType)))
                {
                    terrainSprites[(int)t] = new StaticSprite(new AnimationDefOld(TerrainDef, TerrainAtlas.GetTile((int)t)));
                }
                TerrainAtlas2 = new TTexture(new TextureDef("terrain_atlas", 32, 32));
            }
        }

        public static class Icon
        {
            public static Sprite Border { get; private set; }
            public static Sprite BorderHover { get; private set; }

            public static Sprite Nothing { get; private set; }


            public static void Load()
            {
                Border = FromTexture(TextureType.Icon, "border");
                BorderHover = FromTexture(TextureType.Icon, "border_hover");
                Nothing = FromTexture(TextureType.Icon, "none");
            }
        }


        public static void Load()
        {
            Icon.Load();
            Terrain.Load();
            Blank = new StaticSprite(new AnimationDefOld(new TextureDef("1")));
        }

        public static Sprite FromModel(AnimationDefOld m)
        {
            if (m.IsAnimated)
                return new AnimatedSprite(m);
            else
                return new StaticSprite(m);
        }

        internal static Sprite FromTexture(TextureType t, string texName)
        {
            return new StaticSprite(new AnimationDefOld(new TextureDef(t.GetDirectory(texName))));
        }
    }
}
