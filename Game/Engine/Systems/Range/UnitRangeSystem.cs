using Shanism.Engine.Objects;
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
    class UnitRangeSystem : UnitSystem
    {
        public Unit Origin { get; }

        readonly RangeBuffer distBuffer = new RangeBuffer();

        public UnitRangeSystem(Unit origin)
        {
            Origin = origin;
        }


        internal override void Update(int msElapsed)
        {
            if (!Origin.RangeEvents.Any())
                return;

            var originPos = Origin.Position;
            var maxConstraintRange = Origin.RangeEvents.Max.Range;
            var maxRangeSq = maxConstraintRange * maxConstraintRange;
            var nearbyObjs = Origin.Map
                .RawQuery(new RectangleF(originPos - maxConstraintRange, new Vector(2 * maxConstraintRange)));

            foreach (var otherObject in nearbyObjs)
            {
                if (Origin == otherObject)
                    continue;

                double oldDistSq;
                if (!distBuffer.Front.TryGetValue(otherObject, out oldDistSq))
                    oldDistSq = double.NaN;

                var nowDistSq = otherObject.Position.DistanceToSquared(originPos);
                distBuffer.Back[otherObject] = nowDistSq;

                foreach (var c in Origin.RangeEvents)
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
