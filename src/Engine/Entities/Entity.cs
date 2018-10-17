using Ix.Math;
using Shanism.Common;
using Shanism.Common.Entities;
using Shanism.Engine.Entities;
using System.Collections.Generic;
using System.Numerics;
using static Shanism.Common.Constants.Entities;

namespace Shanism.Engine
{

    /// <summary>
    /// A base class for all objects that show on the game map. 
    /// Currently this includes effects, doodads, units and heroes. 
    /// </summary>
    public abstract class Entity : GameObject, IEntity
    {

        Vector2 position;
        float scale = DefaultScale;
        RectangleF bounds;

        /// <summary>
        /// Gets the list of units that see this entity. 
        /// </summary>
        protected internal readonly HashSet<Unit> visibleFromUnits = new HashSet<Unit>();



        /// <summary>
        /// Gets or sets the name of this entity. 
        /// </summary>
        public string Name { get; set; } = "Dummy Unit";


        /// <summary>
        /// Gets or sets the location of the center of this game object. 
        /// </summary>
        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                bounds = new RectangleF(position - new Vector2(scale / 2), new Vector2(scale));
            }
        }

        /// <summary>
        /// Gets or sets the scale of this entity, also the bounding box for its pathing rect. 
        /// The size must be positive and less than <see cref="Common.MaxSize"/>. 
        /// </summary>
        public float Scale
        {
            get => scale;
            set
            {
                scale = value.Clamp(MinScale, MaxScale);
                bounds = new RectangleF(position - new Vector2(scale / 2), new Vector2(scale));
            }
        }

        public RectangleF Bounds => bounds;

        /// <summary>
        /// Gets a value indicating whether this collides with other entities on the map. 
        /// </summary>
        public abstract bool HasCollision { get; }

        /// <summary>
        /// Gets the orientation of the object. 
        /// </summary>
        public float Orientation { get; set; }

        /// <summary>
        /// Gets or sets the custom data for this entity. 
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the default tint color of this entity. 
        /// </summary>
        public Color DefaultTint { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the current tint color of this entity. 
        /// </summary>
        public Color CurrentTint { get; set; } = Color.White;


        /// <summary>
        /// Gets or sets the animation used as a model of this entity. 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity() { }

        /// <summary>
        /// Creates a new <see cref="Entity"/> that is a clone of the given <see cref="Entity"/>. 
        /// </summary>
        /// <param name="base">The entity that is to be cloned. </param>
        protected Entity(Entity @base)
            : this()
        {
            Name = @base.Name;
            Position = @base.Position;
            Model = @base.Model;
            Scale = @base.Scale;
        }

        /// <summary>
        /// Updates the entity and calls the <see cref="OnUpdate(int)"/> method. 
        /// </summary>
        internal override void Update(int msElapsed)
        {
            OnUpdate(msElapsed);
        }

        /// <summary>
        /// Override to implement custom update functionality. 
        /// </summary>
        protected virtual void OnUpdate(int msElapsed) { }

        /// <summary>
        /// Override to implement custom functionality on entity creation. 
        /// </summary>
        protected internal virtual void OnSpawned() { }
    }
}
