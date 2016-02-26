using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects.Buffs
{
    /// <summary>
    /// A buff that stuns its target for indefinite duration. 
    /// </summary>
    public class StunnedBuff : Buff
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="StunnedBuff"/> class.
        /// </summary>
        public StunnedBuff()
        {
            Name = "Stunned";
            Description = "This unit is stunned. It cannot take any actions. ";
            Type = BuffType.NonStacking;

            UnitStates = UnitFlags.Stunned;
        }
    }
}
