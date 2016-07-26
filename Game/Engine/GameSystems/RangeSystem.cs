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
    class RangeSystem : GameSystem
    {
        public override string SystemName => "Range Events";

        readonly MapSystem map;

        readonly Counter c = new Counter(1000 / 20);    //25 fps max

        public RangeSystem(MapSystem map)
        {
            this.map = map;
        }

        internal override void Update(int msElapsed)
        {
            if (c.Tick(msElapsed))
            {
                //query tree for range events
                foreach (var u in map.Units)
                {
                    var nearbies = u.nearbyEntities;

                    var maxRange = u.range.MaxEventRange;
                    map.GetObjectsInRect(u.Position, new Vector(maxRange), nearbies);

                    foreach (var o in nearbies)
                        updateUnit(u, o);
                }
            }
        }



        static void updateUnit(Unit origin, Entity nearbyObject)
        {
            double dOld, dNew;
            refreshDistances(origin, nearbyObject, out dOld, out dNew);

            //short-circuit if change not "big enough"
            if (Math.Abs(dOld - dNew) < 1e-5)
                return;

            //get the type of event
            double minD, maxD;
            RangeEventTriggerType tty;
            if (dOld < dNew)
            {
                minD = dOld;
                maxD = dNew;
                tty = RangeEventTriggerType.Leave;
            }
            else
            {
                minD = dNew;
                maxD = dOld;
                tty = RangeEventTriggerType.Enter;
            }

            //raise relevant events
            foreach (var ev in origin.range.SortedEvents)
            {
                if (ev.RangeSquared < minD)
                    continue;

                if (ev.RangeSquared > maxD)
                    break;

                ev.Raise(nearbyObject, tty);
            }
        }

        static void refreshDistances(Unit origin, Entity nearbyObject, out double dOld, out double dNew)
        {
            var nearDists = origin.nearbyDistances;

            //get old distance
            if (!nearDists.TryGetValue(nearbyObject, out dOld))
                dOld = double.MaxValue;

            //get new distance
            if (nearbyObject.IsDestroyed)
            {
                dNew = double.MaxValue;
                nearDists.Remove(nearbyObject);
                return;
            }

            dNew = nearbyObject.Position.DistanceToSquared(origin.Position);
            nearDists[nearbyObject] = dNew;
        }
    }
}
