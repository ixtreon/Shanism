using Shanism.Common;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Orders
{
    /// <summary>
    /// A behaviour that makes the controlled unit go in the given direction. 
    /// </summary>
    class MoveDirection : Order
    {
        public new MovementState CurrentState
        {
            get { return base.CurrentState; }
            set { base.CurrentState = value; }
        }
        public MoveDirection(Unit owner) : base(owner)
        {
        }

        public MoveDirection(Unit owner, float ang) : base(owner)
        {
            CurrentState = new MovementState(ang);
        }

        public override bool TakeControl()
        {
            return true;
        }

        public override void Update(int msElapsed)
        {
            //currentstate is already set by user
        }

        public override string ToString() => $"Moving @ {CurrentState}";
    }
}
