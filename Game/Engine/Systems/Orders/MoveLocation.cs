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
    struct MoveLocation : IMoveOrder
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

        public double Direction { get; private set; }
        public Vector SuggestedLocation { get; private set; }

        public MoveLocation(Vector target, double distanceThreshold = 0.05)
        {
            this.TargetLocation = target;
            this.DistanceThrehsold = distanceThreshold;
            Direction = -1;
            SuggestedLocation = target;
        }

        public bool Update(Unit unit, int msElapsed)
        {
            //move the unit towards its target
            var uLoc = unit.Position;
            var dist = uLoc.DistanceTo(TargetLocation);

            //return if already there
            if (dist < DistanceThrehsold)
                return false;

            //move the unit
            Direction = uLoc.AngleTo(TargetLocation);
            unit.Move(msElapsed, Direction, dist);

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
