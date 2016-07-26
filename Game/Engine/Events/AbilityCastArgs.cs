using Shanism.Engine.Entities;
using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Engine.Objects.Abilities;

namespace Shanism.Engine.Events
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
        public Entity TargetEntity { get; }

        /// <summary>
        /// Gets the location this spell targeted. If a unit was targeted, returns its position at the time. 
        /// </summary>
        public Vector TargetLocation { get; }

        /// <summary>
        /// Gets or sets whether the spell cast was successful. True by default. 
        /// If set to false, the ability will not be cast at all. 
        /// </summary>
        public bool Success { get; set; } = true;


        internal AbilityCastArgs(Ability a, Unit caster, Unit target)
        {
            CastingUnit = caster;

            TargetType = AbilityTargetType.UnitTarget;
            TargetEntity = target;
            TargetLocation = TargetEntity.Position;
        }

        internal AbilityCastArgs(Unit caster, CastingData cd)
        {
            var a = cd.Ability;
            CastingUnit = caster;

            if (a.TargetType == AbilityTargetType.NoTarget)
            {
                TargetType = AbilityTargetType.NoTarget;
                TargetEntity = caster;
                TargetLocation = caster.Position;
            }
            else if (cd.IsEntityTarget)
            {
                TargetType = AbilityTargetType.UnitTarget;
                TargetEntity = (Entity)cd.Target;
                TargetLocation = TargetEntity.Position;
            }
            else if (cd.IsGroundTarget)
            {
                TargetType = AbilityTargetType.PointTarget;
                TargetLocation = (Vector)cd.Target;
            }
            else
                throw new Exception();
        }
    }

}
