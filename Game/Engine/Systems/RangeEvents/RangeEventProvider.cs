using Engine.Maps;
using Engine.Objects;
using Engine.Systems.RangeEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    internal class RangeEventProvider
    {
        //all global constraints
        HashMap<MapConstraint> mapConstraints = new HashMap<MapConstraint>(new IO.Common.Vector(Constants.ObjectMap.CellSize));


        private readonly object _mapLock = new object();
        private readonly object _objectLock = new object();


        public RangeEventProvider()
        {

        }


        public void AddConstraint(MapConstraint c)
        {
            lock (_mapLock)
                mapConstraints.Add(c, c.Origin);
        }

        public void AddConstraint(ObjectConstraint c)
        {
            //lock (_objectLock)
                c.Origin.RangeConstraints.Add(c);
        }

        public bool RemoveConstraint(MapConstraint c)
        {
            lock (_mapLock)
                return mapConstraints.Remove(c, c.Origin);
        }

        /// <summary>
        /// Removes the specified ObjectConstraint from this unit's list of range constriants. 
        /// </summary>
        public bool RemoveConstraint(ObjectConstraint c)
        {
            //lock (_objectLock)
                return c.Origin.RangeConstraints.Remove(c);
        }


        public void CheckAllConstraints(GameObject origin, IEnumerable<GameObject> nearbyObjects, int frame)
        { 
            // continue only if we moved
            // TODO: treshold is arbitrary
            var d = origin.FuturePosition - origin.OldPosition;
            if (origin.FuturePosition.DistanceToSquared(origin.OldPosition) < 1e-8)
                return;

            foreach(var target in nearbyObjects)
            {
                var newDist = target.Position.DistanceTo(origin.Position);
                var oldDist = target.OldPosition.DistanceTo(origin.OldPosition);

                foreach (var c in origin.RangeConstraints)
                    c.Check(target, frame);

                foreach (var c in target.RangeConstraints)
                    c.Check(origin, frame);
            }

            //point links to us!!
        }
    }
}
