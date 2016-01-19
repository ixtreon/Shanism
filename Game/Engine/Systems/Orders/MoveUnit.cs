using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// Moves to or starts following a given unit. 
    /// </summary>
    struct MoveUnit : IMoveOrder
    {
        /// <summary>
        /// Gets or sets the distance from the target to stop at. 
        /// </summary>
        public double DistanceThrehsold { get; set; }

        public OrderType Type
        {
            get
            {
                return OrderType.Move;
            }
        }


        public readonly Unit TargetUnit;

        /// <summary>
        /// Gets or sets whether to keep following the target once it is reached. 
        /// </summary>
        public bool KeepFollowing { get; set; }

        public double Direction { get; private set; }
        public Vector TargetLocation { get; private set; }

        public MoveUnit(Unit target, double distThreshold = 0.05, bool keepFollowing = true)
        {
            this.KeepFollowing = keepFollowing;
            this.TargetUnit = target;
            this.DistanceThrehsold = distThreshold;
            Direction = -1;
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
            Direction = uLoc.AngleTo(targetLoc);
            unit.Move(msElapsed, Direction, distanceLeft);

            //keep on
            return true;

        }

        public override bool Equals(object other)
        {
            if (!(other is MoveUnit))
                return false;

            return (MoveUnit)other == this;
        }

        public static bool operator ==(MoveUnit o1, MoveUnit o2)
        {
            return o1.TargetUnit == o2.TargetUnit
                && o1.KeepFollowing == o2.KeepFollowing
                && o1.DistanceThrehsold == o2.DistanceThrehsold;
        }

        public static bool operator !=(MoveUnit o1, MoveUnit o2)
        {
            return !(o1 == o2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
