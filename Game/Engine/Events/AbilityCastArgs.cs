using Shanism.Engine.Entities;
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
        readonly CastingData castData;


        /// <summary>
        /// Gets the unit that cast this ability. 
        /// </summary>
        public Unit CastingUnit { get; }

        /// <summary>
        /// Gets or sets whether the spell cast is successful. True by default. 
        /// If set to false, the ability will not be cast at all. 
        /// </summary>
        public bool Success { get; set; } = true;


        /// <summary>
        /// Gets the type of target this ability was cast on. 
        /// </summary>
        public AbilityTargetType TargetType => castData.TargetType;

        /// <summary>
        /// Gets the unit this ability targeted, if there was one. 
        /// </summary>
        public Entity TargetEntity => castData.TargetEntity;

        /// <summary>
        /// Gets the location this spell targeted. If a unit was targeted, returns its position at the time. 
        /// </summary>
        public Vector TargetLocation => castData.TargetLocation;

        /// <summary>
        /// Gets the ability that was cast.
        /// </summary>
        public Ability Ability => castData.Ability;


        internal AbilityCastArgs(Unit caster, CastingData cast)
        {
            CastingUnit = caster;
            castData = cast;
        }
    }

}
