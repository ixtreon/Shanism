using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Engine.Entities;
using Shanism.Engine.Objects.Range;
using Shanism.Engine.Events;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// </summary>
    class VisionSystem : UnitSystem
    {
        readonly Unit Owner;

        /// <summary>
        /// Gets or sets the RangeEvent that is fired whenever an object approaches this unit. 
        /// </summary>
        RangeEvent ObjectVisionRangeEvent;


        public VisionSystem(Unit u)
        {
            Owner = u;
            u.VisionRangeChanged += ourVisionRangeChanged;
            u.Death += onOurDeath;
            ourVisionRangeChanged(u);
        }

        void onOurDeath(UnitArgs e)
        {
            Owner.unseeAll();
        }
        
        void ourVisionRangeChanged(Unit u)
        {
            //remove old handler
            if (ObjectVisionRangeEvent != null)
            {
                ObjectVisionRangeEvent.Triggered -= onObjectInOurRange;
                Owner.range.RemoveEvent(ObjectVisionRangeEvent);
            }

            //add a new handler
            ObjectVisionRangeEvent = new RangeEvent(u.VisionRange);
            ObjectVisionRangeEvent.Triggered += onObjectInOurRange;
            Owner.range.AddEvent(ObjectVisionRangeEvent);
        }

        void onObjectInOurRange(Entity e, RangeEventTriggerType tty)
        {
            if (tty == RangeEventTriggerType.Enter)
                Owner.see(e);
            else
                Owner.unsee(e);
        }
    }
}
