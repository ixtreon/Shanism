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
using System.Diagnostics;
using System.Threading;

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

        //fucking circular reference
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

        public string ModelString { get; set; }

        /// <summary>
        /// Gets the globally unique identifier of the object. 
        /// </summary>
        public int Guid { get; private set; }

        /// <summary>
        /// Gets or sets the custom data for this object. 
        /// </summary>
        public dynamic Data { get; set; }

        public abstract ObjectType ObjectType { get; }

        /// <summary>
        /// Gets or sets the location of the game object. 
        /// Changes will not take effect until the next game cycle. 
        /// </summary>
        public Vector Position
        {
            get { return _position; }
            set { setLocation(value, false); }
        }

        internal Vector OldPosition { get { return _oldPosition; } }

        internal Vector NewPosition {  get { return _newPosition; } }

        /// <summary>
        /// Gets whether the unit was moved by magix this turn. 
        /// </summary>
        public bool HasCustomPosition {  get { return _customPosition; } }

        /// <summary>
        /// Gets the model of the object. 
        /// </summary>
        AnimationDef IGameObject.Model
        {
            get
            {
                return Game.Scenario.Models[this.ModelString];
            }
        }


        protected GameObject()
        {
            this.Size = 0.4;
            this.ModelString = "default";
            Guid = ObjectGuid.GetNew();
            _oldPosition = new Vector(double.NaN);
        }

        public GameObject(string model, Vector location)
            : this()
        {
            this.Name = "Dummy";
            this.ModelString = model;
            _position = _newPosition = location;
        }


        /// <summary>
        /// Updates the externally visible <see cref="Position"/> and <see cref="OldPosition"/> values. 
        /// Returns whether the unit moved. 
        /// </summary>
        internal bool UpdateLocation()
        {
            Debug.Assert(!IsDestroyed);

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
