using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Entities;
using Shanism.Engine.Systems.Orders;
using Shanism.Engine.Systems.Abilities;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems.Behaviours
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


        public override bool TakeControl(int msElapsed)
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
            Owner.CastAbility(Ability, TargetUnit);
        }

        public override string ToString() => $"Cast {Ability}";
    }
}
