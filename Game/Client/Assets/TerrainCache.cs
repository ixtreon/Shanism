using Client.Common;
using Client.Textures;
using IO;
using IO.Common;
using IO.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IO.Constants.Content;

namespace Client.Assets
{
    class TerrainCache
    {
        public Texture2D Texture { get; private set; }


        public void Reload()
        {
            var terrainTex = Content.Textures.TryGetRaw(TerrainFile);
            Texture = terrainTex;
        }

        public PointsInfo GetTile(TerrainType ty)
        {
            var id = (int)ty;
            var x = id % TerrainFileSplitsX;
            var y = id / TerrainFileSplitsX;
            return new PointsInfo(x, y, TerrainFileSplitsX, TerrainFileSplitsY);
        }
    }
}
