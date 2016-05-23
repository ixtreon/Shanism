using Shanism.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Shanism.Engine.Systems.Behaviours
{
    class BehaviourList : Behaviour, IEnumerable<Behaviour>
    {
        protected List<Behaviour> Behaviours = new List<Behaviour>();

        public Behaviour CurrentBehaviour { get; protected set; }

        public BehaviourList(Unit u)
            : base(u)
        {

        }

        public override bool TakeControl(int msElapsed)
        {
            return Behaviours.Any(b => b.TakeControl(msElapsed));
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
            var newBehaviour = Behaviours.FirstOrDefault(b => b.TakeControl(msElapsed));
            CurrentBehaviour = newBehaviour;

            if (CurrentBehaviour == null)
            {
                CurrentOrder = null;
                return;
            }

            CurrentBehaviour.Update(msElapsed);
            CurrentOrder = CurrentBehaviour.CurrentOrder;
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
