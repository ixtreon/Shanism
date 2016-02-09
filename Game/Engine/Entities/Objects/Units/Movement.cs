using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    partial class Unit
    {
        /// <summary>
        /// Gets the current movement speed of the unit. 
        /// </summary>
        public double MoveSpeed { get; protected internal set; }

        /// <summary>
        /// Gets the walking speed of the unit, which is the running speed divided by 3. 
        /// </summary>
        public double WalkSpeed { get { return MoveSpeed / 3; } }

        /// <summary>
        /// Gets whether this guy can walk on any terrain 
        /// without collision. 
        /// </summary>
        public bool CanFly { get; set; } = false;

        /// <summary>
        /// Gets whether this guy can walk on water. 
        /// </summary>
        public bool CanSwim { get; set; } = false;

        /// <summary>
        /// Gets whether this guy can walk on non-water terrain. 
        /// </summary>
        public bool CanWalk { get; set; } = true;
    }
}
