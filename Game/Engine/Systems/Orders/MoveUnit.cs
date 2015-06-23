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
    struct MoveUnit : Order
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

        public MoveUnit(Unit target, double distThreshold = 0.05, bool keepFollowing = true)
        {
            this.KeepFollowing = keepFollowing;
            this.TargetUnit = target;
            this.DistanceThrehsold = distThreshold;
        }


        public bool Update(Unit unit, int msElapsed)
        {
            //can't follow dead units, aye?
            if (TargetUnit.IsDead)
                return false;


            //get the distance to the target
            var uLoc = unit.Location;
            var targetLoc = TargetUnit.Location;
            var dist = uLoc.DistanceTo(targetLoc);

            //return if already there
            if (dist < DistanceThrehsold)
                return KeepFollowing;

            //get the new position of the unit
            var ang = uLoc.AngleTo(targetLoc);
            var moveDist = Math.Min(dist, unit.MoveSpeed * msElapsed / 1000);
            var pos = uLoc.PolarProjection(ang, moveDist);

            //and move it
            unit.Location = pos;

            //keep on
            return true;

        }

        
        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;

            var oa = (MoveUnit)other;
            return TargetUnit == oa.TargetUnit
                && KeepFollowing == oa.KeepFollowing
                && DistanceThrehsold == oa.DistanceThrehsold;
        }

        public static bool operator ==(MoveUnit o1, MoveUnit o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(MoveUnit o1, MoveUnit o2)
        {
            return !o1.Equals(o2);
        }
    }
}
