using Shanism.Engine.Entities;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Common;

namespace Shanism.Engine.Systems.Orders
{
    /// <summary>
    /// Moves to or starts following a given unit. 
    /// </summary>
    class MoveUnit : IMoveOrder
    {
        public OrderType Type => OrderType.Move;


        /// <summary>
        /// Gets or sets the distance from the target to stop at. 
        /// </summary>
        public double DistanceThrehsold { get; set; }

        public readonly Unit TargetUnit;

        /// <summary>
        /// Gets or sets whether to keep following the target once it is reached. 
        /// </summary>
        public bool KeepFollowing { get; set; }

        public float Direction { get; private set; }

        public Vector TargetLocation { get; private set; }

        public MoveUnit(Unit target, double distThreshold = 0.05, bool keepFollowing = true)
        {
            KeepFollowing = keepFollowing;
            TargetUnit = target;
            DistanceThrehsold = distThreshold;

            Direction = 0;
            TargetLocation = target.Position;
        }


        public bool Update(Unit unit, int msElapsed)
        {
            //can't follow dead units, aye?
            if (TargetUnit.IsDead)
                return false;

            //get the distance to the target
            var uLoc = unit.Position;
            var targetLoc = TargetUnit.Position;
            var distanceLeft = uLoc.DistanceTo(targetLoc);

            //return if already there
            if (distanceLeft < DistanceThrehsold)
                return KeepFollowing;

            //and move it
            Direction = (float)uLoc.AngleTo(targetLoc);
            unit.movement.SetMovementState(Direction, distanceLeft);

            //keep on
            return true;

        }

        public override bool Equals(object obj)
        {
            if (!(obj is MoveUnit))
                return false;

            var other = (MoveUnit)obj;
            return TargetUnit == other.TargetUnit
                && KeepFollowing == other.KeepFollowing
                && DistanceThrehsold == other.DistanceThrehsold;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString() => $"Move to {TargetUnit}";
    }
}
