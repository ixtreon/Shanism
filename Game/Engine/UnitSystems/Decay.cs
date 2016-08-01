using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    class DecaySystem : UnitSystem
    {
        public const int DefaultDecayPeriod = 15000;

        public int DecayPeriod { get; set; } = DefaultDecayPeriod;

        public DecaySystem(Unit owner)
        {
            owner.Death += Owner_Death;
        }
      
        async void Owner_Death(Events.UnitDyingArgs ev)
        {
            var owner = ev.DyingUnit;

            await Task.Delay(DecayPeriod);

            owner.Destroy();
        }
    }
}
