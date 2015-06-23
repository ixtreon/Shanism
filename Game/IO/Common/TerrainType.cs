using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Common
{
    
    public enum TerrainType : byte
    {
        Dirt = 0,

        Grass = 1,
        Stone = 2,
        Water = 3,
        DeepWater = 4,
        Snow = 5,
        Sand = 6,

        None = 63,
    }
    public static class TerrainTypeExt
    {
        public static bool IsWater(this TerrainType t)
        {
            return t == TerrainType.Water || t == TerrainType.DeepWater;
        }
    }


}
