using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems.Range
{
    class RangeSystem : UnitSystem
    {
        readonly Unit Owner;

        readonly RangeBuffer distBuffer = new RangeBuffer();


        internal readonly ConcurrentSet<RangeEvent> events = new ConcurrentSet<RangeEvent>();


        public RangeSystem(Unit origin)
        {
            Owner = origin;
        }


        internal override void Update(int msElapsed)
        {
            if (!events.Any())
                return;

            var originPos = Owner.Position;
            var evs = events.ToList();
            var maxConstraintRange = evs.Max(e => e.Range);
            var maxRangeSq = maxConstraintRange * maxConstraintRange;
            var nearbyObjs = Owner.Map
                .RawQuery(new RectangleF(originPos - maxConstraintRange, new Vector(2 * maxConstraintRange)));

            var oldBuffer = distBuffer.Front;
            var newBuffer = distBuffer.Back;

            foreach (var otherObject in nearbyObjs)
            {
                if (Owner == otherObject)
                    continue;

                double oldDistSq;
                if (!oldBuffer.TryGetValue(otherObject, out oldDistSq))
                    oldDistSq = double.NaN;

                double nowDistSq = otherObject.Position.DistanceToSquared(originPos);
                newBuffer[otherObject] = nowDistSq;

                foreach (var c in evs)
                    c.Check(otherObject, nowDistSq, oldDistSq);
            }

            distBuffer.SwapBuffers();
        }
    }

    class RangeBuffer : GenericBuffer<Dictionary<GameObject, double>>
    {
        public override void SwapBuffers()
        {
            base.SwapBuffers();
            Back.Clear();
        }
    }
}
