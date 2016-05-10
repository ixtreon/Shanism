using Shanism.Common;
using Shanism.Common.Game;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Shanism.Common.Constants.Content;

namespace Shanism.Client.Assets
{
    class TerrainCache
    {
        public Texture2D Texture { get; private set; }


        public void Reload()
        {
            var terrainTex = Content.Textures.TryGetRaw(TerrainFile);
            Texture = terrainTex;
        }

        public RectangleF GetTileTextureBounds(TerrainType tty)
        {
            const double delta = 1e-5;

            var ttyId = (int)tty;
            var x = ttyId % TerrainFileSplitsX;
            var y = ttyId / TerrainFileSplitsX;
            var logicalSize = new Vector(TerrainFileSplitsX, TerrainFileSplitsY);

            return new RectangleF(x, y, 1, 1).Inflate(-delta) / logicalSize;
        }
    }
}
