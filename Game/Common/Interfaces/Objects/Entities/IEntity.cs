using Shanism.Common.Game;
using Shanism.Common.Util;
using Shanism.Common.Interfaces.Engine;
using ProtoBuf;
using System.Collections.Generic;

namespace Shanism.Common.Objects
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
        /// Gets the base animation name of the object.
        /// </summary>
        string AnimationName { get; }


        /// <summary>
        /// Gets or sets the animation suffix of this entity. 
        /// The resulting animation may not actually be present on the client. 
        /// </summary>
        string AnimationSuffix { get; }

        /// <summary>
        /// Gets the orientation of the object's animation. 
        /// </summary>
        double Orientation { get; }

        /// <summary>
        /// Gets the scale of this entity, also the size of its texture. 
        /// The size is always positive and no larger than <see cref="Common.Constants.Engine.MaximumObjectSize"/>. 
        /// </summary>
        double Scale { get; }


        /// <summary>
        /// Gets the current tint color of this entity. 
        /// </summary>
        ShanoColor CurrentTint { get; }
    }
}