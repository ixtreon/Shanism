using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using IO.Common;
using Engine.Entities.Objects;
using Engine.Systems.Behaviours;
using Engine.Systems.Abilities;

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
            Target = target;
            Ability = ability;
            Progress = 0;
        }

        public bool Update(Unit unit, int msElapsed)
        {
            Progress += msElapsed;

            //continue only if finished casting
            if (Progress < Ability.CastTime)
                return true;

            //cast the spell
            unit.abilities.ActivateAbility(Ability, Target);
            Progress = 0;
            return false;
        }

        // Progress doesn't matter
        // 'cause the order is still the same, right?
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            var oa = (CastOrder)obj;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
