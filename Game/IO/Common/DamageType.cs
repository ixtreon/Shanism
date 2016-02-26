using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    /// <summary>
    /// The different types of damage that can be dealt. 
    /// </summary>
    public enum DamageType
    {
        /// <summary>
        /// Physical damage gets reduced by the target's defense. 
        /// </summary>
        Physical,

        /// <summary>
        /// Magical damage gets increased by the caster's magic power
        /// and is reduced by the target's magic defense. 
        /// </summary>
        Magical,
    }

}
