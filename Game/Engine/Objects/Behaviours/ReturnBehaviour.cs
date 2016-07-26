using Shanism.Engine.Systems.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;

namespace Shanism.Engine.Objects.Behaviours
{

    class ReturnBehaviour : Behaviour
    {
        public Vector OriginPosition { get; set; }

        public double MaxDistance { get; set; }

        public bool Returning { get; private set; }

        /// <summary>
        /// Fired whenever the unit starts returning. 
        /// </summary>
        public event Action OnReturnStarted;

        public event Action OnReturnFinished;


        public ReturnBehaviour(Behaviour b, Vector origin, double maxDistance) 
            : base(b)
        {
            OriginPosition = origin;
            MaxDistance = maxDistance;
        }


        public override bool TakeControl()
        {
            return Returning || Owner.Position.DistanceTo(OriginPosition) > MaxDistance;
        }


        public override void Update(int msElapsed)
        {
            var returnOrder = new MoveLocation(OriginPosition, 0.05);

            //issue the return order if needed
            if (!Returning)
            {
                CurrentOrder = returnOrder;
                Returning = true;
                OnReturnStarted?.Invoke();
            }

            //if returning and back at the origin, say we are back. 
            if (Owner.Position.DistanceTo(OriginPosition) < 0.1)
            {
                Returning = false;
                CurrentOrder = new Stand();
                OnReturnFinished?.Invoke();
            }
        }
        public override string ToString() => $"Return to {OriginPosition}";
    }
}
