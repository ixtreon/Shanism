using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Objects.Buffs
{
    /// <summary>
    /// A buff that stuns its target. By default lasts infinitely. 
    /// </summary>
    public class StunnedBuff : Buff
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StunnedBuff"/> class.
        /// </summary>
        public StunnedBuff(int msDuration = 0)
        {
            Name = "Stunned";
            Description = "This unit is stunned. It cannot take any actions. ";

            MaxStacks = 0;
            FullDuration = msDuration;

            UnitStates = UnitFlags.Stunned;
        }
    }
}
