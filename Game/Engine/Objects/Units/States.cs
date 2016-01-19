using IO;
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
        /// Applies a single instance of the given state to the unit. 
        /// </summary>
        public void ApplyState(UnitState state)
        {
            var stacks = StateStacks.TryGet(state);

            stacks++;
            StateStacks[state] = stacks;
            StateFlags |= state;

            Console.WriteLine("Added state {0} from unit {1}", state, this);
        }

        /// <summary>
        /// Removes one or all instances of the given UnitState. 
        /// </summary>
        /// <param name="state">The UnitState to remove. </param>
        /// <param name="purgeAll">Whether to remove all instances of the state. </param>
        public void RemoveState(UnitState state, bool purgeAll = false)
        {
            //get n of stacks
            var stacks = StateStacks.TryGet(state);

            if (stacks == 0)
                return;

            //if no purge, reduce stacks, then mb short-exit
            if (!purgeAll)
            {
                stacks--;

                //if there is at least one more instance we done
                if (stacks > 0)
                {
                    StateStacks[state] = stacks;
                    return;
                }
            }

            //remove the state flag, dictionary entry
            StateFlags &= (~state);
            StateStacks.Remove(state);
            Console.WriteLine("Removed state {0} from unit {1}", state, this);
        }


        public bool HasState(UnitState state)
        {
            return StateFlags.HasFlag(state);
        }
    }
}
