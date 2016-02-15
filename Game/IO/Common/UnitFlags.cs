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
    public enum UnitFlags
    {
        None = 0,

        Sleeping = 1 << 0,

        Stunned = 1 << 1,

        Fleeing = 1 << 2,

        Casting = 1 << 3,

        Chanelling = 1 << 4,

        MagicImmune = 1 << 5,

        PhysicalImmune = 1 << 6,

        Invulnerable = MagicImmune | PhysicalImmune,

        NoCollision = 1 << 7,
    }
}
