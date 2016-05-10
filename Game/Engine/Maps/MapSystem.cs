using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects;
using Shanism.Common.Game;
using Shanism.Engine.Objects.Entities;
using System.Diagnostics;
using System.Collections;
using Shanism.Engine.Systems;
using Shanism.Engine.Systems.Range;
using System.Collections.Concurrent;
using Shanism.Common;
using Shanism.ScenarioLib;

namespace Shanism.Engine.Maps
{
    /// <summary>
    /// Contains all entities (such as units, doodads and effects) currently in the game.  
    /// </summary>
    class MapSystem : GameSystem, IGameMap
    {
        //contains objects keyed by their location
        internal readonly ObjectMap Map = new ObjectMap();


        //all objects keyed by their guid
        readonly ConcurrentDictionary<uint, Entity> objectsGuidTable = new ConcurrentDictionary<uint, Entity>();


        internal event Action<Entity> ObjectAdded;


        internal MapSystem()
        {

        }


        /// <summary>
        /// Adds the specified object to the game map.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        /// <exception cref="System.InvalidOperationException">An unit with the same GUID already exists on the map. </exception>
        public void Add(Entity obj)
        {
            var guidAdded = objectsGuidTable.TryAdd(obj.Id, obj);
            if (!guidAdded)
                throw new InvalidOperationException("An unit with the same GUID already exists on the map. ");

            Map.Add(obj);
            ObjectAdded?.Invoke(obj);
        }


        /// <summary>
        /// Tries to get the entity with the specified id. 
        /// Returns null if no such entity exists. 
        /// </summary>
        public Entity GetByGuid(uint guid)
        {
            return objectsGuidTable.TryGet(guid) as Entity;
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to return units within.</param>
        /// <param name="aliveOnly">True to only return alive units.</param>
        /// <returns>
        /// All units within the specified rectangle.
        /// </returns>
        public IEnumerable<Unit> GetUnitsInRect(RectangleF rect, bool aliveOnly = true)
        {
            return Map
                .RangeQuery(rect)
                .OfType<Unit>()
                .Where(u => !(u.IsDead && aliveOnly));
        }

        /// <summary>
        /// Returns all entities with locations within the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to return units within.</param>
        /// <returns>
        /// All entities within the specified rectangle.
        /// </returns>
        public IEnumerable<Entity> GetObjectsInRect(RectangleF rect)
        {
            return Map.RangeQuery(rect);
        }


        /// <summary>
        /// Returns all units with locations in a given range from the specified position.
        /// </summary>
        /// <param name="pos">The coordinates of the central point.</param>
        /// <param name="range">The maximum range of a unit from the central point.</param>
        /// <param name="aliveOnly">True to only return alive units.</param>
        /// <returns>
        /// All units within range of the given central point.
        /// </returns>
        public IEnumerable<Unit> GetUnitsInRange(Vector pos, double range, bool aliveOnly = true)
        {
            var window = new RectangleF(pos - range, new Vector(range * 2));
            var rangeSq = range * range;

            return GetUnitsInRect(window, aliveOnly)
                .Where(u => u.Position.DistanceToSquared(pos) <= rangeSq);
        }

        /// <summary>
        /// Returns all entities with locations in a given range from the specified position.
        /// </summary>
        /// <param name="pos">The coordinates of the central point.</param>
        /// <param name="range">The maximum range of a unit from the central point.</param>
        /// <returns>
        /// All entities within range of the given central point.
        /// </returns>
        public IEnumerable<Entity> GetObjectsInRange(Vector pos, double range)
        {
            //get a rectangle around the query region. 
            var window = new RectangleF(pos - range, new Vector(range * 2));
            var rangeSq = range * range;

            return GetObjectsInRect(window)
                .Where(u => u.Position.DistanceToSquared(pos) <= rangeSq);
        }

        /// <summary>
        /// Executes a raw query which returns all entities inside a given rectangle,
        /// but may also returns entities that are around it.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public IEnumerable<Entity> RawQuery(RectangleF rect)
        {
            return Map.RawQuery(rect);
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
