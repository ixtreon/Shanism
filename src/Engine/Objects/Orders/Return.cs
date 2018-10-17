using Ix.Math;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Engine.Objects.Orders
{

    class ReturnOrder : Order
    {
        public Vector2 ReturnPosition { get; set; }

        public double ReturnStartDistance { get; set; }

        public bool Returning { get; private set; }

        /// <summary>
        /// Fired whenever the unit starts returning. 
        /// </summary>
        public event Action OnReturnStarted;

        public event Action OnReturnFinished;

        public ReturnOrder(Order b, Vector2 origin, double maxDistance) 
            : base(b)
        {
            ReturnPosition = origin;
            ReturnStartDistance = maxDistance;
        }


        public override bool TakeControl()
        {
            return Returning 
                || Owner.Position.DistanceTo(ReturnPosition) > ReturnStartDistance;
        }


        public override void Update(int msElapsed)
        {
            //raise the event
            if (!Returning)
            {
                Returning = true;
                OnReturnStarted?.Invoke();
            }

            var ang = (float)Owner.Position.AngleTo(ReturnPosition);
            CurrentState = new MovementState(ang);

            //if returning and back at the origin, say we are back. 
            if (Owner.Position.DistanceTo(ReturnPosition) <= 1)
            {
                Returning = false;
                CurrentState = MovementState.Stand;
                OnReturnFinished?.Invoke();
            }
        }
        public override string ToString() => $"Return to {ReturnPosition}";
    }
}
