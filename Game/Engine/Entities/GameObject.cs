using Engine.Entities.Objects;
using Engine.Systems.Range;
using IO;
using IO.Common;
using IO.Content;
using IO.Objects;
using IO.Serialization;
using IO.Util;
using Network.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Entities
{
    /// <summary>
    /// A base class for all entities on the map. 
    /// Currently this includes doodads, units, effects. 
    /// </summary>
    //[ProtoContract]
    //[ProtoInclude(1, typeof(Unit))]
    //[ProtoInclude(2, typeof(Doodad))]
    //[ProtoInclude(3, typeof(Effect))]
    public abstract class GameObject : ScenarioObject, IGameObject
    {
        #region Static
        /// <summary>
        /// Setup custom serialization based on interfaces. 
        /// </summary>
        static GameObject()
        {
            ProtoConverter.Default.AddMappingFromTo<IGameObject, GameObject>();
            ProtoConverter.Default.AddMappingFromTo<IEffect, Effect>();
            ProtoConverter.Default.AddMappingFromTo<IDoodad, Doodad>();
            ProtoConverter.Default.AddMappingFromTo<IUnit, Unit>();
            ProtoConverter.Default.AddMappingFromTo<IHero, Hero>();
        }
        #endregion


        ModelDef _model;

        /// <summary>
        /// The size of the *texture*. 
        /// TODO: make it a Vector. 
        /// </summary>
        double _scale;


        //circulars ...
        protected internal readonly ConcurrentSet<Unit> SeenBy = new ConcurrentSet<Unit>();


        /// <summary>
        /// Gets or setss the name of the game object. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the scale of the object, also the size of its texture. 
        /// The size must be positive and less than <see cref="IO.Constants.Engine.MaximumObjectSize"/>. 
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set
            {
                if (value <= 0 || value > IO.Constants.Engine.MaximumObjectSize)
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and {1} (see {0})!"
                        .F(nameof(IO.Constants.Engine.MaximumObjectSize), IO.Constants.Engine.MaximumObjectSize));

                _scale = value;
            }
        }

        /// <summary>
        /// Gets the in-game rectangle this object occupies (its hitbox). 
        /// </summary>
        public RectangleF Bounds
        {
            get
            {
                var size = Model.HitBox.Size * Scale;
                return new RectangleF(Position - size / 2, size); 
            }
        }

        /// <summary>
        /// Gets the in-game rectangle occupied by this object's texture. 
        /// </summary>
        public RectangleF TextureBounds
        {
            get
            {
                var texPos = Position - (Model.HitBox.Size / 2 + Model.HitBox.X) * Scale;
                return new RectangleF(texPos, new Vector(Scale));
            }
        }

        public abstract bool HasCollision { get; }

        public abstract ObjectType Type { get; }

        /// <summary>
        /// Gets or sets the custom data for this object. 
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// Gets whether this object should be removed from the map.
        /// </summary>
        internal bool IsDestroyed { get; set; }

        /// <summary>
        /// Gets or sets the model of this object. 
        /// </summary>
        public string ModelName { get; set; } = IO.Constants.Content.DefaultValues.ModelName;

        /// <summary>
        /// Gets the current animation of the model.
        /// </summary>
        public string AnimationName { get; internal set; } = IO.Constants.Content.DefaultValues.Animation;

        /// <summary>
        /// Gets the current model of the object as specified by <see cref="ModelName"/>. 
        /// 
        /// Returns null if <see cref="ModelName"/> is null or the model does not exist. 
        /// </summary>
        public ModelDef Model
        {
            get
            {
                if (_model?.Name != ModelName)
                    _model = Scenario?.Content.Models
                        .FirstOrDefault(m => m.Name == ModelName)
                        ?? ModelDef.Default;
                return _model;
            }
        }

        /// <summary>
        /// Gets the current <see cref="Model"/> animation of the object as specified by <see cref="AnimationName"/>. 
        /// <para/>
        /// Returns null if <see cref="Model"/> is null or if the animation 
        /// specified by <see cref="AnimationName"/> is not defined for the current model. 
        /// </summary>
        public AnimationDef Animation
        {
            get { return Model?.Animations.TryGet(AnimationName); }
        }


        /// <summary>
        /// Gets or sets the location of the center of this game object. 
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets whether the object was moved by magix this turn. 
        /// </summary>
        internal bool HasCustomPosition { get; private set; }




        protected GameObject()
        {
            Name = "Dummy Unit";
            ModelName = IO.Constants.Content.DefaultValues.ModelName;
            Scale = 0.4;
        }

        protected GameObject(
            string name = null,
            Vector? position = null, 
            string modelName = null,
            double? scale = null)
        {
            Name = name ?? "Dummy Unit";
            Position = position ?? Vector.Zero;
            ModelName = modelName ?? IO.Constants.Content.DefaultValues.ModelName;
            Scale = scale ?? 0.4;
        }

        protected GameObject(GameObject proto)
        {
            Name = proto.Name;
            Position = proto.Position;
            ModelName = proto.ModelName;
            Scale = proto.Scale;
        }

        public void SetAnimation(string anim)
        {
            if (string.IsNullOrWhiteSpace(anim))
                AnimationName = IO.Constants.Content.DefaultValues.Animation;
            else
                AnimationName = anim;
        }

        /// <summary>
        /// Resets this object's current animation to the default one. 
        /// </summary>
        public void ResetAnimation()
        {
            AnimationName = IO.Constants.Content.DefaultValues.Animation;
        }


        /// <summary>
        /// Marks this GameObject for destruction, eventually removing it from the game. 
        /// </summary>
        public virtual void Destroy()
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Trying to destroy an object twice!");

            IsDestroyed = true;
        }

        internal override void Update(int msElapsed)
        {
        }


        public override int GetHashCode()
        {
            return (int)Id;
        }


        public override bool Equals(object obj)
        {
            if (!(obj is GameObject))
                return false;
            return ((GameObject)obj).Id == Id;
        }

        public override string ToString()
        {
            return "{0} #{1}".F(Type.ToString(), Id);
        }


        public static implicit operator ObjectStub(GameObject obj)
        {
            return new ObjectStub();
        }

        public static implicit operator GameObject(ObjectStub obj)
        {
            return new Doodad();
        }
    }
}
