using Engine.Objects;
using Engine.Systems.Abilities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Events
{
    /// <summary>
    /// The arguments passed whenever an ability is being cast. 
    /// </summary>
    public class AbilityCastArgs
    {

        /// <summary>
        /// Gets the type of target this ability was cast on. 
        /// </summary>
        public AbilityTargetType TargetType { get; }

        /// <summary>
        /// Gets the unit that cast this ability. 
        /// </summary>
        public Unit CastingUnit { get; }

        /// <summary>
        /// Gets the unit this ability targeted, if there was one. 
        /// </summary>
        public Unit TargetUnit { get; }

        /// <summary>
        /// Gets the location this spell targeted. If a unit was targeted, returns its position at the time. 
        /// </summary>
        public Vector TargetLocation { get; }

        /// <summary>
        /// Gets or sets whether the spell cast was successful. True by default. 
        /// If set to false, the ability will not be cast at all. 
        /// </summary>
        public bool Success { get; set; } = true;


        public AbilityCastArgs(Ability a, Unit caster, object target)
        {
            CastingUnit = caster;

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
            {
                TargetType = AbilityTargetType.NoTarget;
                TargetUnit = caster;
                TargetLocation = TargetUnit.Position;
            }
        }
    }

}
