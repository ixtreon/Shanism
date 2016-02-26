using Engine.Objects;
using Engine.Objects.Entities;
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
        /// <summary>
        /// Gets the area this map is defined in.         
        /// </summary>
        Rectangle Bounds { get; }
         
        void GetMap(Rectangle rect, ref TerrainType[] outMap);

        IEnumerable<Entity> GetNativeEntities(Rectangle rect);

        /// <summary>
        /// Gets the terrain type at the given location. 
        /// </summary>
        /// <param name="loc">The in-game location to retrieve the terrain at. </param>
        TerrainType GetTerrain(Vector loc);
    }
}
