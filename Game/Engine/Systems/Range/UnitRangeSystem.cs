using Engine.Entities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems.Range
{
    class UnitRangeSystem : UnitSystem
    {
        public Unit Origin { get; }


        public UnitRangeSystem(Unit origin)
        {
            Origin = origin;
        }


        internal override void Update(int msElapsed)
        {
            if (!Origin.RangeEvents.Any())
                return;

            var maxConstraintRange = Origin.RangeEvents.Max.Range;
            var maxRangeSq = maxConstraintRange * maxConstraintRange;
            var nearbyObjs = Origin.Map.Map
                .RawQuery(new RectangleF(Origin.Position - maxConstraintRange, new Vector(2 * maxConstraintRange)));

            foreach (var otherObject in nearbyObjs)
            {
                if (Origin == otherObject)
                    continue;

                var objDistSq = otherObject.Position.DistanceToSquared(Origin.Position);
                foreach (var c in Origin.RangeEvents)
                    c.Check(otherObject, objDistSq);
            }
        }
    }
}
