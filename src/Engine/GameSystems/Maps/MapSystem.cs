using Shanism.Common;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.ScenarioLib;
using Shanism.Engine.GameSystems.Maps;
using Shanism.Engine.Maps.Terrain;

namespace Shanism.Engine.Maps
{
    /// <summary>
    /// Contains all entities (such as units, doodads and effects) currently in the game.  
    /// </summary>
    class MapSystem : GameSystem, IGameMap
    {
        /// <summary>
        /// The minimum cell size of the QuadTree. 
        /// </summary>
        const double TreeMinCellSize = 4;


        public override string SystemName => "Map";

        //containers
        ITerrainMap terrain;
        QuadTree<Entity> tree;
        Dictionary<uint, Entity> objectsGuidTable;
        HashSet<Unit> units;

        readonly List<Entity> destroyList = new List<Entity>();

        /// <summary>
        /// Gets all entities on the map.
        /// </summary>
        public IEnumerable<Entity> Entities => objectsGuidTable.Values;

        /// <summary>
        /// Gets all units on the map.
        /// </summary>
        public IEnumerable<Unit> Units => units;

        /// <summary>
        /// Gets the terrain of the map.
        /// </summary>
        public ITerrainMap Terrain => terrain;

        /// <summary>
        /// Gets the area this map is defined in.
        /// </summary>
        public Rectangle Bounds => terrain.Bounds;


        internal MapSystem() { }


        /// <summary>
        /// Clears all units from the map and then adds all startup objects 
        /// from the given scenario. 
        /// </summary>
        /// <param name="sc">The scenario to load.</param>
        /// <param name="mapSeed">The random map seed.</param>
        internal void LoadScenario(Scenario sc, int mapSeed)
        {
            var mapSize = (Vector)sc.Config.Map.Size;

            terrain = MapGod.Create(sc.Config.Map, mapSeed);
            tree = new QuadTree<Entity>(mapSize / 2, mapSize / 2, TreeMinCellSize);
            objectsGuidTable = new Dictionary<uint, Entity>();
            units = new HashSet<Unit>();

            //create startup entities
            var oc = new ObjectCreator(sc);
            var entities = oc.CreateAllEntities();
            foreach (var e in entities)
                Add(e);
        }

        /// <summary>
        /// Adds the specified entity to the game map.
        /// </summary>
        /// <param name="e">The entity to add.</param>
        /// <exception cref="System.InvalidOperationException">An entity with the same GUID already exists on the map. </exception>
        public void Add(Entity e)
        {
            if (objectsGuidTable.ContainsKey(e.Id))
                throw new InvalidOperationException("An entity with the same GUID already exists on the map.");

            if (e.IsDestroyed)
                throw new InvalidOperationException("Unable to add a destroyed object to the map.");

            var pos = e.Position;

            tree.Add(e, pos);
            objectsGuidTable[e.Id] = e;
            if (e is Unit)
                units.Add((Unit)e);

            e.MapPosition = pos;
            e.OnSpawned();
            e.Scripts.Run(s => s.OnEntityAdded(e));
        }



        /// <summary>
        /// Tries to get the entity with the specified id. 
        /// Returns null if no such entity exists. 
        /// </summary>
        public Entity GetByGuid(uint guid) => objectsGuidTable.TryGet(guid);


        /// <summary>
        /// Returns all entities with locations within the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to return units within.</param>
        /// <returns>
        /// All entities within the specified rectangle.
        /// </returns>
        public IEnumerable<Entity> GetObjectsInRect(RectangleF rect)
        {
            return tree.Query(rect.Center, rect.Size / 2);
        }

        /// <summary>
        /// Returns all entities with locations within the specified rectangle.
        /// </summary>
        public IEnumerable<Entity> GetObjectsInRect(Vector center, Vector range)
        {
            return tree.Query(center, range);
        }

        /// <summary>
        /// Adds all entities with locations within the specified rectangle to the
        /// given collection.
        /// </summary>
        /// <param name="center">The center of the query rectange.</param>
        /// <param name="range">The range of the query rectangle.</param>
        /// <param name="collection">The collection to add entities to.</param>
        public void GetObjectsInRect(Vector center, Vector range, ICollection<Entity> collection)
        {
            tree.Query(center, range, collection);
        }

        /// <summary>
        /// Returns all entities with locations in a given range from the specified position.
        /// </summary>
        /// <param name="origin">The coordinates of the central point.</param>
        /// <param name="range">The maximum range of a unit from the central point.</param>
        /// <returns>
        /// All entities within range of the given central point.
        /// </returns>
        public IEnumerable<Entity> GetObjectsInRange(Vector origin, double range)
        {
            var rangeSq = range * range;

            foreach (var e in tree.Query(origin, new Vector(range)))
                if (inRange(e, origin, rangeSq))
                    yield return e;
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
            foreach (var e in GetObjectsInRect(rect))
                if (isUnit(e, aliveOnly))
                    yield return (Unit)e;
        }

        /// <summary>
        /// Returns all units with locations in a given range from the specified origin. 
        /// </summary>
        /// <param name="origin">The coordinates of the origin position.</param>
        /// <param name="range">The maximum range of a unit from the origin.</param>
        /// <param name="aliveOnly">True to only return alive units.</param>
        /// <returns>
        /// All units within range of the given central point.
        /// </returns>
        public IEnumerable<Unit> GetUnitsInRange(Vector origin, double range, bool aliveOnly = true)
        {
            foreach (var e in GetObjectsInRange(origin, range))
                if (isUnit(e, aliveOnly))
                    yield return (Unit)e;
        }


        static bool inRange(Entity e, Vector origin, double squaredRange)
            => e.Position.DistanceToSquared(origin) <= squaredRange;

        static bool isUnit(Entity e, bool aliveOnly)
            => e is Unit && (!aliveOnly || !((Unit)e).IsDead);


        /// <summary>
        /// Calls the update function for all entities on the GameMap.
        /// Also updates their positions in the spatial structure. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        internal override void Update(int msElapsed)
        {
            destroyList.Clear();
            foreach (var e in Entities)
            {
                //remove maybe
                if (e.IsDestroyed)
                {
                    destroyList.Add(e);
                    continue;
                }

                //update position & state
                var curPos = e.Position;
                if (!tree.Update(e, e.MapPosition, curPos))
                    throw new Exception("Unit on the map was not found in the tree!");
                e.MapPosition = curPos;
            }

            foreach (var e in destroyList)
            {
                if (!tree.Remove(e, e.MapPosition))
                    throw new Exception("Unit on the map was not found in the tree!");

                if (!objectsGuidTable.Remove(e.Id))
                    throw new Exception("Unit on the map was not found in the id table!");

                if (e is Unit)
                    units.Remove((Unit)e);

                e.Dispose();
            }

        }
    }
}
