using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Game
{
    /// <summary>
    /// Indicates the way a buff stacks on its target in the context of a single caster. 
    /// </summary>
    public enum BuffStackType
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
