using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Entities;
using IO.Message;
using IO.Common;

namespace Engine.Systems
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
            if (Owner.IsDead || Owner.StateFlags.HasFlag(UnitState.Stunned) || Owner.Behaviour == null)
                return;

            //update behaviour
            Owner.Behaviour.Update(msElapsed);

            if (Owner.CustomOrder || Owner.Behaviour.CurrentOrder == Owner.Order)
                return;

            //apply behaviour
            Owner.SetOrder(Owner.Behaviour.CurrentOrder, false);
        }
    }
}
