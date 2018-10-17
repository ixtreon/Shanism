using Ix.Math;
using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Models.Systems;
using Shanism.Engine.Systems.Map;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Contains all entities (such as units, doodads and effects) currently in the game.  
    /// </summary>
    class MapSystem : GameSystem, IGameMap
    {
        /// <summary>
        /// The minimum cell size of a QuadTree cell. 
        /// </summary>
        const float TreeMinCellSize = 4;


        public override string Name => "Map";

        readonly CounterF updateCounter = new CounterF(1000 / 30f);

        //containers
        readonly List<Entity> entityList = new List<Entity>();                              // iteration
        readonly EntityQuadTree entityTree;                                              // spatial map
        readonly Dictionary<uint, Entity> entityLookup = new Dictionary<uint, Entity>();    // guid lookup
        readonly HashSet<Unit> unitSet = new HashSet<Unit>();                               // unit lookup & iteration

        List<Entity> treeInvalidateList = new List<Entity>();

        /// <summary>
        /// Gets the current map terrain object.
        /// </summary>
        public ITerrainMap Terrain { get; }

        /// <summary>
        /// Gets all entities on the map.
        /// </summary>
        public IReadOnlyList<Entity> Entities => entityList;

        /// <summary>
        /// Gets all units on the map.
        /// </summary>
        public IReadOnlyCollection<Unit> Units => unitSet;


        /// <summary>
        /// Gets the area this map is defined in.
        /// </summary>
        public Rectangle? Bounds => Terrain.Bounds;

        /// <summary>
        /// Creates a map system for the given scenario.
        /// </summary>
        /// <param name="sc"></param>
        public MapSystem(Scenario sc)
        {
            Terrain = sc.Config.Map.GetTerrainMap();

            // that won't really work for infinite maps now, would it
            var mapSize = (Vector2)sc.Config.Map.Size;
            entityTree = new EntityQuadTree(new RectangleF(Vector2.Zero, mapSize), Constants.Map.ChunkMinSize);
            //create startup entities
            var oc = new ObjectCreator(sc);
            foreach(var e in oc.CreateAllEntities())
                Add(e);
        }

        /// <summary>
        /// Adds the specified entity to the game map.
        /// </summary>
        /// <param name="e">The entity to add.</param>
        /// <exception cref="InvalidOperationException">An entity with the same GUID already exists on the map. </exception>
        public void Add(Entity e)
        {
            if(entityLookup.ContainsKey(e.Id))
                throw new InvalidOperationException("An entity with the same GUID already exists on the map.");

            if(e.IsDestroyed)
                throw new InvalidOperationException("Unable to add a destroyed object to the map.");

            add(e);
        }



        /// <summary>
        /// Tries to get the entity with the specified id. 
        /// Returns null if no such entity exists. 
        /// </summary>
        public Entity GetByGuid(uint guid) => entityLookup.TryGet(guid);


        /// <summary>
        /// Adds all entities with locations within the specified rectangle to the
        /// given collection but will add some entities outside it, too.
        /// </summary>
        /// <param name="query">The rectangle to query for objects.</param>
        /// <param name="collection">The collection to add entities to.</param>
        /// <returns>All entities intersecting with the specified rectangle.</returns>
        public int GetObjectsInRect(RectangleF query, IList<Entity> collection)
        {
            return entityTree.QueryRect(query, collection);
        }

        public int GetObjectsInRange(Ellipse query, IList<Entity> collection)
        {
            return entityTree.QueryEllipse(query, collection);
        }

        /// <summary>
        /// Returns all entities with locations in a given range from the specified position.
        /// </summary>
        /// <param name="query">The ellipse to query for units.</param>
        /// <returns>
        /// All entities within range of the given central point.
        /// </returns>
        public IReadOnlyList<Entity> GetObjectsInRange(Ellipse query)
        {
            var l = new List<Entity>();
            var count = entityTree.QueryEllipse(query, l);
            return l;
        }

        /// <summary>
        /// Returns all entities with locations within the specified rectangle.
        /// </summary>
        /// <param name="query">The rectangle to query for objects.</param>
        /// <returns>All entities intersecting with the specified rectangle.</returns>
        public IReadOnlyList<Entity> GetObjectsInRect(RectangleF query)
        {
            var l = new List<Entity>();
            var count = entityTree.QueryRect(query, l);
            return l;
        }


        /// <summary>
        /// Returns all units with locations within the specified rectangle.
        /// </summary>
        /// <param name="query">The rectangle to return units within.</param>
        /// <param name="aliveOnly">True to only return alive units.</param>
        /// <returns>
        /// All units within the specified rectangle.
        /// </returns>
        public IEnumerable<Unit> GetUnitsInRect(RectangleF query, bool aliveOnly = true)
        {
            var ans = GetObjectsInRect(query);
            for(int i = 0; i < ans.Count; i++)
                if(isUnit(ans[i], aliveOnly))
                    yield return (Unit)ans[i];
        }

        /// <summary>
        /// Returns all units with locations in a given range from the specified origin. 
        /// </summary>
        /// <param name="query">The ellipse to query for units.</param>
        /// <param name="aliveOnly">True to only return alive units.</param>
        /// <returns>
        /// All units within range of the given central point.
        /// </returns>
        public IEnumerable<Unit> GetUnitsInRange(Ellipse query, bool aliveOnly = true)
        {
            var ans = GetObjectsInRange(query);
            for(int i = 0; i < ans.Count; i++)
                if(isUnit(ans[i], aliveOnly))
                    yield return (Unit)ans[i];
        }


        static bool inRange(Entity e, Vector2 origin, float squaredRange)
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
            if(updateCounter.Tick(msElapsed))
            {
                // update tree
                treeInvalidateList.Clear();
                entityTree.Update(out treeInvalidateList);
            }

            for(int i = 0; i < entityList.Count; i++)
            {
                var e = entityList[i];
                if(e.IsDestroyed)
                {
                    entityList.RemoveAtFast(i--);
                    continue;
                }

                e.Update(msElapsed);
            }
        }

        void add(Entity e)
        {
            // tree
            entityTree.Add(e);
            // lookup
            entityLookup.Add(e.Id, e);
            // sets
            entityList.Add(e);
            if(e is Unit u)
                unitSet.Add(u);

            // events
            e.OnSpawned();
            e.Scripts.Run(s => s.OnEntityAdded(e));
        }

        bool remove(Entity e)
            => entityLookup.Remove(e.Id)
            && !(e is Unit u && !unitSet.Remove(u));

    }
}
