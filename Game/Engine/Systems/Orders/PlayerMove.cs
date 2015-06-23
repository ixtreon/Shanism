using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;

namespace Engine.Systems.Orders
{
    /// <summary>
    /// An order that instructs the given unit to follow a <see cref="MovementState"/>. 
    /// </summary>
    struct PlayerMoveOrder : Order
    {
        public readonly MovementState State;

        public PlayerMoveOrder(MovementState state) 
        {
            this.State = state;
        }

        public OrderType Type
        {
            get { return OrderType.Move; }
        }

        public bool Update(Unit unit, int msElapsed)
        {
            var v = State.DirectionVector;
            var dist = unit.MoveSpeed * msElapsed / 1000;
            
            unit.Location += v * dist;
            return true;
        }
    }
}
