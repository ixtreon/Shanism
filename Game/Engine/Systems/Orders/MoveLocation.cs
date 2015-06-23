using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// Moves to a specified target position. 
    /// </summary>
    struct MoveLocation : Order
    {
        /// <summary>
        /// The distance from the target to stop at. 
        /// 
        /// The default value is 0.05, or a 1/20th of a square
        /// </summary>
        public readonly double DistanceThrehsold;

        /// <summary>
        /// Gets the target location. 
        /// </summary>
        public readonly Vector TargetLocation;


        public OrderType Type
        {
            get { return OrderType.Move; }
        }


        public MoveLocation(Vector target, double distanceThreshold = 0.05)
        {
            this.TargetLocation = target;
            this.DistanceThrehsold = distanceThreshold;
        }

        public bool Update(Unit unit, int msElapsed)
        {
            //move the unit towards its target
            var uLoc = unit.Location;
            var dist = uLoc.DistanceTo(TargetLocation);

            //return if already there
            if (dist < DistanceThrehsold)
                return false;

            //calculate the distance
            var ang = uLoc.AngleTo(TargetLocation);
            var moveDist = Math.Min(dist, unit.MoveSpeed * msElapsed / 1000);
            var pos = uLoc.PolarProjection(ang, moveDist);

            //move the unit
            unit.Location = pos;

            //keep on
            return true;
        }

        public static bool operator ==(MoveLocation o1, MoveLocation o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(MoveLocation o1, MoveLocation o2)
        {
            return !o1.Equals(o2);
        }
    }
}
