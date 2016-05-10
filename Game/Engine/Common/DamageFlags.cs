using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Common
{
    /// <summary>
    /// The enumeration of flags specifying extra rules when dealing (receiving) damage. 
    /// </summary>
    [Flags]
    public enum DamageFlags
    {
        /// <summary>
        /// No special rules are applied to this damage instance. 
        /// </summary>
        None = 0,

        /// <summary>
        /// An instance of damage that should not be reflected. 
        /// </summary>
        NoDeflect = 1,

        /// <summary>
        /// Specifies damage instance cannot be dodged. 
        /// </summary>
        NoDodge = 2,

        /// <summary>
        /// A damage instance that cannot deal bonus critical damage. 
        /// </summary>
        NoCrit = 4,
    }
}
