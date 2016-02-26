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
        public double DistanceThrehsold { get; }

        /// <summary>
        /// Gets the target location. 
        /// </summary>
        public Vector TargetLocation { get; }


        public OrderType Type => OrderType.Move;

        public double Direction { get; private set; }

        public MoveLocation(Vector target, double distanceThreshold = 0.05)
        {
            DistanceThrehsold = distanceThreshold;
            Direction = -1; //wat
            TargetLocation = target;
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
            unit.movement.SetMovementState(Direction, dist);

            //var potential =Vector.Zero;

            //var nearbyUnits = unit.Map.GetObjectsInRange(unit.Position, 10)
            //    .Where(u => u.HasCollision);


            //keep on
            return true;
        }

        void addForce(Vector potential, Vector pos, Vector target, double power)
        {
            potential += (1);
        }

        public static bool operator ==(MoveLocation o1, MoveLocation o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(MoveLocation o1, MoveLocation o2)
        {
            return !(o1 == o2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MoveLocation))
                return false;
            return (MoveLocation)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
