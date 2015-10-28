using Engine.Objects;
using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Events
{
    /// <summary>
    /// The arguments passed whenever an ability is being casted. 
    /// </summary>
    public class AbilityCastArgs
    {

        /// <summary>
        /// Gets the type of target this ability was cast on. 
        /// </summary>
        public readonly AbilityTargetType TargetType;

        /// <summary>
        /// Gets the unit this ability targeted, if there was one. 
        /// </summary>
        public Unit TargetUnit { get; private set; }

        /// <summary>
        /// Gets the location this spell targeted. If a unit was targeted, returns its position at the time. 
        /// </summary>
        public Vector TargetLocation { get; private set; }

        /// <summary>
        /// Gets or sets whether the spell cast was successful. True by default. 
        /// If set to false, the ability will not be cast at all. 
        /// </summary>
        public bool Success { get; set; } = true;


        public AbilityCastArgs(Ability a, object target)
        {
            if(target is Unit)
            {
                TargetType = AbilityTargetType.UnitTarget;
                TargetUnit = (Unit)target;
                TargetLocation = TargetUnit.Position;
            }
            else if (target is Vector)
            {
                TargetType = AbilityTargetType.PointTarget;
                TargetLocation = (Vector)target;
            }
            else
                TargetType = AbilityTargetType.NoTarget;
        }
    }

}
