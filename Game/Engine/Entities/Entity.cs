using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.Common.StubObjects;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Engine
{
    /// <summary>
    /// A base class for all objects that show on the game map. 
    /// Currently this includes effects, doodads, units and heroes (a type of unit). 
    /// </summary>
    public abstract class Entity : GameObject, IEntity
    {
        /// <summary>
        /// The size of the *texture*. 
        /// TODO: make it a Vector. 
        /// </summary>
        double _scale = Constants.Entities.DefaultSize;

        
        /// <summary>
        /// Gets the list of units that see this entity. 
        /// </summary>
        protected internal readonly HashSet<Unit> visibleFromUnits = new HashSet<Unit>();



        /// <summary>
        /// Gets or sets the name of this entity. 
        /// </summary>
        public string Name { get; set; } = "Dummy Unit";


        /// <summary>
        /// Gets or sets the scale of this entity, also the size of its texture. 
        /// The size must be positive and less than <see cref="Constants.Entities.MaxSize"/>. 
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value.Clamp(0.1, Constants.Entities.MaxSize);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collides with other entities on the map. 
        /// </summary>
        public abstract bool HasCollision { get; }

        /// <summary>
        /// Gets the object type of this entity.
        /// </summary>
        public abstract override ObjectType ObjectType { get; }


        /// <summary>
        /// Gets the orientation of the object. 
        /// </summary>
        public double Orientation { get; set; }

        /// <summary>
        /// Gets or sets the custom data for this entity. 
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// Gets whether this entity should be removed from the map as soon as possible. 
        /// </summary>
        internal bool IsDestroyed { get; set; }

        /// <summary>
        /// Gets or sets the location of the center of this game object. 
        /// </summary>
        public Vector Position { get; set; }

        internal Vector MapPosition { get; set; }
        /// <summary>
        /// Gets the angle at which this entity is displayed. 
        /// </summary>
        public double Facing { get; set; }

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
        /// Gets or sets the animation suffix of this entity. 
        /// The resulting animation may not actually be present on the client. 
        /// </summary>
        public string Animation { get; private set; }

        public bool LoopAnimation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity() { }


        /// <summary>
        /// Creates a new <see cref="Entity"/> that is a clone of the given <see cref="Entity"/>. 
        /// </summary>
        /// <param name="base">The entity that is to be cloned. </param>
        protected Entity(Entity @base)
        {
            Name = @base.Name;
            Position = @base.Position;
            Model = @base.Model;
            Animation = @base.Animation;
            Scale = @base.Scale;
        }

        /// <summary>
        /// Resets this entity's current animation suffix to the default animation name. 
        /// </summary>
        public void ResetAnimation()
        {
            Animation = Shanism.Common.Constants.Content.DefaultValues.Animation;
            LoopAnimation = true;
        }

        public void PlayAnimation(string animation, bool loop)
        {
            Animation = animation;
            LoopAnimation = loop;
        }

        /// <summary>
        /// Marks this GameObject for destruction, eventually removing it from the game. 
        /// </summary>
        public virtual void Destroy()
        {
            if (IsDestroyed)
            {
                Console.WriteLine("Trying to destroy an object twice!");
                return;
            }

            IsDestroyed = true;
        }

        /// <summary>
        /// Calls the <see cref="OnUpdate(int)"/> method. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal override void Update(int msElapsed)
        {
            OnUpdate(msElapsed);
        }

        /// <summary>
        /// Override to implement custom update functionality. 
        /// </summary>
        public virtual void OnUpdate(int msElapsed) { }

        public virtual void OnSpawned() { }
    }
}
