using IO.Common;
using IO.Content;
using IO.Interfaces.Engine;
using ProtoBuf;
using System.Collections.Generic;

namespace IO.Objects
{
    /// <summary>
    /// A base class for all objects that show on the game map. 
    /// Currently this includes effects, doodads, units and heroes (a type of unit). 
    /// </summary>
    public interface IEntity : IGameObject
    {
        /// <summary>
        /// Gets the name of this entity. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the location of the center of this game object. 
        /// </summary>
        Vector Position { get; }

        /// <summary>
        /// Gets the full animation name of the object.
        /// </summary>
        string AnimationName { get; }

        /// <summary>
        /// Gets the scale of this entity, also the size of its texture. 
        /// The size is always positive and no larger than <see cref="IO.Constants.Engine.MaximumObjectSize"/>. 
        /// </summary>
        double Scale { get; }


        /// <summary>
        /// Gets the current tint color of this entity. 
        /// </summary>
        ShanoColor CurrentTint { get; }
    }
}