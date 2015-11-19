using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using Engine.Objects.Game;
using System.Diagnostics;
using System.Collections;
using Engine.Systems;
using Engine.Systems.RangeEvents;

namespace Engine.Maps
{
    /// <summary>
    /// Another needless layer of abstraction over the HashMap. 
    /// </summary>
    public partial class EntityMap
    {
        //contains objects keyed by their location
        readonly ObjectMap<GameObject> map;

        //contains objects keyed by their guid
        readonly Hashtable objectsGuidTable;


        internal readonly RangeEventProvider RangeProvider;

        private int currentFrame = -1;

        /// <summary>
        /// Gets all objects in the entity map. 
        /// </summary>
        public IEnumerable<GameObject> Objects
        {
            get { return map; }
        }

        internal EntityMap(RangeEventProvider rangeProvider)
        {
            this.RangeProvider = rangeProvider;

            objectsGuidTable = new Hashtable();

            map = new ObjectMap<GameObject>();
        }

        public void Add(GameObject obj)
        {
            objectsGuidTable.Add(obj.Guid, obj);
            map.Add(obj);
        }

        public GameObject GetByGuid(int guid)
        {
            var obj = objectsGuidTable[guid];
            return obj as GameObject;
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<Unit> GetUnitsInRect(Vector pos, Vector size, bool aliveOnly = true)
        {
            return map
                .RangeQuery(pos, size)
                .OfType<Unit>()
                .Where(u => !(u.IsDead && aliveOnly));
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<GameObject> GetObjectsInRect(Vector pos, Vector size)
        {
            return map.RangeQuery(pos, size);
        }

        
        /// <summary>
        /// Returns all units with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <returns>All units within range of the given central point. </returns>
        public IEnumerable<Unit> GetUnitsInRange(Vector pos, double range, bool aliveOnly = true)
        {
            //get a rectangle around the query region. 
            var windowPos = pos - range;
            var windowSize = new Vector(range * 2);

            //pick the units in this rectangle. 
            var us = GetUnitsInRect(windowPos, windowSize, aliveOnly);

            //get exactly the units within the circle. 
            var rangeSq = range * range;
            return us.Where(u => u.Position.DistanceToSquared(pos) <= rangeSq);
        }


        /// <summary>
        /// Calls the update function for all objects on the GameMap: units, doodads, special effects. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        internal void Update(int msElapsed)
        {
            currentFrame++;


            //state update pass
            map.AddPendingObjects();

            //shouldn't have destroyed units inside the map. 
            var objs = Objects.ToArray();
            Debug.Assert(!objs.Any(o => o.IsDestroyed));

            //update constraints
            //TODO: still passing all objects
            //      rather than nearby ones
            foreach (var obj in objs)
                RangeProvider.CheckAllConstraints(obj, objs, currentFrame);

            //update units n map
            map.Update(msElapsed);
        }
    }
}
