using Shanism.Common.Util;
using System.Collections.Generic;
using Shanism.Common.Interfaces.Objects;
using System;

namespace Shanism.Common.Interfaces.Entities
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
        /// Gets the location of the center of this entity. 
        /// </summary>
        Vector Position { get; }

        /// <summary>
        /// Gets the current model of the entity.
        /// </summary>
        string Model { get; }


        /// <summary>
        /// Gets or sets the animation of this entity. 
        /// The resulting animation may not be present on the client. 
        /// </summary>
        [Obsolete]
        string Animation { get; }



        /// <summary>
        /// Gets the in-game orientation of the entity. 
        /// </summary>
        float Orientation { get; }

        [Obsolete]
        bool LoopAnimation { get; }

        /// <summary>
        /// Gets the scale of the entity, also the size of its texture. 
        /// This value is always positive. 
        /// </summary>
        double Scale { get; }


        /// <summary>
        /// Gets the current tint color of this entity. 
        /// </summary>
        Color CurrentTint { get; }
    }
}