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
    internal class UnitRangeConstraint : RangeConstraint
    {

        protected readonly Unit Target;

        protected readonly Action<RangeArgs<Unit>> EventHandler;

        public UnitRangeConstraint(Unit tar, Vector o, double range, EventType type, Action<RangeArgs<Unit>> act)
            : base(o, range, type)
        {
            this.Target = tar;
            EventHandler = act;
        }

        //any unit approaches the unit o
        public UnitRangeConstraint(Unit tar, Unit o, double range, EventType type, Action<RangeArgs<Unit>> act)
            : base(o, range, type)
        {
            this.Target = tar;
            EventHandler = act;
        }

        public override void Check(GameObject obj)
        {
            if (!(obj is Unit) || obj != Target)
                throw new InvalidOperationException("Verifiying a UnitRangeConstraint with non-related object!");

            base.Check(obj);
        }

        protected override void OnConstraintActivated(RangeConstraint constraint, EventType eventType, GameObject triggerObject)
        {
            if (!(triggerObject is Unit) || triggerObject != Target)
                throw new InvalidOperationException("Verifiying a UnitRangeConstraint with non-related object!");

            if (triggerObject is Unit && triggerObject == Target)
                EventHandler?.Invoke(new RangeArgs<Unit>(constraint, eventType, (Unit)triggerObject));
        }
    }
}
