using System.Collections.Generic;
using Shanism.Engine.Objects;
using Shanism.Common;

namespace Shanism.Engine.Maps
{
    /// <summary>
    /// Represents the in-game map in a ShanoRPG scenario. 
    /// </summary>
    public interface IGameMap
    {
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
        /// Returns all entities with locations within the specified rectangle. 
        /// </summary>
        /// <param name="rect">The rectangle to return units within. </param>
        /// <returns>All entities within the specified rectangle. </returns>
        IEnumerable<Entity> GetObjectsInRect(RectangleF rect);

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="rect">The rectangle to return units within. </param>
        /// <param name="aliveOnly">True to only return alive units. </param>
        /// <returns>All units within the specified rectangle. </returns>
        IEnumerable<Unit> GetUnitsInRect(RectangleF rect, bool aliveOnly = true);

        /// <summary>
        /// Returns all entities with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <returns>All entities within range of the given central point. </returns>
        IEnumerable<Entity> GetObjectsInRange(Vector pos, double range);

        /// <summary>
        /// Returns all units with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <param name="aliveOnly">True to only return alive units. </param>
        /// <returns>All units within range of the given central point. </returns>
        IEnumerable<Unit> GetUnitsInRange(Vector pos, double range, bool aliveOnly = true);

        /// <summary>
        /// Executes a raw query which returns all entities inside a given rectangle,
        /// but may also returns entities that are around it. 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        IEnumerable<Entity> RawQuery(RectangleF rect);

    }
}