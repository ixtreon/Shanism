using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using System;
using System.Linq;

namespace Shanism.Engine.Events
{
    /// <summary>
    /// The arguments passed whenever a unit dies. 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class UnitDyingArgs : EventArgs
    {
        /// <summary>
        /// The unit that is dying. 
        /// </summary>
        public readonly Unit DyingUnit;

        /// <summary>
        /// The killing unit, if any. 
        /// </summary>
        public readonly Unit KillingUnit;

        /// <summary>
        /// Gets whether the unit killed itself (no killer). 
        /// </summary>
        public bool IsSuicide => (KillingUnit != DyingUnit);

        /// <summary>
        /// Creates the arguments for the given unit killing itself. 
        /// </summary>
        /// <param name="dyingUnit"></param>
        public UnitDyingArgs(Unit dyingUnit)
            : this(dyingUnit, dyingUnit)
        { }

        /// <summary>
        /// Creates the arguments for one unit killing the other. 
        /// </summary>
        /// <param name="dyingUnit">The dying unit.</param>
        /// <param name="killerUnit">The killer unit.</param>
        public UnitDyingArgs(Unit dyingUnit, Unit killerUnit)
        {
            DyingUnit = dyingUnit;
            KillingUnit = killerUnit;
        }

        /// <summary>
        /// Creates a new UnitDyingArgs instance from the provided UnitDamageArgs argument. 
        /// To be used in cases where a unit damages another and the amount proves lethal. 
        /// </summary>
        /// <param name="args"></param>
        public UnitDyingArgs(UnitDamagedArgs args)
            : this(args.DamagedUnit, args.DamagingUnit)
        { }
    }
}
