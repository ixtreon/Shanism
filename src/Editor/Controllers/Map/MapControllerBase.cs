using Ix.Math;
using Shanism.ScenarioLib;

namespace Shanism.Editor.Controllers
{
    class MapControllerBase
    {

        protected ITerrainMap Terrain { get; }

        public Rectangle? MapBounds => Terrain.Bounds;
        public bool ProceduralMap => Terrain.Bounds == null;

        public MapControllerBase(ITerrainMap terrain)
        {
            Terrain = terrain;
        }

    }
}
