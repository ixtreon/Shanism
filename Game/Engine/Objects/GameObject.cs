using Engine.Maps.Concurrent;
using Engine.Systems.RangeEvents;
using IO;
using IO.Common;
using IO.Content;
using IO.Objects;
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


        Vector _newPosition;
        Vector _position;
        Vector _oldPosition;

        protected internal bool _customPosition;

        //circular reference .
        protected internal readonly HashSet<Unit> SeenBy = new HashSet<Unit>();

        /// <summary>
        /// Gets the list of players who currently see this object. 
        /// </summary>
        public IEnumerable<Player> SeenByPlayers
        {
            get
            {
                return SeenBy.Select(u => u.Owner).Distinct();
            }
        }

        /// <summary>
        /// Gets the name of the game object. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the object. 
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// Gets the globally unique identifier of the object. 
        /// </summary>
        public int Guid { get; private set; }

        /// <summary>
        /// Gets or sets the custom data for this object. 
        /// </summary>
        public dynamic Data { get; set; }

        public abstract ObjectType ObjectType { get; }


        internal SortedSet<ObjectConstraint> RangeConstraints { get; } = new SortedSet<ObjectConstraint>();

        /// <summary>
        /// Gets or sets the location of the game object. 
        /// Changes will not take effect until the next game cycle. 
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
        internal Vector OldPosition { get { return _oldPosition; } }

        /// <summary>
        /// Gets the location where this object will move next game frame. 
        /// </summary>
        internal Vector FuturePosition {  get { return _newPosition; } }

        /// <summary>
        /// Gets whether the object was moved by magix this turn. 
        /// </summary>
        public bool HasCustomPosition {  get { return _customPosition; } }

        /// <summary>
        /// Gets whether this object should be removed from the map.
        /// </summary>
        internal bool IsDestroyed { get; private set; }


        #region Model and Animations
        /// <summary>
        /// Gets the model of the object. 
        /// </summary>
        public string Model { get; }

        public string Animation { get; private set; } = IO.Constants.Content.DefaultAnimation;

        public void SetAnimation(string anim)
        {
            if (string.IsNullOrWhiteSpace(anim))
                Animation = IO.Constants.Content.DefaultAnimation;
            else
                Animation = anim;
        }

        /// <summary>
        /// Resets this object's current animation to the default one. 
        /// </summary>
        public void ResetAnimation()
        {
            Animation = IO.Constants.Content.DefaultAnimation;
        }
        #endregion


        protected GameObject()
        {
            this.Size = 0.4;
            this.Model = IO.Constants.Content.DefaultModel;
            Guid = ObjectGuid.GetNew();
            _oldPosition = new Vector(double.NaN);
        }

        public GameObject(string model, Vector location)
            : this()
        {
            this.Name = "Dummy";
            this.Model = model;
            _position = _newPosition = location;
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

        /// <summary>
        /// Updates the externally visible <see cref="Position"/> and <see cref="OldPosition"/> values. 
        /// Returns whether the unit moved (NYI). 
        /// </summary>
        internal bool UpdateLocation()
        {
            //Debug.Assert(!IsDestroyed);

            _customPosition = false;
            _oldPosition = _position;
            _position = _newPosition;
            return true;
        }


        protected internal void setLocation(Vector loc, bool isRegularMove)
        {
            _newPosition = loc;
            _customPosition = !isRegularMove;
        }
    }
}
