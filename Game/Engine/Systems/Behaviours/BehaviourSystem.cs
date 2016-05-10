using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects;
using Shanism.Common.Message;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems
{
    class BehaviourSystem : UnitSystem
    {
        public Unit Owner { get; }

        public BehaviourSystem(Unit owner)
        {
            Owner = owner;
        }

        internal override void Update(int msElapsed)
        {
            if (Owner.IsDead || Owner.States.HasFlag(UnitFlags.Stunned) || Owner.Behaviour == null)
                return;

            //update behaviour
            Owner.Behaviour.Update(msElapsed);

            // if custom order, or behaviour's order hasn't changed
            // return
            if (Owner.CustomOrder
                || Owner.Behaviour.CurrentOrder == Owner.Order)
                return;

            //apply behaviour
            Owner.SetOrder(Owner.Behaviour.CurrentOrder, false);
        }
    }
}
