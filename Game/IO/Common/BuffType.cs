using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    /// <summary>
    /// Indicates the way a buff stacks on its target in the context of a single caster. 
    /// </summary>
    public enum BuffType
    {
        /// <summary>
        /// The same as <see cref="BuffType.NonStacking"/>. 
        /// </summary>
        Aura,

        /// <summary>
        /// A buff that does not stack but instead refreshes the active instance. 
        /// </summary>
        NonStacking,

        /// <summary>
        /// A buff that has a number of stacks that expire independently. 
        /// </summary>
        StackingNormal,

        /// <summary>
        /// A buff that has a number of stacks where each application resets existings stacks. 
        /// </summary>
        StackingRefresh,

    }
}
