using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Common.Message;
using Shanism.Engine.Objects.Range;
using Shanism.Engine.Players;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {
        //used by range events
        internal readonly Dictionary<Entity, double> nearbyDistances = new Dictionary<Entity, double>();

        internal readonly HashSet<Entity> nearbyEntities = new HashSet<Entity>();

        //used by the vision system
        readonly HashSet<Entity> objectsSeen = new HashSet<Entity>();

        double _visionRange = 20;


        /// <summary>
        /// The event raised whenever an entity enters this unit's vision range. 
        /// </summary>
        public event Action<Entity> ObjectSeen;

        /// <summary>
        /// The event raised whenever an entity leaves this unit's vision range. 
        /// </summary>
        public event Action<Entity> ObjectUnseen;

        /// <summary>
        /// The event raised whenever this unit's vision range is changed. 
        /// </summary>
        public event Action<Unit> VisionRangeChanged;


        /// <summary>
        /// Gets all entities this unit can see. 
        /// </summary>
        public IEnumerable<Entity> VisibleObjects => objectsSeen;

        /// <summary>
        /// Gets or sets the vision range of the unit. 
        /// </summary>
        public double VisionRange
        {
            get { return _visionRange; }

            set
            {
                _visionRange = value;
                OnVisionRangeChanged();
            }
        }


        /// <summary>
        /// Gets whether the specified entity is visible by this unit. 
        /// </summary>
        /// <param name="o">The entity to check for visibility. </param>
        public bool IsInVisionRange(Entity o)
            => objectsSeen.Contains(o);


        internal void SendMessageToVisibles(IOMessage msg)
        {
            var set = new HashSet<ShanoReceptor>();
            set.Add(Owner.Receptor);

            foreach (var u in visibleFromUnits)
                set.Add(u.Owner.Receptor);

            foreach (var pl in set)
                pl?.SendMessage(msg);
        }


        /// <summary>
        /// Called whenever an entity enters this unit's vision range. 
        /// Raises the <see cref="ObjectSeen"/> event. 
        /// </summary>
        public virtual void OnObjectSeen(Entity e)
            => ObjectSeen?.Invoke(e);

        /// <summary>
        /// Called whenever an entity leaves this unit's vision range. 
        /// Raises the <see cref="ObjectUnseen"/> event. 
        /// </summary>
        public virtual void OnObjectUnseen(Entity e)
            => ObjectUnseen?.Invoke(e);


        /// <summary>
        /// Called whenever this unit's vision range changes. 
        /// Raises the <see cref="VisionRangeChanged"/> event. 
        /// </summary>
        public virtual void OnVisionRangeChanged()
            => VisionRangeChanged?.Invoke(this);


        internal void see(Entity e)
        {
            if (objectsSeen.Add(e))
            {
                e.visibleFromUnits.Add(this);
                OnObjectSeen(e);
            }
        }

        internal void unsee(Entity e)
        {
            if (objectsSeen.Remove(e))
            {
                e.visibleFromUnits.Remove(this);
                OnObjectUnseen(e);
            }
        }

        internal void unseeAll()
        {
            foreach (var e in objectsSeen)
            {
                e.visibleFromUnits.Remove(this);
                OnObjectUnseen(e);
            }
            objectsSeen.Clear();
        }
    }
}
