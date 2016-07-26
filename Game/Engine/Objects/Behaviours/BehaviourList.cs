using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
            foreach (var b in Behaviours)
                if (b.TakeControl())
                    return true;
            return false;
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
            // get the active behaviour (if any)
            Behaviour activeBehaviour = null;
            foreach (var b in Behaviours)
                if (b.TakeControl())
                {
                    activeBehaviour = b;
                    break;
                }

            if (activeBehaviour != null)
            {
                activeBehaviour.Update(msElapsed);

                CurrentBehaviour = activeBehaviour;
                CurrentOrder = activeBehaviour.CurrentOrder;
            }
            else
            {
                CurrentBehaviour = null;
                CurrentOrder = null;
            }
        }

        public IEnumerator<Behaviour> GetEnumerator()
        {
            return Behaviours.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Behaviours.GetEnumerator();
        }

        public override string ToString() => CurrentBehaviour?.ToString() ?? "<none>";
    }
}
