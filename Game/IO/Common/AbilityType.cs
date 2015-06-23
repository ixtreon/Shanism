using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Common
{
    /// <summary>
    /// Specifies the type of an ability. 
    /// </summary>
    public enum AbilityTargetType
    {
        /// <summary>
        /// An ability which targets a point on the ground. 
        /// </summary>
        PointTarget, 

        /// <summary>
        /// An ability that targets another unit. 
        /// </summary>
        UnitTarget, 

        /// <summary>
        /// An ability that can target a location on the ground, or another unit. 
        /// </summary>
        PointOrUnitTarget,

        /// <summary>
        /// An instant-cast ability. 
        /// </summary>
        NoTarget
    }
}
