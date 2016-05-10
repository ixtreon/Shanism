using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Game
{
    /// <summary>
    /// The basic types of orders units can perform. 
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Indicates that a unit has no order. 
        /// </summary>
        Stand,
        /// <summary>
        /// Indicates that a unit is currently holding its position. 
        /// </summary>
        HoldPosition,
        /// <summary>
        /// Indicates that a unit is currently moving. 
        /// </summary>
        Move,
        /// <summary>
        /// Indicates that a unit is currently casting an ability. 
        /// </summary>
        Casting,
    }
}
