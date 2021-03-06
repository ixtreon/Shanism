﻿using Shanism.Common;
using Shanism.Engine.Models.Systems;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Raises the events of type 
    /// <see cref="Objects.Range.RangeEvent"/> 
    /// for all units on the provided game map. 
    /// </summary>
    class RangeSystem : GameSystem
    {
        //limit the maximum updates per second
        //as this tends to be the heaviest system
        //10 updates per second should be perfectly fine.
        const int MaxFPS = 10;

        public override string Name => "Range Events";


        readonly MapSystem map;

        readonly Counter updateCounter = new Counter(1000 / MaxFPS);


        public RangeSystem(MapSystem map)
        {
            this.map = map;
        }

        internal override void Update(int msElapsed)
        {
            if(!updateCounter.Tick(msElapsed))
                return;

            foreach(var u in map.Units)
                u.range.FireEvents();
        }
    }
}
