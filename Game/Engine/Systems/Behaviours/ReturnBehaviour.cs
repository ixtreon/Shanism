using Engine.Systems.Orders;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems.Behaviours
{

    class ReturnBehaviour : Behaviour
    {
        public Vector OriginPosition { get; set; }

        public double Distance { get; set; }

        public bool Returning { get; private set; }

        /// <summary>
        /// Fired whenever the unit starts returning. 
        /// </summary>
        public event Action OnReturnStarted;

        public event Action OnReturnFinished;


        public ReturnBehaviour(Behaviour b, Vector origin, double distance) 
            : base(b)
        {
            OriginPosition = origin;
            Distance = distance;
        }


        public override bool TakeControl(int msElapsed)
        {
            var pos = Unit.Location;
            return Returning || pos.DistanceTo(OriginPosition) > Distance;
        }


        public override void Update(int msElapsed)
        {
            var returnOrder = new MoveLocation(OriginPosition);

            //issue the return order if needed
            if (!Returning)
            {
                CurrentOrder = returnOrder;
                Returning = true;
                OnReturnStarted?.Invoke();
            }

            //if returning and back at the origin, say we are back. 
            if (Unit.Location.DistanceTo(OriginPosition) < 0.05)
            {
                Returning = false;
                OnReturnFinished?.Invoke();
            }
        }
    }
}
