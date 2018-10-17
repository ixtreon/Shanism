using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common
{
    /// <summary>
    /// An enumeration of the types of terrain.
    /// </summary>
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

    /// <summary>
    /// Provides extensions for the <see cref="TerrainType"/> enum.
    /// </summary>
    public static class TerrainTypeExt
    {
        /// <summary>
        /// Determines whether this terrain is OK for swimming.
        /// </summary>
        public static bool IsWater(this TerrainType t)
            => t == TerrainType.Water
            || t == TerrainType.DeepWater;

        /// <summary>
        /// Determines whether this terrain is OK for swimming.
        /// </summary>
        public static bool IsGround(this TerrainType t)
            => (t != TerrainType.None) 
            && !IsWater(t);
    }


}
