using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Entities;
using IO.Common;
using Engine.Entities.Objects;
using System.Diagnostics;
using System.Collections;
using Engine.Systems;
using Engine.Systems.Range;
using System.Collections.Concurrent;
using IO;

namespace Engine.Maps
{
    /// <summary>
    /// Contains all entities (such as units, doodads and effects) currently in the game.  
    /// </summary>
    public class MapSystem : GameSystem
    {
        //contains objects keyed by their location
        internal readonly ObjectMap Map = new ObjectMap();


        //all objects keyed by their guid
        readonly ConcurrentDictionary<uint, GameObject> objectsGuidTable = new ConcurrentDictionary<uint, GameObject>();


        internal event Action<GameObject> ObjectAdded;


        internal MapSystem()
        {
        }


        public void Add(GameObject obj)
        {
            var guidAdded = objectsGuidTable.TryAdd(obj.Id, obj);
            if (!guidAdded)
                throw new InvalidOperationException("An unit with the same GUID already exists on the map!");

            Map.Add(obj);
            ObjectAdded?.Invoke(obj);
        }


        public GameObject GetByGuid(uint guid)
        {
            return objectsGuidTable.TryGet(guid) as GameObject;
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<Unit> GetUnitsInRect(RectangleF rect, bool aliveOnly = true)
        {
            return Map
                .RangeQuery(rect)
                .OfType<Unit>()
                .Where(u => !(u.IsDead && aliveOnly));
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<GameObject> GetObjectsInRect(RectangleF rect)
        {
            return Map.RangeQuery(rect);
        }

        
        /// <summary>
        /// Returns all units with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <returns>All units within range of the given central point. </returns>
        public IEnumerable<Unit> GetUnitsInRange(Vector pos, double range, bool aliveOnly = true)
        {
            var window = new RectangleF(pos - range, new Vector(range * 2));
            var rangeSq = range * range;

            return GetUnitsInRect(window, aliveOnly)
                .Where(u => u.Position.DistanceToSquared(pos) <= rangeSq);
        }

        public IEnumerable<GameObject> GetObjectsInRange(Vector pos, double range)
        {
            //get a rectangle around the query region. 
            var window = new RectangleF(pos - range, new Vector(range * 2));
            var rangeSq = range * range;

            return GetObjectsInRect(window)
                .Where(u => u.Position.DistanceToSquared(pos) <= rangeSq);
        }


        /// <summary>
        /// Calls the update function for all objects on the GameMap: units, doodads, special effects. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        internal override void Update(int msElapsed)
        {
            //update units n map
            Map.Update(msElapsed);
        }
    }
}
