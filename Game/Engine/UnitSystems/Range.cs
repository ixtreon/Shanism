using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects.Range;
using System.Threading;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Tracks nearby units and raises range events (see <see cref="RangeEvent"/>). 
    /// </summary>
    /// <seealso cref="Shanism.Engine.Systems.UnitSystem" />
    class RangeSystem : UnitSystem
    {
        static readonly GenericComparer<RangeEvent> eventComparer = new GenericComparer<RangeEvent>((a, b) => a.Range.CompareTo(b.Range));



        readonly Unit Owner;

        readonly SortedSet<RangeEvent> events = new SortedSet<RangeEvent>(eventComparer);

        readonly HashSet<Entity> nearbyEntities = new HashSet<Entity>();

        //the latest _squared_ distances to all nearby entities.
        readonly Dictionary<Entity, double> nearbyDistancesSquared = new Dictionary<Entity, double>();


        public RangeSystem(Unit origin)
        {
            Owner = origin;
        }

        public IEnumerable<RangeEvent> SortedEvents => events;

        public bool AddEvent(RangeEvent e) => events.Add(e);

        public bool RemoveEvent(RangeEvent e) => events.Remove(e);


        double MaxEventRange => events.Max.Range;

        double MaxEventRangeSquared => events.Max.RangeSquared;


        internal void FireEvents()
        {
            if (Owner.IsDead)
            {
                double d;
                foreach (var e in nearbyEntities)
                    foreach (var ev in Owner.range.events)
                        if ((d = Owner.Position.DistanceToSquared(e.Position)) < ev.RangeSquared)
                            ev.Raise(e, RangeEventTriggerType.Leave);
                nearbyEntities.Clear();
                return;
            }

            //refresh the nearby units field
            Owner.Map.GetObjectsInRect(Owner.Position, new Vector(MaxEventRange), nearbyEntities);

            //fire events, remove units that left range
            nearbyEntities.RemoveWhere(updateEntity);
        }

        /// <summary>
        /// Fires all events related to the entity at question
        /// by checking if it crossed some <see cref="RangeEvent"/> boundary
        /// since the last method call.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Whether the entity should be removed from the <see cref="nearbyEntities"/> list.</returns>
        bool updateEntity(Entity other)
        {
            double dOld, dNew;
            var isTooFarAway = refreshDistances(other, out dOld, out dNew);

            //short-circuit if change not "big enough"
            if (Math.Abs(dOld - dNew) < 1e-5)
                return isTooFarAway;

            //get the event type/args
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


            //raise events, skipping some or all of 'em
            foreach (var ev in events)
            {
                if (ev.RangeSquared <= minD) continue;
                if (ev.RangeSquared > maxD) break;
                ev.Raise(other, tty);
            }

            return isTooFarAway;
        }

        /// <summary>
        /// Refreshes the distances between the Owner and the other entity.
        /// Makes sure <see cref="nearbyDistancesSquared"/> contains only units that
        /// are in the same map, and at most <see cref="MaxEventRange"/> units away.
        /// Returns whether the other entity has left our range. 
        /// </summary>
        bool refreshDistances(Entity other,
            out double dOld, out double dNew)
        {
            //old dist
            if (!nearbyDistancesSquared.TryGetValue(other, out dOld))
                dOld = double.MaxValue;         //coming from really far

            //new dist
            if (other.IsDestroyed)
                dNew = double.MaxValue;         //going really far
            else
                dNew = other.Position.DistanceToSquared(Owner.Position);

            //remove from distance-map
            if (dOld > MaxEventRangeSquared && dNew > MaxEventRangeSquared)
            {
                nearbyDistancesSquared.Remove(other);
                return true;
            }
            else
            {
                nearbyDistancesSquared[other] = dNew;      //otherwise update pos & return
                return false;
            }
        }
    }
}
