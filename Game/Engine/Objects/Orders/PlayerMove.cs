using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common;
using Shanism.Engine.Entities;

namespace Shanism.Engine.Systems.Orders
{
    /// <summary>
    /// An order that instructs the given unit to follow a <see cref="MovementState"/>. 
    /// </summary>
    class PlayerMoveOrder : IMoveOrder
    {
        public double Direction { get; set; }

        public Vector TargetLocation { get; private set; }

        public PlayerMoveOrder(double direction) 
        {
            Direction = direction;
            TargetLocation = Vector.Zero;
        }

        Vector nextLocation(Unit u)
        {
            return u.Position.PolarProjection(Direction, u.MoveSpeed * 0.1);
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
