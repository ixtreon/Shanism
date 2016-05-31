using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Common.Game;

namespace Shanism.Engine.Systems.Orders
{
    /// <summary>
    /// Simply stands at a place. 
    /// </summary>
    struct Stand : IOrder
    {
        public OrderType Type => OrderType.Stand;


        public bool Update(Unit u, int msElapsed)
        {
            u.movement.Stop();
            return true;
        }

        public override string ToString() => "Stand";
    }
}
