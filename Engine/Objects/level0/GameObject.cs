using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using Engine.Systems;
using IO;
using IO.Common;
using ProtoBuf;

namespace Engine.Objects
{
    /// <summary>
    /// A base class for all entities on the map. 
    /// Currently this includes doodads, units, special effects. 
    /// </summary>
    [ProtoContract]
    [ProtoInclude(1, typeof(Unit))]
    [ProtoInclude(2, typeof(Doodad))]
    [ProtoInclude(3, typeof(Effect))]
    public abstract class GameObject : ScenarioObject, IGameObject
    {
        public const double MaximumSize = 3;

        private static int guidCount = 0;
        private static int GetGuid()
        {
            return ++guidCount;
        }

        /// <summary>
        /// Gets the name of the game object. 
        /// </summary>
        [ProtoMember(4)]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the size of the object. 
        /// </summary>
        [ProtoMember(5)]
        public double Size { get; set; }

        [ProtoMember(6)]
        public string Model { get; set; }

        //[ProtoMember(7)]
        public Color Tint { get; set; }

        /// <summary>
        /// Gets the globally unique identifier of the object. 
        /// </summary>
        public int Guid { get; private set; }

        private Vector location;

        /// <summary>
        /// Gets or sets the location of the game object. 
        /// </summary>
        public Vector Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    if (!IsDestroyed && LocationChanged != null)
                        LocationChanged(this);

                    location = value;
                }
            }
        }

        /// <summary>
        /// Raised whenever this object changes its location. 
        /// </summary>
        public event Action<GameObject> LocationChanged;

        protected GameObject()
        {
            this.Size = 0.4;
            this.Model = "default";
            this.Tint = Color.White;
            Guid = GetGuid();
        }

        public GameObject(string name)
            : this()
        {
            this.Name = name;
        }

        internal bool IsDestroyed { get; private set; }

        Model IGameObject.Model
        {
            get
            {
                return Game.Scenario.Models[this.Model];
            }
        }

        internal event Action<GameObject> Destroyed;

        /// <summary>
        /// Destroys this GameObject, removing it from the game. 
        /// </summary>
        public void Destroy()
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Trying to destroy an object twice!");
            this.IsDestroyed = true;

            //fire the event. 
            if (Destroyed != null)
                Destroyed(this);
        }

        /// <summary>
        /// The function to update an object's position as game progresses. 
        /// It should not calculate any effects or collision as not all objects
        /// will have moved yet. 
        /// </summary>
        internal virtual void UpdateLocation(int msElapsed) { }

        /// <summary>
        /// The function to update this object's effects as the game progresses. 
        /// It should not move the object around the map as other objects may
        /// have already applied effects onto it. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void UpdateEffects(int msElapsed) { }
    }
}
