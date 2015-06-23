using Engine.Events;
using Engine.Objects;
using IO;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Maps
{
    /// <summary>
    /// Represents a distance constraint between a unit and another object 
    /// that raises events when the specified range is crossed. 
    /// 
    /// The other object can be either a game-point (<see cref="Vector"/>) or another <see cref="Unit"/>. 
    /// </summary>
    internal abstract class RangeConstraint
    {
        /// <summary>
        /// The range events this constraint responds to. 
        /// </summary>
        public readonly EventType ConstraintType;

        /// <summary>
        /// Gets the type of the second object in the constraint. 
        /// </summary>
        public readonly OriginType OriginType;


        public readonly object Origin;

        /// <summary>
        /// Gets the distance threshold at which the events should be fired. 
        /// </summary>
        public readonly double RangeThreshold;

        public readonly Action<RangeArgs> Handler;

        public Vector OriginOldLocation
        {
            get
            {
                switch (OriginType)
                {
                    case OriginType.AnotherUnit:
                        return ((Unit)Origin).OldLocation;
                    case OriginType.Location:
                        return ((Vector)Origin);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public Vector OriginLocation
        {
            get
            {
                switch(OriginType)
                {
                    case OriginType.AnotherUnit:
                        return ((Unit)Origin).Location;
                    case OriginType.Location:
                        return ((Vector)Origin);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
        

        protected RangeConstraint(Vector origin, double range, EventType type, Action<RangeArgs> act)
        {
            OriginType = OriginType.Location;
            RangeThreshold = range;
            ConstraintType = type;
            Origin = origin;
            Handler = act;
        }

        protected RangeConstraint(Unit origin, double range, EventType type, Action<RangeArgs> act)
        {
            OriginType = OriginType.AnotherUnit;
            RangeThreshold = range;
            ConstraintType = type;
            Origin = origin;
            Handler = act;
        }

        public virtual void Check(Unit u)
        {
            if (Origin == u)
                return;

            if (u.IsDead)
                throw new Exception("Unit dead yo!");

            //get the distance before and now
            var newDistance = (u.Location - OriginLocation).Length();
            var oldDistance = (u.OldLocation - OriginOldLocation).Length();

            //are we close or not
            var wasCloser = (oldDistance < RangeThreshold);
            var nowCloser = (newDistance < RangeThreshold);

            //continue only if change in closeness
            if (wasCloser == nowCloser)
                return;

            //the actual event that caused this raise
            var evType = (wasCloser) ? (EventType.LeavesRange) : (EventType.EntersRange);

            //check if we actually listen for that event
            //either we listen to both, or evType must correspond to our type
            if (ConstraintType != EventType.LeavesOrEnters && evType != ConstraintType)
                return;

            //raise the actual event
            var evArgs = new RangeArgs(this, evType, u);

            if(Handler != null)
                Handler(evArgs);
        }


    }

    internal class GlobalRangeConstraint : RangeConstraint
    {
        //any unit approaches (leaves) the location o
        public GlobalRangeConstraint(Vector o, double range, EventType type, Action<RangeArgs> act)
            : base(o, range, type, act) { }

        //any unit approaches the unit o
        public GlobalRangeConstraint(Unit o, double range, EventType type, Action<RangeArgs> act)
            : base(o, range, type, act) { }
    }

    internal class UnitRangeConstraint : RangeConstraint
    {
        internal Unit Target;

        public UnitRangeConstraint(Unit tar, Vector o, double range, EventType type, Action<RangeArgs> act)
            : base(o, range, type, act)
        {
            this.Target = tar;
        }

        //any unit approaches the unit o
        public UnitRangeConstraint(Unit tar, Unit o, double range, EventType type, Action<RangeArgs> act)
            : base(o, range, type, act)
        {
            this.Target = tar;
        }

        public override void Check(Unit u)
        {
            if (u != Target)
                throw new InvalidOperationException("Verifiying a UnitRangeConstraint with non-related unit!");

            base.Check(u);
        }
    }

    /// <summary>
    /// A collection of the events that range constraints (see <see cref="RangeConstraint"/>) can respond to. 
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Indicates that an event is to be raised whenever the distance becomes greater than the threshold. 
        /// </summary>
        LeavesRange,
        /// <summary>
        /// Indicates that an event is to be raised whenever the distance becomes less than the threshold. 
        /// </summary>
        EntersRange,

        /// <summary>
        /// Indicates that an event is to be raised whenever the threshold line is crossed. 
        /// </summary>
        LeavesOrEnters,
    }

    public enum TargetType
    {
        SingleUnit,

        AnyUnit,
    }

    public enum OriginType
    {
        AnotherUnit,
        Location,
    }
}
