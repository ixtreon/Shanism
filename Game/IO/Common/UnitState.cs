using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    /// <summary>
    /// The states that can affect units. 
    /// </summary>
    [Flags]
    public enum UnitState
    {
        None = 0,

        [UnitState(false)]
        Sleeping = 1,

        [UnitState(true)]
        Stunned = 2,

        [UnitState(false)]
        Fleeing = 4,

    }
}
