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
            var b = Owner.Behaviour;
            if (Owner.IsDead || b == null)
                return;

            if (b.TakeControl())
            {
                b.Update(msElapsed);

                if (!Owner.CustomOrder && Owner.Order != b.CurrentOrder)
                    Owner.SetOrder(b.CurrentOrder, false);
            }
        }
    }
}
