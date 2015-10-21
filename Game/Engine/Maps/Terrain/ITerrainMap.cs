using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    public interface ITerrainMap
    {
        int Left { get; }

        int Right { get; }

        int Top { get; }

        int Bottom { get; }

        void GetMap(Rectangle rect, ref TerrainType[,] outMap);

        IEnumerable<Doodad> GetNativeDoodads(Rectangle rect);

        TerrainType GetTerrainAt(Vector loc);
    }
}
