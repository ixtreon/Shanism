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

        /// <summary>
        /// Applies (an instance of) the given state to the unit. 
        /// </summary>
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

        /// <summary>
        /// Removes one or all instances of the given UnitState. 
        /// </summary>
        /// <param name="state">The UnitState to remove. </param>
        /// <param name="purge">Whether to remove all instances of the state. </param>
        public void RemoveState(UnitState state, bool purge = false)
        {
            //get n of stacks
            int stacks = 1;
            if (state.IsStacking())
                StateStacks.TryGetValue(state, out stacks);

            if (stacks == 0)
                return;

            //update the # stacks
            stacks--;

            //if there is at least one more instance, we done
            if (stacks > 0 && !purge)
            {
                StateStacks[state] = stacks;
                return;
            }

            //remove the state flag, dictionary entry
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
