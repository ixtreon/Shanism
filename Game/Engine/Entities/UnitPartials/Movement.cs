using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {

        /// <summary>
        /// Gets whether this unit can walk on any terrain 
        /// without collision. 
        /// </summary>
        public bool CanFly => (MovementType & MovementFlags.All) != 0;

        /// <summary>
        /// Gets whether this unit can walk on water. 
        /// </summary>
        public bool CanSwim => (MovementType & MovementFlags.Water) != 0;

        /// <summary>
        /// Gets whether this unit can walk on non-water terrain. 
        /// </summary>
        public bool CanWalk => (MovementType & MovementFlags.Ground) != 0;

        /// <summary>
        /// Gets or sets the kind of terrain this unit can walk on,
        /// considering it can move at all.
        /// </summary>
        public MovementFlags MovementType { get; set; }

        /// <summary>
        /// Gets or sets whether this unit can move at all. 
        /// Setting this value to false is the same as setting 
        /// </summary>
        public bool CanMove => MovementType != 0;
    }

    [Flags]
    public enum MovementFlags : byte
    {
        None = 0,

        Ground = 1 << 0,
        Water = 1 << 1,
        Unpassable = 1 << 2,

        All = Ground | Water | Unpassable,
    }
}
