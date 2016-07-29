using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace Shanism.Engine.Objects.Behaviours
{
    class BehaviourList : Behaviour, IEnumerable<Behaviour>
    {
        protected List<Behaviour> Behaviours = new List<Behaviour>();

        public Behaviour CurrentBehaviour { get; protected set; }

        public BehaviourList(Unit u)
            : base(u)
        {

        }

        public override bool TakeControl()
        {
            CurrentBehaviour = AskForControl();
            return (CurrentBehaviour != null);
        }

        public void Add(Behaviour b)
        {
            Behaviours.Add(b);
        }

        public void AddRange(IEnumerable<Behaviour> b)
        {
            Behaviours.AddRange(b);
        }

        public override void Update(int msElapsed)
        {
            Debug.Assert(CurrentBehaviour != null);     //shouldn't be here

            CurrentBehaviour.Update(msElapsed);
            CurrentOrder = CurrentBehaviour.CurrentOrder;
        }

        protected Behaviour AskForControl()
        {
            foreach (var b in Behaviours)
                if (b.TakeControl())
                    return b;
            return null;
        }

        public IEnumerator<Behaviour> GetEnumerator() 
            => Behaviours.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => Behaviours.GetEnumerator();

        public override string ToString() 
            => CurrentBehaviour?.ToString() ?? "<none>";
    }
}
