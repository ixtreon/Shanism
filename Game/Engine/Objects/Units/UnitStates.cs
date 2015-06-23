using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    partial class Unit
    {
        public UnitState StateFlags { get; private set; }

        public Dictionary<UnitState, int> StateStacks = new Dictionary<UnitState, int>();

        public void ApplyState(UnitState state)
        {
            int stacks = 0;
            if (state.IsStacking())
                StateStacks.TryGetValue(state, out stacks);

            stacks++;
            StateStacks[state] = stacks;
            StateFlags |= state;
            Console.WriteLine("Added state {0}", state);
        }


        public void RemoveState(UnitState state, bool removeAllStacks = false)
        {
            var canStackState = state.IsStacking();

            //get n of stacks
            int stacks = 1;
            if (canStackState)
            {
                StateStacks.TryGetValue(state, out stacks);
            }

            if (stacks == 0)
                return;

            //update the # stacks
            stacks--;

            //continue only if to remove the state ccompletely
            if (stacks > 0)
            {
                StateStacks[state] = stacks;
                return;
            }

            //remove the flag, dictionary entry
            StateFlags &= (~state);
            StateStacks.Remove(state);
            Console.WriteLine("Removed state {0}", state);
        }


        public bool HasState(UnitState state)
        {
            return StateFlags.HasFlag(state);
        }
    }
}
