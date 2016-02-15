﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Entities;
using Engine.Systems.Range;

namespace Engine.Systems
{
    /// <summary>
    /// </summary>
    class VisionSystem : UnitSystem
    {
        Unit Target { get; }

        public VisionSystem(Unit u)
        {
            Target = u;
            u.VisionRangeChanged += unit_VisionRangeChanged;
            unit_VisionRangeChanged(u);
        }


        internal override void Update(int msElapsed)
        {
            //re-evaluate visible objects. 
            var toRemove = Target.visibleObjects.Where(shouldRemoveObject).ToArray();
            foreach (var obj in toRemove)
            {
                Target.visibleObjects.TryRemove(obj);
                obj.SeenBy.TryRemove(Target);

                Target.OnObjectUnseen(obj);
            }
        }

        void unit_VisionRangeChanged(Unit u)
        {
            //remove old handler
            if (u.ObjectVisionRangeEvent == null || u.RangeEvents.Remove(u.ObjectVisionRangeEvent))
            {
                //add a new handler
                u.ObjectVisionRangeEvent = new UnitRangeEvent(u, u.VisionRange);
                u.ObjectVisionRangeEvent.Triggered += unit_ObjectInRange;

                u.RangeEvents.Add(u.ObjectVisionRangeEvent);
            }
        }

        void unit_ObjectInRange(GameObject obj)
        {
            if (Target.visibleObjects.TryAdd(obj))
            {
                Target.OnObjectSeen(obj);
                obj.SeenBy.TryAdd(Target);
            }
        }

        bool shouldRemoveObject(GameObject o)
        {
            if(o is Entities.Objects.Doodad)
            {
                int life = 42;
            }
            //not destroyed
            if (o.IsDestroyed)
                return true;

            //inside our vision range
            if (Target.Position.DistanceTo(o.Position) > Target.VisionRange)
                return true;

            //TODO: is it behind an object?


            //otherwise visible
            return false;
        }
    }
}