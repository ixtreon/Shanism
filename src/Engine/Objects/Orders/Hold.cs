using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// Instructs the unit to hold its ground and do nothing.
    /// </summary>
    /// <seealso cref="Shanism.Engine.Objects.Orders.Order" />
    class Hold : Order
    {
        protected Hold(Unit owner) : base(owner) { }

        public override bool TakeControl()
        {
            return true;
        }

        public override void Update(int msElapsed) { }
    }
}
