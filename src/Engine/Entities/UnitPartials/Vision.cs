using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Messages;
using Shanism.Engine.Players;
using Shanism.Engine.Objects.Range;
using Shanism.Common;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {
        //used by the vision system
        readonly HashSet<Entity> visibleEntities = new HashSet<Entity>();

        float _visionRange = 20;


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
        public IEnumerable<Entity> VisibleEntities => visibleEntities;

        /// <summary>
        /// Gets or sets the vision range of the unit. 
        /// </summary>
        public float VisionRange
        {
            get { return _visionRange; }
            set
            {
                _visionRange = Math.Max(0, value);
                OnVisionRangeChanged();
            }
        }


        /// <summary>
        /// Gets whether the specified entity is visible by this unit. 
        /// </summary>
        /// <param name="o">The entity to check for visibility. </param>
        public bool IsVisible(Entity o)
            => visibleEntities.Contains(o);


        internal void SendMessageToVisibles(ServerMessage msg)
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
        internal void OnObjectSeen(Entity e)
            => ObjectSeen?.Invoke(e);

        /// <summary>
        /// Called whenever an entity leaves this unit's vision range. 
        /// Raises the <see cref="ObjectUnseen"/> event. 
        /// </summary>
        internal void OnObjectUnseen(Entity e)
            => ObjectUnseen?.Invoke(e);


        /// <summary>
        /// Called whenever this unit's vision range changes. 
        /// Raises the <see cref="VisionRangeChanged"/> event. 
        /// </summary>
        internal void OnVisionRangeChanged()
            => VisionRangeChanged?.Invoke(this);

        /// <summary>
        /// Causes this unit to "see" the other entity.
        /// </summary>
        internal void See(Entity e)
        {
            if (visibleEntities.Add(e))
            {
                e.visibleFromUnits.Add(this);
                OnObjectSeen(e);
            }
        }

        internal void Unsee(Entity e)
        {
            if (visibleEntities.Remove(e))
            {
                e.visibleFromUnits.Remove(this);
                OnObjectUnseen(e);
            }
        }

        internal void UnseeAll()
        {
            foreach (var e in visibleEntities)
            {
                e.visibleFromUnits.Remove(this);
                OnObjectUnseen(e);
            }
            visibleEntities.Clear();
        }
    }
}
