using Ix.Math;
using Shanism.Engine.Entities;
using Shanism.ScenarioLib;
using System.Collections.Generic;

namespace Shanism.Engine.Systems
{
    /// <summary>
    /// Represents the in-game map of a ShanoRPG scenario. 
    /// Contains all entities on the map and all its terrain. 
    /// </summary>
    public interface IGameMap
    {
        /// <summary>
        /// Gets the area this map is defined in.         
        /// </summary>
        Rectangle? Bounds { get; }

        /// <summary>
        /// Gets the list of all entities on the map.
        /// </summary>
        IReadOnlyList<Entity> Entities { get; }

        /// <summary>
        /// Gets the collection of all units on the map.
        /// </summary>
        IReadOnlyCollection<Unit> Units { get; }

        /// <summary>
        /// Gets the terrain map of the scenario this object is part of. 
        /// </summary>
        ITerrainMap Terrain { get; }

        /// <summary>
        /// Adds the specified object to the game map. 
        /// </summary>
        /// <param name="obj">The object to add.</param>
        void Add(Entity obj);

        /// <summary>
        /// Tries to get the entity with the specified id. 
        /// Returns null if no such entity exists. 
        /// </summary>
        Entity GetByGuid(uint id);

        /// <summary>
        /// Adds all entities with locations within the specified rectangle to the 
        /// given collection. 
        /// </summary>
        /// <param name="query">The rectangle to query for objects.</param>
        /// <param name="collection">The collection to add entities to.</param>
        int GetObjectsInRect(RectangleF query, IList<Entity> collection);

        int GetObjectsInRange(Ellipse query, IList<Entity> collection);


        /// <summary>
        /// Returns all entities with locations within the specified rectangle. 
        /// </summary>
        /// <param name="query">The rectangle to return units within. </param>
        /// <returns>All entities within the specified rectangle. </returns>
        IReadOnlyList<Entity> GetObjectsInRect(RectangleF query);

        /// <summary>
        /// Returns all entities with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <returns>All entities within range of the given central point. </returns>
        IReadOnlyList<Entity> GetObjectsInRange(Ellipse query);


        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="rect">The rectangle to return units within. </param>
        /// <param name="aliveOnly">True to only return alive units. </param>
        /// <returns>All units within the specified rectangle. </returns>
        IEnumerable<Unit> GetUnitsInRect(RectangleF rect, bool aliveOnly = true);

        /// <summary>
        /// Returns all units with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <param name="aliveOnly">True to only return alive units. </param>
        /// <returns>All units within range of the given central point. </returns>
        IEnumerable<Unit> GetUnitsInRange(Ellipse query, bool aliveOnly = true);


    }
}