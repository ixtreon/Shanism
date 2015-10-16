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
    /// Represents an abstract distance constraint between an origin GameObject or point and one or all other GameObjects
    /// that raises events when the specified range is crossed. 
    /// </summary>
    abstract class RangeConstraint : IRangeConstraint
    {
        /// <summary>
        /// The range events this constraint responds to. 
        /// </summary>
        public EventType ConstraintType { get; private set; }

        /// <summary>
        /// Gets the type of the second object in the constraint. 
        /// </summary>
        public OriginType OriginType { get; private set; }

        /// <summary>
        /// Gets the origin of the constraint. 
        /// Currently either a <see cref="GameObject"/> or a <see cref="Vector"/>. 
        /// </summary>
        public object Origin { get; private set; }

        /// <summary>
        /// Gets the distance threshold at which the events should be fired. 
        /// </summary>
        public double RangeThreshold { get; private set; }

        protected Vector originOldLocation
        {
            get
            {
                switch (OriginType)
                {
                    case OriginType.GameObject:
                        return ((Unit)Origin).OldPosition;
                    case OriginType.Location:
                        return ((Vector)Origin);
                }
                throw new InvalidOperationException();
            }
        }

        public Vector OriginLocation
        {
            get
            {
                switch(OriginType)
                {
                    case OriginType.GameObject:
                        return ((Unit)Origin).Position;
                    case OriginType.Location:
                        return ((Vector)Origin);
                }
                throw new InvalidOperationException();
            }
        }
        

        protected RangeConstraint(Vector origin, double range, EventType type)
        {
            OriginType = OriginType.Location;
            RangeThreshold = range;
            ConstraintType = type;
            Origin = origin;
        }

        protected RangeConstraint(GameObject origin, double range, EventType type)
        {
            OriginType = OriginType.GameObject;
            RangeThreshold = range;
            ConstraintType = type;
            Origin = origin;
        }


        public virtual void Check(GameObject triggerObject)
        {
            // sanity check
            if (Origin == triggerObject)    
                return;

            // check if we are/were close 
            var nowCloser = triggerObject.Position.DistanceTo(OriginLocation) < RangeThreshold;
            var wasCloser = triggerObject.OldPosition.DistanceTo(originOldLocation) < RangeThreshold;

            // continue only if closeness changed
            if (wasCloser == nowCloser)
                return;

            //check if we actually listen for that event
            //either we listen to both, or evType must correspond to our type
            var evType = (wasCloser) ? (EventType.LeavesRange) : (EventType.EntersRange);
            if (!(ConstraintType == EventType.LeavesOrEnters || evType == ConstraintType))
                return;

            //finally raise the event
            OnConstraintActivated(this, evType, triggerObject);

        }

        protected abstract void OnConstraintActivated(RangeConstraint constraint, EventType eventType, GameObject triggerObject);
    }


    /// <summary>
    /// The events that range constraints (see <see cref="RangeConstraint"/>) can respond to. 
    /// Basically either object enters or leaves range; or both.  
    /// </summary>
    [Flags]
    public enum EventType
    {
        /// <summary>
        /// Indicates that an event is to be raised whenever the distance becomes greater than the threshold. 
        /// </summary>
        LeavesRange = 1,
        /// <summary>
        /// Indicates that an event is to be raised whenever the distance becomes less than the threshold. 
        /// </summary>
        EntersRange = 2,

        /// <summary>
        /// Indicates that an event is to be raised whenever the threshold line is crossed. 
        /// </summary>
        LeavesOrEnters = LeavesRange | EntersRange,
    }


    public enum OriginType
    {
        GameObject,
        Location,
    }
}
