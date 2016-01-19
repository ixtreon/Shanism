using Engine.Systems.RangeEvents;
using IO;
using IO.Common;
using IO.Content;
using IO.Objects;
using IO.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Objects
{
    /// <summary>
    /// A base class for all entities on the map. 
    /// Currently this includes doodads, units, effects. 
    /// </summary>
    public abstract class GameObject : ScenarioObject, IGameObject
    {

        Vector _position;

        ModelDef _model;

        /// <summary>
        /// The size of the *texture*. 
        /// TODO: make it a Vector. 
        /// </summary>
        double _scale;


        //circulars ...
        protected internal readonly ConcurrentSet<Unit> SeenBy = new ConcurrentSet<Unit>();

        internal SortedSet<ObjectRangeEvent> RangeEvents { get; } = new SortedSet<ObjectRangeEvent>();


        /// <summary>
        /// Gets the name of the game object. 
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

        /// <summary>
        /// Gets the globally unique identifier of the object. 
        /// </summary>
        public uint Guid { get; private set; }

        /// <summary>
        /// Gets or sets the custom data for this object. 
        /// </summary>
        public dynamic Data { get; set; }

        public abstract ObjectType ObjectType { get; }

        /// <summary>
        /// Gets whether this object should be removed from the map.
        /// </summary>
        internal bool IsDestroyed { get; private set; }

        /// <summary>
        /// Gets or sets the model of this object. 
        /// </summary>
        public string ModelName { get; set; } = IO.Constants.Content.DefaultModelName;

        /// <summary>
        /// Gets the current animation of the model.
        /// </summary>
        public string AnimationName { get; private set; } = IO.Constants.Content.DefaultAnimation;

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
        public Vector Position
        {
            get { return _position; }
            set { setLocation(value, false); }
        }

        /// <summary>
        /// Gets the location of this object in the previous game frame. 
        /// A value of <see cref="double.NaN"/> indicates the object was just added to the map. 
        /// </summary>
        internal Vector OldPosition { get; private set; }

        /// <summary>
        /// Gets whether the object was moved by magix this turn. 
        /// </summary>
        internal bool HasCustomPosition { get; private set; }

        /// <summary>
        /// Gets the list of players who currently see this object. 
        /// </summary>
        //public IEnumerable<Player> SeenByPlayers
        //{
        //    get
        //    {
        //        return SeenBy.Select(u => u.Owner).Distinct();
        //    }
        //}


        protected GameObject()
        {
            this.Name = "Dummy Unit";
            this.Scale = 0.4;
            ModelName = IO.Constants.Content.DefaultModelName;

            Guid = ObjectGuid.GetNew();
            OldPosition = new Vector(double.NaN);
        }

        public GameObject(Vector location)
            : this()
        {
            _position = location;
        }

        public GameObject(GameObject proto)
            : this(proto.Position)
        {
            ModelName = proto.ModelName;
            Scale = proto.Scale;
        }

        public void SetAnimation(string anim)
        {
            if (string.IsNullOrWhiteSpace(anim))
                AnimationName = IO.Constants.Content.DefaultAnimation;
            else
                AnimationName = anim;
        }

        /// <summary>
        /// Resets this object's current animation to the default one. 
        /// </summary>
        public void ResetAnimation()
        {
            AnimationName = IO.Constants.Content.DefaultAnimation;
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

        /// <summary>
        /// Updates the value of <see cref="OldPosition"/> and <see cref="HasCustomPosition"/>. 
        /// </summary>
        internal void UpdatePosition()
        {
            HasCustomPosition = false;
            OldPosition = _position;
        }


        protected internal void setLocation(Vector loc, bool isRegularMove)
        {
            _position = loc;
            HasCustomPosition = !isRegularMove;
        }

        public override int GetHashCode()
        {
            return (int)Guid;
        }


        public override bool Equals(object obj)
        {
            if (!(obj is GameObject))
                return false;
            return ((GameObject)obj).Guid == Guid;
        }
    }
}
