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

namespace Engine.Entities
{
    partial class Unit
    {


        double _visionRange;

        internal ConcurrentSet<GameObject> visibleObjects { get; } = new ConcurrentSet<GameObject>();


        /// <summary>
        /// Gets or sets the RangeEvent that is fired whenever an object approaches this unit. 
        /// </summary>
        internal UnitRangeEvent ObjectVisionRangeEvent { get; set; }


        /// <summary>
        /// The event raised whenever a game object enters this unit's vision range. 
        /// </summary>
        public event Action<GameObject> ObjectSeen;

        /// <summary>
        /// The event raised whenever a game object leaves this unit's vision range. 
        /// </summary>
        public event Action<GameObject> ObjectUnseen;

        /// <summary>
        /// The event raised whenever this unit's vision range is changed. 
        /// </summary>
        public event Action<Unit> VisionRangeChanged;


        /// <summary>
        /// Gets all units this guy can see. 
        /// </summary>
        public IEnumerable<GameObject> VisibleObjects
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


        /// Gets whether the specified object is visible by this unit. 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool IsInVisionRange(GameObject o)
        {
            return visibleObjects.Contains(o);
        }

        internal void SendMessageToVisibles(IOMessage msg)
        {
            var pls = SeenBy
                .Select(u => u.Owner)
                .Concat(new[] { Owner })
                .Where(pl => pl.IsHuman)
                .Distinct()
                .ToList();

            foreach (var pl in pls)
                pl.SendMessage(msg);
        }

        /// <summary>
        /// Raises the <see cref="ObjectSeen"/> event. 
        /// </summary>
        internal virtual void OnObjectSeen(GameObject obj) { ObjectSeen?.Invoke(obj); }


        /// <summary>
        /// Raises the <see cref="ObjectUnseen"/> event. 
        /// </summary>
        internal virtual void OnObjectUnseen(GameObject obj) { ObjectUnseen?.Invoke(obj); }
    }
}
