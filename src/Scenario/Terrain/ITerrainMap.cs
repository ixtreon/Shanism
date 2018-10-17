using Ix.Math;
using Shanism.Common;
using System.Numerics;

namespace Shanism.ScenarioLib
{
    public interface ITerrainMap
    {
        /// <summary>
        /// Gets the area this terrain is defined in.         
        /// </summary>
        Rectangle? Bounds { get; }

        /// <summary>
        /// Puts the terrain values from the specified in-game rect 
        /// in the given 1D array. 
        /// </summary>
        /// <param name="rect">The in-game rect to get terrain values for.</param>
        /// <param name="outMap">The array where terrain values are put.</param>
        void Get(Rectangle rect, ref TerrainType[] outMap);

        /// <summary>
        /// Gets the terrain type at the given location. 
        /// </summary>
        /// <param name="loc">The in-game location to retrieve the terrain at. </param>
        TerrainType Get(Vector2 loc);

        /// <summary>
        /// Sets the terrain type at the given location.
        /// </summary>
        /// <param name="loc">The location at which to change the terrain.</param>
        /// <param name="tty">The new terrain type at the specified location.</param>
        void Set(Point loc, TerrainType tty);
    }
}
