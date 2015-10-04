using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Events;
using System.Diagnostics;

namespace Engine.Objects
{
    partial class Unit
    {

        /// <summary>
        /// The event executed whenever another unit approaches our vision range. 
        /// </summary>
        public event Action<RangeArgs<Unit>> UnitInVisionRange;


        public static event Action<RangeArgs<Unit>> AnyUnitInVisionRange;
        //handles to the events registered to track units/objects in range
        int unitInRangeHandlerId = -1;
        int objectRangeHandlerId = -1;


        HashSet<GameObject> visibleObjects = new HashSet<GameObject>();

        /// <summary>
        /// Gets all units this guy can see. 
        /// </summary>
        public IEnumerable<GameObject> VisibleObjects
        {
            get { return visibleObjects; }
        }

        double visionRange;

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
                if (!visionRange.AlmostEqualTo(value, 0.0005))
                {
                    if (unitInRangeHandlerId != -1)
                    {
                        Map.UnregisterRangeEvent(unitInRangeHandlerId);
                        Map.UnregisterRangeEvent(objectRangeHandlerId);
                    }

                    visionRange = value;

                    unitInRangeHandlerId = Map.RegisterAnyUnitInRangeEvent(this, visionRange, Maps.EventType.EntersRange, RaiseUnitInVisionEvent);
                    objectRangeHandlerId = Map.RegisterAnyObjectInRangeEvent(this, visionRange, Maps.EventType.LeavesOrEnters, raisePlayerObjectVisionEvent);
                }
            }
        }


        //TODO: add seeing checks?
        public bool IsInVisionRange(GameObject o)
        {
            return (Position.DistanceTo(o.Position) <= VisionRange);
        }


        void RaiseUnitInVisionEvent(RangeArgs<Unit> args)
        {
            UnitInVisionRange?.Invoke(args);
            AnyUnitInVisionRange?.Invoke(args);

            if (args.OriginUnit.Owner.IsPlayer)
                args.OriginUnit.Owner.raiseUnitInVisionRange(args);
        }

        void raisePlayerObjectVisionEvent(RangeArgs<GameObject> args)
        {
            Debug.Assert(this == args.OriginUnit);

            //someone came in range! 
            Owner.OnObjectVisionRange(args);

            //fix up the list of objects we see
            var trigObject = args.TriggerObject;
            if (args.EventType == Maps.EventType.EntersRange)
            {
                visibleObjects.Add(args.TriggerObject);
                trigObject.SeenBy.Add(this);
            }
            else
            {
                trigObject.SeenBy.Remove(this);
                visibleObjects.Remove(trigObject);
            }
        }
    }
}
