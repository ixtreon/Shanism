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

namespace Engine.Maps
{
    /// <summary>
    /// Keeps track of entities of type <see cref="GameObject"/>
    /// </summary>
    public partial class EntityMap
    {
        //contains objects keyed by their location
        readonly ObjectMap<GameObject> map;

        //contains objects keyed by their guid
        readonly Hashtable objectsGuidTable;

        public IEnumerable<GameObject> Objects
        {
            get { return map; }
        }

        public EntityMap()
        {
            objectsGuidTable = new Hashtable();
            map = new ObjectMap<GameObject>();
            map.ObjectUpdate += onObjectUpdate;
        }

        public void Add(GameObject obj)
        {
            objectsGuidTable.Add(obj.Guid, obj);
            map.Add(obj);
        }

        public GameObject GetByGuid(int guid)
        {
            var obj = objectsGuidTable[guid];
            return (GameObject)obj;
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
            var windowSize = new Vector(range) * 2;

            //pick the units in this rectangle. 
            var us = GetUnitsInRect(windowPos, windowSize, aliveOnly);

            //get exactly the units within the circle. 
            var rsq = range * range;
            return us.Where(u => u.Position.DistanceToSquared(pos) <= rsq);
        }


        /// <summary>
        /// Calls the update function for all objects on the GameMap: units, doodads, special effects. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        internal void Update(int msElapsed)
        {
            var objs = Objects.ToArray();

            //shouldn't have destroyed units inside the map. 
            Debug.Assert(!objs.Any(o => o.IsDestroyed));

            //state update pass
            map.Update(msElapsed);
        }


        //checks the range constraints for this object.
        //executed every frame thanks to the ObjectMap 
        void onObjectUpdate(GameObject obj)
        {
            checkRangeConstraints(obj);
        }
    }
}
