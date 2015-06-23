using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    /// <summary>
    /// Unused
    /// </summary>
    [Flags]
    public enum UnitState
    {
        None = 0,

        [UnitState(false)]
        Sleeping = 1 << 0,

        [UnitState(true)]
        Stunned = 1 << 1,

        [UnitState(false)]
        Fleeing = 1 << 2,

    }
}
