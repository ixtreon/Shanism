using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects.Game;
using IO.Common;

namespace Engine.Maps
{
    class ScenarioMap : ITerrainMap
    {
        private readonly Rectangle bounds;

        public int Bottom { get { return bounds.Bottom; } }

        public int Left { get { return bounds.Left; } }
        public int Right { get { return bounds.Right; } }

        public int Top { get { return bounds.Top; } }

        public void GetMap(Rectangle rect, ref TerrainType[,] outMap)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Doodad> GetNativeDoodads(Rectangle rect)
        {
            throw new NotImplementedException();
        }
    }
}
