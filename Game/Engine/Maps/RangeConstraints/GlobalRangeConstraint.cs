using Engine.Events;
using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    /// <summary>
    /// A constraint between a GameObject or a point and all other GameObjects
    /// </summary>
    internal class GlobalRangeConstraint<T> : RangeConstraint
        where T : GameObject
    {
        protected readonly Action<RangeArgs<T>> EventHandler;

        //any object approaches (leaves) the location o
        public GlobalRangeConstraint(Vector origin, double range, EventType type, Action<RangeArgs<T>> act)
            : base(origin, range, type)
        {
            EventHandler = act;
        }

        //any object approaches the object o
        public GlobalRangeConstraint(GameObject origin, double range, EventType type, Action<RangeArgs<T>> act)
            : base(origin, range, type)
        {
            EventHandler = act;
        }

        protected override void OnConstraintActivated(RangeConstraint constraint, EventType eventType, GameObject triggerObject)
        {
            if (triggerObject is T)
                EventHandler?.Invoke(new RangeArgs<T>(constraint, eventType, (T)triggerObject));
        }
    }
}
