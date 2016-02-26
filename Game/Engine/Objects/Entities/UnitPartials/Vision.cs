using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Engine.Events;
using System.Diagnostics;
using Engine.Systems.Range;
using IO.Util;
using IO.Message;

namespace Engine.Objects
{
    partial class Unit
    {


        double _visionRange;

        internal ConcurrentSet<Entity> visibleObjects { get; } = new ConcurrentSet<Entity>();


        /// <summary>
        /// Gets or sets the RangeEvent that is fired whenever an object approaches this unit. 
        /// </summary>
        internal RangeEvent ObjectVisionRangeEvent { get; set; }


        /// <summary>
        /// The event raised whenever a game object enters this unit's vision range. 
        /// </summary>
        public event Action<Entity> ObjectSeen;

        /// <summary>
        /// The event raised whenever a game object leaves this unit's vision range. 
        /// </summary>
        public event Action<Entity> ObjectUnseen;

        /// <summary>
        /// The event raised whenever this unit's vision range is changed. 
        /// </summary>
        public event Action<Unit> VisionRangeChanged;


        /// <summary>
        /// Gets all units this guy can see. 
        /// </summary>
        public IEnumerable<Entity> VisibleObjects
        {
            get { return visibleObjects; }
        }

        /// <summary>
        /// Gets or sets the vision range of the unit. 
        /// </summary>
        public double VisionRange
        {
            get { return _visionRange; }

            set
            {
                if (!_visionRange.AlmostEqualTo(value, 0.0005))  //should be ok, lel
                {
                    _visionRange = value;
                    VisionRangeChanged?.Invoke(this);
                }
            }
        }


        /// <summary>
        /// Gets whether the specified object is visible by this unit. 
        /// </summary>
        /// <param name="o">The object to check for visibility. </param>
        public bool IsInVisionRange(Entity o)
        {
            return visibleObjects.Contains(o);
        }

        internal void SendMessageToVisibles(IOMessage msg)
        {
            var pls = SeenBy
                .Select(u => u.Owner.Receptor)
                .Concat(new[] { Owner.Receptor })
                .Where(pl => pl != null)
                .Distinct()
                .ToList();

            foreach (var pl in pls)
                pl.SendMessage(msg);
        }

        /// <summary>
        /// Raises the <see cref="ObjectSeen"/> event. 
        /// </summary>
        internal virtual void OnObjectSeen(Entity obj) { ObjectSeen?.Invoke(obj); }


        /// <summary>
        /// Raises the <see cref="ObjectUnseen"/> event. 
        /// </summary>
        internal virtual void OnObjectUnseen(Entity obj) { ObjectUnseen?.Invoke(obj); }
    }
}
