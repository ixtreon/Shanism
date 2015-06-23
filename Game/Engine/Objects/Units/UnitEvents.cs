using Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    partial class Unit
    {
        /// <summary>
        /// The event fired when the unit gets killed by another unit. 
        /// </summary>
        public event Action<UnitDyingArgs> Dying;

        /// <summary>
        /// The event executed right after a unit receives damage, but before it is eventually killed. 
        /// </summary>
        public event Action<UnitDamagedArgs> DamageReceived;

        /// <summary>
        /// The event executed right before a unit deals damage to a target. 
        /// </summary>
        public event Action<UnitDamagingArgs> DamageDealt;

        /// <summary>
        /// The event executed whenever another unit approaches our vision range. 
        /// </summary>
        public event Action<RangeArgs> UnitInVisionRange;

        //unused
        public event Action OnAbilityCast;



        private int unitInRangeHandlerId = -1;


        private double visionRange;

        /// <summary>
        /// Gets or sets the vision range of the unit. 
        /// </summary>
        public double VisionRange
        {
            get
            {
                return visionRange;
            }
            set
            {
                if(visionRange != value)
                {
                    if (unitInRangeHandlerId != -1)
                        this.Map.UnregisterRangeEvent(unitInRangeHandlerId);

                    visionRange = value;

                    unitInRangeHandlerId = Map.RegisterRangeEvent(this, visionRange, Maps.EventType.EntersRange, raiseUnitInVisionEvent);
                }
            }
        }

        //TODO: add seeing checks?
        public bool InVisionRange(GameObject o)
        {
            return (Location.DistanceTo(o.Location) <= VisionRange);
        }


        internal void raiseUnitInVisionEvent(RangeArgs args)
        {
            if (UnitInVisionRange != null)
                UnitInVisionRange(args);
        }
    }
}
