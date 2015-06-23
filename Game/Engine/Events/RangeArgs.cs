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
    /// The arguments for whenever a unit comes in range or leaves range. 
    /// </summary>
    public class RangeArgs
    {
        public readonly EventType ConstraintType;

        public readonly EventType EventType;

        public readonly OriginType OriginType;

        public readonly object Origin;

        public readonly double RangeThreshold;

        public readonly Unit TriggerUnit;

        public Unit OriginUnit
        {
            get
            {
                return Origin as Unit;
            }
        }

        public readonly Vector OriginLocation;

        internal RangeArgs(RangeConstraint c, EventType evType, Unit u)
        {
            EventType = evType;
            ConstraintType = c.ConstraintType;
            OriginType = c.OriginType;
            Origin = c.Origin;
            OriginLocation = c.OriginLocation;
            RangeThreshold = c.RangeThreshold;
            TriggerUnit = u;
        }

    }
}
