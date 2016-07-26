using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Abilities;

namespace Shanism.Engine.Objects.Behaviours
{
    /// <summary>
    /// A behaviour that casts spammable abilities on the specified target, if that is possible. 
    /// </summary>
    class SpamBehaviour : Behaviour
    {
        public Ability Ability { get; private set; }

        public Unit TargetUnit { get; set; }

        public SpamBehaviour(Behaviour b)
            : base(b)
        {

        }


        public override bool TakeControl()
        {
            if (TargetUnit == null)
                return false;

            Ability = Owner
                .GetAbilitiesOfType(AbilityTypeFlags.Spammable)
                .FirstOrDefault(a => a.CanCast(TargetUnit));

            return Ability != null;
        }

        public override void Update(int msElapsed)
        {
            if (Ability.CanTargetUnits)
                Owner.TryCastAbility(Ability, TargetUnit);
            else if (Ability.CanTargetGround)
                Owner.TryCastAbility(Ability, TargetUnit.Position);
            else
                Owner.TryCastAbility(Ability);

        }

        public override string ToString() => $"Cast {Ability}";
    }
}
