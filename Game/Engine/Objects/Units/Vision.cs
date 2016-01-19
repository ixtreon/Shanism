using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Events;
using System.Diagnostics;
using Engine.Systems.RangeEvents;
using IO.Util;

namespace Engine.Objects
{
    partial class Unit
    {


        double _visionRange;

        ConcurrentSet<GameObject> visibleObjects { get; } = new ConcurrentSet<GameObject>();


        /// <summary>
        /// Gets or sets the RangeEvent that is fired whenever an object approaches this unit. 
        /// </summary>
        internal ObjectRangeEvent ObjectVisionRangeEvent { get; set; }


        /// <summary>
        /// The event raised whenever a game object enters this unit's vision range. 
        /// </summary>
        public event Action<GameObject> ObjectSeen;

        /// <summary>
        /// The event raised whenever a game object leaves this unit's vision range. 
        /// </summary>
        public event Action<GameObject> ObjectUnseen;

        /// <summary>
        /// The event raised whenever this unit's vision range is changed. 
        /// </summary>
        public event Action<Unit> VisionRangeChanged;


        /// <summary>
        /// Gets all units this guy can see. 
        /// </summary>
        public IEnumerable<GameObject> VisibleObjects
        {
            get { return visibleObjects; }
        }

        /// <summary>
        /// Gets or sets the vision range of the unit. 
        /// </summary>
        public double VisionRange
        {
            get
            {
                return _visionRange;
            }

            set
            {
                if (!_visionRange.AlmostEqualTo(value, 0.0005))  //should be ok, lel
                {

                    _visionRange = value;

                    VisionRangeChanged?.Invoke(this);
                }
            }
        }


        internal void AddObjectInVision(GameObject obj)
        {
            if (visibleObjects.TryAdd(obj))
            {
                ObjectSeen?.Invoke(obj);
                obj.SeenBy.TryAdd(this);
            }
        }

        /// <summary>
        /// Updates the list of visible objects. 
        /// </summary>
        void updateVision(int msElapsed)
        {
            //re-evaluate visible objects. 
            var toRemove = visibleObjects.Where(o => !doVisionCheck(o)).ToArray();
            foreach (var obj in toRemove)
            {
                visibleObjects.TryRemove(obj);
                obj.SeenBy.TryRemove(this);
                ObjectUnseen?.Invoke(obj);
            }
        }

        /// <summary>
        /// Gets whether the specified object is visible by us. 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool IsInVisionRange(GameObject o)
        {
            return visibleObjects.Contains(o);
        }

        bool doVisionCheck(GameObject o)
        {
            //not destroyed
            if (o.IsDestroyed)
                return false;

            //inside our vision range
            if (Position.DistanceTo(o.Position) > VisionRange)
                return false;

            //TODO:
            //is it behind an object?

            return true;
        }
    }
}
