using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Common.Message;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems
{
    class BehaviourSystem : UnitSystem
    {
        readonly Unit Owner;

        public BehaviourSystem(Unit owner)
        {
            Owner = owner;
        }

        public override void Update(int msElapsed)
        {
            if (Owner.IsDead 
                || Owner.Behaviour == null 
                || Owner.States.HasFlag(StateFlags.Stunned))
                return;

            Owner.Behaviour.Update(msElapsed);

            if (!Owner.CustomOrder && Owner.Behaviour.CurrentOrder != Owner.Order)
                Owner.SetOrder(Owner.Behaviour.CurrentOrder, false);
        }
    }
}
