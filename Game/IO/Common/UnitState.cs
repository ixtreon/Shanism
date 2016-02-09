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

        Sleeping = 1,

        Stunned = 2,

        Fleeing = 4,

        Casting = 8,

        Chanelling = 16,
    }
}
