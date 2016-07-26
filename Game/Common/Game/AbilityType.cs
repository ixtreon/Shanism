using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Game
{
    /// <summary>
    /// Specifies the type of an ability.
    /// 
    /// A type of <seealso cref="byte"/>. 
    /// </summary>
    [Flags]
    public enum AbilityTargetType : byte
    {
        /// <summary>
        /// A passive ability.  
        /// </summary>
        Passive = 0,

        /// <summary>
        /// An instant-cast ability that requires no target. 
        /// </summary>
        NoTarget = 1,

        /// <summary>
        /// An ability which targets a point on the ground. 
        /// </summary>
        PointTarget = 2, 

        /// <summary>
        /// An ability that targets another unit. 
        /// </summary>
        UnitTarget = 4, 

        /// <summary>
        /// An ability that can target a location on the ground, or another unit. 
        /// </summary>
        PointOrUnitTarget = PointTarget | UnitTarget,
    }
}
