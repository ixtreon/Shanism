using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Common;

namespace Shanism.Engine.Objects.Orders
{
    class MoveAttack : OrderList
    {
        readonly Stand aggro;
        readonly MoveToGround move;

        public MoveAttack(Unit owner, Vector target) : base(owner)
        {
            Add(aggro = new Stand(owner));
            Add(move = new MoveToGround(owner, target));
        }

    }
}
