using Shanism.Common;
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
        /// Gets whether this unit can cross water. 
        /// <para/>
        /// See <see cref="MovementType"/> for more details.
        /// </summary>
        public bool CanSwim => movement.Type.CanSwim();

        /// <summary>
        /// Gets whether this unit can cross ground. 
        /// <para/>
        /// See <see cref="MovementType"/> for more details.
        /// </summary>
        public bool CanWalk => movement.Type.CanWalk();

        /// <summary>
        /// Gets whether this unit can move. 
        /// <para/>
        /// See <see cref="MovementType"/> for more details.
        /// </summary>
        public bool CanMove => movement.Type != MovementType.None;

        /// <summary>
        /// Gets or sets the movement type of this unit.
        /// </summary>
        public MovementType MovementType
        {
            get => movement.Type;
            set => movement.Type = value;
        }

    }
}
