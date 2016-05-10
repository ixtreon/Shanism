using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Game
{
    /// <summary>
    /// An enumeration of the possible outcomes of a damage event. 
    /// </summary>
    public enum DamageResult
    {
        /// <summary>
        /// The damage was normally applied to the target. 
        /// </summary>
        Hit,

        /// <summary>
        /// The damage inflicted was greater than the initial damage. 
        /// </summary>
        Crit,

        /// <summary>
        /// No damage was inflicted to the target. 
        /// </summary>
        Dodge,

        /// <summary>
        /// Like dodge but for invulnerable units. And other stuff?
        /// </summary>
        Thunk
    }
}
