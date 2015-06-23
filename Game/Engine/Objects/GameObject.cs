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
using IO.Content;
using Engine.Objects.Game;

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
        public string Name { get; set; }

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

        internal Vector OldLocation { get; private set; }

        private Vector _location;

        private Vector newLocation { get; set; }

        /// <summary>
        /// Gets or sets the location of the game object. 
        /// 
        /// Changes will not take effect until the next cycle. 
        /// </summary>
        public Vector Location
        {
            get { return _location; }
            set
            {
                // do not update the location directly
                if (newLocation != value)
                    newLocation = value;
            }
        }

        /// <summary>
        /// Updates the external 
        /// </summary>
        internal void SyncLocation()
        {
            if (this.IsDestroyed)
                throw new Exception();

            // check if we have moved
            if (_location == newLocation)
                return;

            //update our location records
            OldLocation = _location;
            _location = newLocation;

            //fire the event
            if (LocationChanged != null)
                LocationChanged(this);
        }

        /// <summary>
        /// Raised whenever this object changes its location. 
        /// The old location is passed as an argument. 
        /// </summary>
        public event Action<GameObject> LocationChanged;

        protected GameObject()
        {
            this.Size = 0.4;
            this.Model = "default";
            this.Tint = Color.White;
            Guid = GetGuid();
        }

        public GameObject(string model, Vector location)
            : this()
        {
            this.Name = "Dummy";
            this.Model = model;
            OldLocation = _location = newLocation = location;
        }


        AnimationDef IGameObject.Model
        {
            get
            {
                return Game.Scenario.Models[this.Model];
            }
        }

        /// <summary>
        /// The function to update this object's effects as the game progresses. 
        /// </summary>
        /// <param name="msElapsed"></param>
        //internal virtual void Update(int msElapsed) { }
    }
}
