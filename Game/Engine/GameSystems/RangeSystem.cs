using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Maps;
using Shanism.Engine.Objects.Range;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shanism.Engine.GameSystems
{
    /// <summary>
    /// Raises the events of type <see cref="RangeEvent"/> defined for all units on the provided game map. 
    /// </summary>
    class RangeSystem : GameSystem
    {
        //limit the maximum updates per second
        //as this tends to be the heaviest system
        //10 updates per second should be perfectly fine.
        const int MaxFPS = 10;

        public override string SystemName => "Range Events";


        readonly MapSystem map;

        readonly Counter updateCounter = new Counter(1000 / MaxFPS);


        public RangeSystem(MapSystem map)
        {
            this.map = map;
        }

        internal override void Update(int msElapsed)
        {
            if (updateCounter.Tick(msElapsed))
            {
                //query tree for range events
                foreach (var u in map.Units)
                    u.range.FireEvents();
            }
        }
    }
}
