using Shanism.Engine.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.GameSystems
{
    /// <summary>
    /// Calls <see cref="Entity.Update(int)"/> for all entities on the map.
    /// </summary>
    /// <seealso cref="Shanism.Engine.GameSystem" />
    class EntitiesSystem : GameSystem
    {
        public override string SystemName { get; } = "Entities";

        readonly MapSystem map;

        public EntitiesSystem(MapSystem map)
        {
            this.map = map;
        }

        internal override void Update(int msElapsed)
        {
            //update the entities
            foreach (var e in map.Entities.ToList())
                e.Update(msElapsed);
        }
    }
}
