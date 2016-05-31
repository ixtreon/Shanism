using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.Common.Objects;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        double _scale = Constants.Units.DefaultUnitSize;

        
        /// <summary>
        /// Gets the list of units that see this entity. 
        /// </summary>
        protected internal readonly ConcurrentSet<Unit> seenByUnits = new ConcurrentSet<Unit>();



        /// <summary>
        /// Gets or sets the name of this entity. 
        /// </summary>
        public string Name { get; set; } = "Dummy Unit";

        /// <summary>
        /// Gets or sets the scale of this entity, also the size of its texture. 
        /// The size must be positive and less than <see cref="IO.Constants.Engine.MaximumObjectSize"/>. 
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value.Clamp(0, Constants.Units.MaximumObjectSize);
            }
        }

        /// <summary>
        /// Gets the orientation of the object. 
        /// </summary>
        public double Orientation { get; set; }

        /// <summary>
        /// Gets a value indicating whether this collides with other entities on the map. 
        /// </summary>
        public abstract bool HasCollision { get; }

        /// <summary>
        /// Gets the object type of this entity.
        /// </summary>
        public abstract override ObjectType ObjectType { get; }
        

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

        /// <summary>
        /// Gets the angle at which this entity is displayed. 
        /// </summary>
        public double Facing { get; set; }

        /// <summary>
        /// Gets or sets the default tint color of this entity. 
        /// </summary>
        public ShanoColor DefaultTint { get; set; } = ShanoColor.White;

        /// <summary>
        /// Gets or sets the current tint color of this entity. 
        /// </summary>
        public ShanoColor CurrentTint { get; set; } = ShanoColor.White;


        /// <summary>
        /// Gets or sets the model, also animation prefix, of this entity. 
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the animation suffix of this entity. 
        /// </summary>
        public string AnimationSuffix { get; set; }

        /// <summary>
        /// Gets the full animation name of the object.
        /// </summary>
        public string AnimationName
            => ModelName + "/" + AnimationSuffix;

        /// <summary>
        /// The event raised whenever the entity is updated by the engine. 
        /// </summary>
        public event Action<Entity, int> Updated;

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
            ModelName = @base.ModelName;
            AnimationSuffix = @base.AnimationSuffix;
            Scale = @base.Scale;
        }

        /// <summary>
        /// Resets this entity's current animation to the default animation name. 
        /// </summary>
        public void ResetAnimation() =>
            AnimationSuffix = Shanism.Common.Constants.Content.DefaultValues.Animation;


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
        /// Raises the <see cref="Updated"/> event. 
        /// </summary>
        internal override void Update(int msElapsed)
        {
            Updated?.Invoke(this, msElapsed);
        }


        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => (int)Id;


        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) =>
            (obj is Entity) && ((Entity)obj).Id == Id;


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"{ObjectType} #{Id}";
    }
}
