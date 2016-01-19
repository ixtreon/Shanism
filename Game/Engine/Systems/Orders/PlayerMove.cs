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
    /// An order that instructs the given unit to obey a <see cref="MovementState"/>. 
    /// </summary>
    struct PlayerMoveOrder : IMoveOrder
    {
        public readonly MovementState State;

        public double Direction { get; set; }

        public Vector TargetLocation { get; private set; }

        public PlayerMoveOrder(MovementState state) 
        {
            this.State = state;

            Direction = state.DirectionVector.Angle;
            TargetLocation = Vector.Zero;
        }

        Vector nextLocation(Unit u)
        {
            return u.Position.PolarProjection(State.DirectionVector.Angle, u.MoveSpeed * 0.1);
        }

        public OrderType Type
        {
            get { return OrderType.Move; }
        }

        public bool Update(Unit unit, int msElapsed)
        {
            TargetLocation = nextLocation(unit);
            unit.Move(msElapsed, Direction);

            return true;
        }
    }
}
