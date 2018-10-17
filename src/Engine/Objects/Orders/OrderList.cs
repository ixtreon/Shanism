using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace Shanism.Engine.Objects.Orders
{
    class OrderList : Order, IEnumerable<Order>
    {
        protected List<Order> Behaviours { get; } = new List<Order>();

        public Order CurrentBehaviour { get; protected set; }

        public OrderList(Unit u)
            : base(u)
        {

        }

        public override bool TakeControl()
        {
            CurrentBehaviour = AskForControl();
            return (CurrentBehaviour != null);
        }

        public void Add(Order b)
        {
            Behaviours.Add(b);
        }

        public void AddRange(IEnumerable<Order> b)
        {
            Behaviours.AddRange(b);
        }

        public override void Update(int msElapsed)
        {
            Debug.Assert(CurrentBehaviour != null);     //shouldn't be here

            CurrentBehaviour.Update(msElapsed);
            CurrentState = CurrentBehaviour.CurrentState;
        }

        protected Order AskForControl()
        {
            foreach (var b in Behaviours)
                if (b.TakeControl())
                    return b;
            return null;
        }

        public IEnumerator<Order> GetEnumerator() 
            => Behaviours.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => Behaviours.GetEnumerator();

        public override string ToString() 
            => CurrentBehaviour?.ToString() ?? "<none>";
    }
}
