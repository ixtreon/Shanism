using Engine.Events;
using Engine.Systems.Orders;
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


        public static event Action<RangeArgs> AnyUnitInVisionRange;

        /// <summary>
        /// The event executed whenever any unit's order is changed. 
        /// </summary>
        public static event Action<Unit, Order> AnyOrderChanged;

        /// <summary>
        /// The event executed whenever the unit's order is changed. 
        /// </summary>
        public event Action<Order> OrderChanged;

        private void RaiseOrderChangedEvent()
        {
            OrderChanged?.Invoke(Order);
            AnyOrderChanged?.Invoke(this, Order);
        }


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

                    unitInRangeHandlerId = Map.RegisterRangeEvent(this, visionRange, Maps.EventType.EntersRange, RaiseUnitInVisionEvent);
                }
            }
        }

        //TODO: add seeing checks?
        public bool IsInVisionRange(GameObject o)
        {
            return (Location.DistanceTo(o.Location) <= VisionRange);
        }


        private void RaiseUnitInVisionEvent(RangeArgs args)
        {
            UnitInVisionRange?.Invoke(args);
            AnyUnitInVisionRange?.Invoke(args);
        }
    }
}
