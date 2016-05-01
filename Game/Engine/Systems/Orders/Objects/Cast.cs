using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using IO.Common;
using Engine.Objects.Entities;
using Engine.Systems.Behaviours;
using Engine.Systems.Abilities;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// Casts an ability. 
    /// </summary>
    struct CastOrder : IOrder
    {
        public OrderType Type => OrderType.Casting;

        public readonly Ability Ability;

        public readonly object Target;

        public int Progress { get; private set; }


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

        public override string ToString() => $"Cast {Ability}";
    }
}
