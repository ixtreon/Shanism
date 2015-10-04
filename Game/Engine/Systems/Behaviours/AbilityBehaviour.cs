using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Objects.Game;
using Engine.Systems.Orders;
using IO.Common;

namespace Engine.Systems.Behaviours
{
    /// <summary>
    /// A behaviour that casts the given ability on the specified target, if that is possible. 
    /// </summary>
    class AbilityBehaviour : Behaviour
    {
        private readonly Ability Ability;

        public Unit TargetUnit { get; set; }

        private object AbilityTarget
        {
            get
            {
                if (!Ability.IsActive)
                    throw new Exception("You have put a passive spell in AbilityBehaviour!");

                if (!Ability.IsTargeted)
                    return null;

                if (Ability.CanTargetUnits())
                    return TargetUnit;

                if (Ability.CanTargetGround())
                    return TargetUnit.Position;

                throw new NotImplementedException();

            }
        }

        public AbilityBehaviour(Behaviour b, Ability ab)
            : base(b)
        {
            this.Ability = ab;
        }


        public override bool TakeControl(int msElapsed)
        {
            if (TargetUnit == null)
                return false;
            return Ability.CanCast(AbilityTarget);
        }

        public override void Update(int msElapsed)
        {
            CurrentOrder = new CastOrder(AbilityTarget, Ability);
        }
    }
}
