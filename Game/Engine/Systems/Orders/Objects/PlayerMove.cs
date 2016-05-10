using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects;
using Shanism.Common.Game;
using Shanism.Common;

namespace Shanism.Engine.Systems.Orders
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

        public OrderType Type => OrderType.Move;

        public bool Update(Unit unit, int msElapsed)
        {
            TargetLocation = nextLocation(unit);
            unit.movement.SetMovementState(Direction);

            return true;
        }

        public override string ToString() => $"Move @ {Direction}rad";
    }
}
