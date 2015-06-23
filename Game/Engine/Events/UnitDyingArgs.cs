using Engine.Objects;
using Engine.Objects.Game;
using System;
using System.Linq;

namespace Engine.Events
{
    public class UnitDyingArgs : EventArgs
    {
        public readonly Unit DyingUnit;

        public readonly Unit KillingUnit;

        public bool IsSuicide
        {
            get { return KillingUnit != DyingUnit; }
        }

        /// <summary>
        /// Creates the arguments for the given unit killing itself. 
        /// </summary>
        /// <param name="dyingUnit"></param>
        public UnitDyingArgs(Unit dyingUnit)
            : this(dyingUnit, dyingUnit)
        { }

        public UnitDyingArgs(Unit dyingUnit, Unit killerUnit)
        {
            DyingUnit = dyingUnit;
            KillingUnit = killerUnit;
        }

        /// <summary>
        /// Creates a new UnitDyingArgs instance from the provided UnitDamageArgs argument. 
        /// 
        /// To be used in cases where a unit damages another and the amount proves lethal. 
        /// </summary>
        /// <param name="args"></param>
        public UnitDyingArgs(UnitDamagedArgs args)
            : this(args.DamagedUnit, args.DamagingUnit)
        { }
    }
}
