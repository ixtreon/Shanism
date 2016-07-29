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
        List<Ability> spammableAbilities;

        public Ability CurrentAbility { get; private set; }

        public Unit TargetUnit { get; set; }

        public SpamBehaviour(Behaviour b)
            : base(b)
        {
            Owner.abilities.OnAbilityLearned += owner_abilitiesChanged;
            Owner.abilities.OnAbilityLost += owner_abilitiesChanged;
            owner_abilitiesChanged(null);
        }

        private void owner_abilitiesChanged(Ability a)
        {
            spammableAbilities = Owner
                .GetAbilitiesOfType(AbilityTypeFlags.Spammable)
                .ToList();
        }

        public override bool TakeControl()
        {
            if (TargetUnit == null)
                return false;

            CurrentAbility = null;
            foreach (var ab in spammableAbilities)
                if (ab.CanCast(TargetUnit))
                {
                    CurrentAbility = ab;
                    break;
                }

            return CurrentAbility != null;
        }

        public override void Update(int msElapsed)
        {
            if (CurrentAbility.CanTargetUnits)
                Owner.TryCastAbility(CurrentAbility, TargetUnit);
            else if (CurrentAbility.CanTargetGround)
                Owner.TryCastAbility(CurrentAbility, TargetUnit.Position);
            else
                Owner.TryCastAbility(CurrentAbility);

        }

        public override string ToString() => $"Cast {CurrentAbility}";
    }
}
