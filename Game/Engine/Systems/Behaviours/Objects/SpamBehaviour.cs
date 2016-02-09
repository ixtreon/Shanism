using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Entities;
using Engine.Entities.Objects;
using Engine.Systems.Orders;
using Engine.Systems.Abilities;
using IO.Common;

namespace Engine.Systems.Behaviours
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

            Ability = Unit
                .GetAbilitiesOfType(AbilityType.Spammable)
                .FirstOrDefault(a => a.CanCast(TargetUnit));

            return Ability != null;
        }

        public override void Update(int msElapsed)
        {
            CurrentOrder = new CastOrder(Ability, TargetUnit);
        }
    }
}
