using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Maps.Terrain
{
    static class MapGod
    {
        public static ITerrainMap Create(MapConfig mapConfig, int seed)
        {
            if (mapConfig.IsInfinite)
                return new RandomMap(seed);
            else
                return new FixedMap(mapConfig.Terrain);
        }
    }
}
