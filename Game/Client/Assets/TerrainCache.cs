using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Shanism.Common.Constants.Content;

namespace Shanism.Client.Drawing
{
    class TerrainCache
    {
        public Texture2D Texture { get; }


        public TerrainCache(TextureCache textures)
        {
            Texture2D outTex;
            if (!textures.TryGet(TerrainFile, out outTex))
                Console.WriteLine("Warning: unable to load the terrain file!");

            Texture = outTex;
        }

    }
}
