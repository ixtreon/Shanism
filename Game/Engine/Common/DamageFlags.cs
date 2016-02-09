using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Common
{
    [Flags]
    public enum DamageFlags
    {
        None = 0,

        NoDeflect = 1,

        NoDodge = 2,

        NoCrit = 4,
    }
}
