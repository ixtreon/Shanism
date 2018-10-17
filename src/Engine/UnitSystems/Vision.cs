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
    /// Keeps a <see cref="RangeEvent"/> object 
    /// which is in sync with its owner's <see cref="Unit.VisionRange"/>. 
    /// <para>
    /// Calls into the appropriate methods of the Unit class which "see" and "unsee" other entities.
    /// </para>
    /// </summary>
    class VisionSystem : UnitSystem
    {
        RangeEvent @event;

        public VisionSystem(Unit owner)
        {
            owner.VisionRangeChanged += ourVisionRangeChanged;
            owner.Death += onOurDeath;

            ourVisionRangeChanged(owner);
        }

        void onOurDeath(UnitArgs e)
        {
            e.TriggerUnit.UnseeAll();
        }

        void ourVisionRangeChanged(Unit owner)
        {
            //remove old handler
            if(@event != null)
            {
                @event.Triggered -= onObjectInOurRange;
                owner.range.RemoveEvent(@event);
            }

            //add a new handler
            @event = new RangeEvent(owner.VisionRange);
            @event.Triggered += onObjectInOurRange;
            owner.range.AddEvent(@event);

            void onObjectInOurRange(Entity e, RangeEventTriggerType tty)
            {
                if(tty == RangeEventTriggerType.Enter)
                    owner.See(e);
                else
                    owner.Unsee(e);
            }
        }
    }
}
