using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    class CollisionSystem : UnitSystem
    {
        Unit Owner { get; }

        public CollisionSystem(Unit owner)
        {
            Owner = owner;
        }

        internal override void Update(int msElapsed)
        {
            throw new NotImplementedException();
        }
    }
}
