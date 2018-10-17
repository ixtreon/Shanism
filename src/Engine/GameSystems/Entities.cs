using Shanism.Engine.Models.Systems;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Calls <see cref="Entity.Update(int)"/> for all entities on the map.
    /// </summary>
    /// <seealso cref="GameSystem" />
    class EntitiesSystem : GameSystem
    {
        public override string Name { get; } = "Entities";

        readonly MapSystem map;

        public EntitiesSystem(MapSystem map)
        {
            this.map = map;
        }

        internal override void Update(int msElapsed)
        {

        }
    }
}
