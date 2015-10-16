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
        //public event Action<RangeArgs<Unit>> UnitInVisionRange;


        //public static event Action<RangeArgs<Unit>> AnyUnitInVisionRange;
        //handles to the events registered to track units/objects in range
        int unitInRangeHandlerId = -1;
        int objectRangeHandlerId = -1;


        HashSet<GameObject> visibleObjects = new HashSet<GameObject>();

        public event Action<GameObject> ObjectSeen;
        public event Action<GameObject> ObjectUnseen;

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
                if (!visionRange.AlmostEqualTo(value, 0.0005))  //should be ok, lel
                {
                    if (unitInRangeHandlerId != -1)
                    {
                        Map.UnregisterRangeEvent(unitInRangeHandlerId);
                        Map.UnregisterRangeEvent(objectRangeHandlerId);
                    }

                    visionRange = value;

                    objectRangeHandlerId = Map.RegisterAnyObjectInRangeEvent(this, visionRange, Maps.EventType.LeavesOrEnters, raisePlayerObjectVisionEvent);
                }
            }
        }


        //TODO: add visibility checks?
        public bool IsInVisionRange(GameObject o)
        {
            return (Position.DistanceTo(o.Position) <= VisionRange);
        }


        void raisePlayerObjectVisionEvent(RangeArgs<GameObject> args)
        {
            Debug.Assert(this == args.OriginUnit);
            var trigObject = args.TriggerObject;

            //inform the unit
            if (args.EventType == Maps.EventType.EntersRange)
                ObjectSeen?.Invoke(trigObject);
            else
                ObjectUnseen?.Invoke(trigObject);

            //inform its owner 
            Owner.OnObjectVisionRange(args);

            //add to or remove from the list of units that are seen
            if (args.EventType == Maps.EventType.EntersRange)
            {
                visibleObjects.Add(trigObject);
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
