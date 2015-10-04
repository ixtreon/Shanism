using Engine.Maps;
using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Events
{
    /// <summary>
    /// The arguments for whenever a GameObject comes in range or leaves range from another unit or object. 
    /// </summary>
    /// <typeparam name="T">The type of GameObject that triggered the constraint. </typeparam>
    public class RangeArgs<T>
        where T : GameObject
    {
        public readonly EventType ConstraintType;

        public readonly EventType EventType;

        public readonly OriginType OriginType;

        public readonly object Origin;

        public readonly double RangeThreshold;

        public readonly T TriggerObject;

        public Unit OriginUnit
        {
            get
            {
                return Origin as Unit;
            }
        }

        public readonly Vector OriginLocation;

        internal RangeArgs(RangeConstraint c, EventType evType, T obj)
        {
            EventType = evType;
            ConstraintType = c.ConstraintType;
            OriginType = c.OriginType;
            Origin = c.Origin;
            OriginLocation = c.OriginLocation;
            RangeThreshold = c.RangeThreshold;
            TriggerObject = obj;
        }

    }
}
