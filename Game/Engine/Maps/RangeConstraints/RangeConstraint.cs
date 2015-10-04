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
    /// 
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

        //sees if any of those guys trigger the constraint on its creation
        // candidates must include all possible units, maybe some more. 
        public void CustomCheck(IEnumerable<GameObject> candidates)
        {
            //triggered only for proximity events. can't trigger leave lolz
            if (!ConstraintType.HasFlag(EventType.EntersRange))
                return;

            foreach (var obj in candidates.Where(o => objectCheck(o)))
                OnConstraintActivated(this, EventType.EntersRange, obj);
        }

        public void CustomCheck(GameObject obj)
        {
            //triggered only for proximity events. can't trigger leave lolz
            if (!ConstraintType.HasFlag(EventType.EntersRange))
                return;

            if(objectCheck(obj))
                OnConstraintActivated(this, EventType.EntersRange, obj);
        }

        /// <summary>
        /// Checks whether the given object is or was in range of the origin. 
        /// </summary>
        /// <param name="triggerObject"></param>
        /// <param name="currentLocations">Whether to make the check for the current objects' locations, or to use their previous ones. </param>
        /// <returns>Whether <paramref name="triggerObject"/> is, or was, in range of the origin. </returns>
        bool objectCheck(GameObject triggerObject, bool currentLocations = true)
        {
            if (currentLocations)
                return triggerObject.Position.DistanceTo(OriginLocation) < RangeThreshold;
            else
                return triggerObject.OldPosition.DistanceTo(originOldLocation) < RangeThreshold;
        }

        public virtual void Check(GameObject triggerObject)
        {
            if (Origin == triggerObject)
                return;

            //are we close or not
            var nowCloser = objectCheck(triggerObject, true);
            var wasCloser = objectCheck(triggerObject, false);

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

    public enum TargetType
    {
        SingleObject,
        AllObjects,
    }

    public enum OriginType
    {
        GameObject,
        Location,
    }
}
