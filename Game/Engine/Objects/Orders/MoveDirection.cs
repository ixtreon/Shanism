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
    /// The order for a unit to move in the specified direction. 
    /// </summary>
    class MoveDirection : Order
    {
        float _angle = float.NaN;

        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                CurrentState = new MovementState(value);
            }
        }

        public MoveDirection(Unit owner) : base(owner)
        {
        }

        public MoveDirection(Unit owner, float ang) : base(owner)
        {
            Angle = ang;
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
