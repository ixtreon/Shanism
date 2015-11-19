using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Events;
using System.Diagnostics;
using Engine.Systems.RangeEvents;

namespace Engine.Objects
{
    partial class Unit
    {

        ObjectConstraint objectInRangeConstraint;

        double _visionRange;

        HashSet<GameObject> visibleObjects = new HashSet<GameObject>();

        /// <summary>
        /// The event raised whenever a game object enters this unit's vision range. 
        /// </summary>
        public event Action<GameObject> ObjectSeen;

        /// <summary>
        /// The event raised whenever a game object leaves this unit's vision range. 
        /// </summary>
        public event Action<GameObject> ObjectUnseen;

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
                if (Map != null && !_visionRange.AlmostEqualTo(value, 0.0005))  //should be ok, lel
                {
                    //remove old handler
                    if (objectInRangeConstraint != null)
                        Map.RangeProvider.RemoveConstraint(objectInRangeConstraint);

                    _visionRange = value;

                    //add a new handler
                    objectInRangeConstraint = new ObjectConstraint(this, _visionRange);
                    objectInRangeConstraint.Triggered += onObjectInVisionRange;
                    Map.RangeProvider.AddConstraint(objectInRangeConstraint);
                }
            }
        }

        readonly object _visibleObjectsLock = new object();

        void onObjectInVisionRange(GameObject obj)
        {
            //add the unit to the list
            visibleObjects.Add(obj);
            obj.SeenBy.Add(this);
            ObjectSeen?.Invoke(obj);
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
                visibleObjects.Remove(obj);
                obj.SeenBy.Remove(this);
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
