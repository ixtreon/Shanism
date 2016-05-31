using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Entities;
using Shanism.Engine.Systems.Range;
using Shanism.Common.Util;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// </summary>
    class VisionSystem : UnitSystem
    {
        readonly Unit Owner;


        internal readonly ConcurrentSet<Entity> objectsSeen = new ConcurrentSet<Entity>();



        public VisionSystem(Unit u)
        {
            Owner = u;
            u.VisionRangeChanged += unit_VisionRangeChanged;
            unit_VisionRangeChanged(u);
        }


        internal override void Update(int msElapsed)
        {
            //re-evaluate visible objects. 
            var toRemove = objectsSeen.Where(shouldRemoveObject).ToList();
            foreach (var obj in toRemove)
            {
                objectsSeen.TryRemove(obj);
                obj.seenByUnits.TryRemove(Owner);

                Owner.OnObjectUnseen(obj);
            }
        }

        void unit_VisionRangeChanged(Unit u)
        {
            //remove old handler
            if (u.ObjectVisionRangeEvent == null || u.range.events.TryRemove(u.ObjectVisionRangeEvent))
            {
                //add a new handler
                u.ObjectVisionRangeEvent = new RangeEvent(u.VisionRange);
                u.ObjectVisionRangeEvent.Triggered += unit_ObjectInRange;

                u.range.events.TryAdd(u.ObjectVisionRangeEvent);
            }
        }

        void unit_ObjectInRange(Entity obj)
        {
            if (objectsSeen.TryAdd(obj))
            {
                Owner.OnObjectSeen(obj);
                obj.seenByUnits.TryAdd(Owner);
            }
        }

        bool shouldRemoveObject(Entity o)
        {
            //not destroyed
            if (o.IsDestroyed)
                return true;

            //inside our vision range
            if (Owner.Position.DistanceTo(o.Position) > Owner.VisionRange)
                return true;

            //TODO: is it behind an object?


            //otherwise visible
            return false;
        }
    }
}
