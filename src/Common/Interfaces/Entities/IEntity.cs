using Shanism.Common.Objects;
using System.Numerics;

namespace Shanism.Common.Entities
{
    /// <summary>
    /// A base class for all objects that show on the game map. 
    /// </summary>
    public interface IEntity : IGameObject
    {
        /// <summary>
        /// Gets the name of this entity. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the location of the center of this entity. 
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the current model of the entity.
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Gets the in-game orientation of the entity. 
        /// </summary>
        float Orientation { get; }


        /// <summary>
        /// Gets the pathing size of the entity.
        /// </summary>
        float Scale { get; }


        /// <summary>
        /// Gets the current tint color of this entity. 
        /// </summary>
        Color CurrentTint { get; }
    }
}