using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// Indicates the way a buff stacks on its target in the context of a single caster. 
    /// </summary>
    public enum BuffStackType : byte
    {
        /// <summary>
        /// A buff that has a number of stacks that expire independently. 
        /// </summary>
        Normal,

        /// <summary>
        /// A buff that has a number of stacks where each application resets existings stacks. 
        /// </summary>
        Refresh,

    }
}
