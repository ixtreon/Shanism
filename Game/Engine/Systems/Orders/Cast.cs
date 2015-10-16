using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using IO.Common;
using Engine.Objects.Game;
using Engine.Systems.Behaviours;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// Casts an ability. 
    /// </summary>
    struct CastOrder : IOrder
    {
        public readonly Ability Ability;

        public readonly object Target;

        public int Progress { get; private set; }

        public OrderType Type
        {
            get { return OrderType.Casting; }
        }

        public CastOrder(Ability ability, object target)
        {
            this.Target = target;
            this.Ability = ability;
            this.Progress = 0;
        }

        public bool Update(Unit unit, int msElapsed)
        {
            Progress += msElapsed;

            //continue only if finished casting
            if (Progress < Ability.CastTime)
                return true;

            //cast the spell
            unit.activateAbility(Ability, Target);
            Progress = 0;
            return false;
        }

        // DOES NOT CHECK PROGRESS
        // cuz an order is still the same, right?
        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;

            var oa = (CastOrder)other;
            return Ability == oa.Ability && Target == oa.Target;    // the last check is flaky for point targets; who cares..
        }

        public static bool operator ==(CastOrder o1, CastOrder o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(CastOrder o1, CastOrder o2)
        {
            return !o1.Equals(o2);
        }
    }
}
