using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Systems.RangeEvents;

namespace Engine.Systems
{
    class VisionSystem : ShanoSystem
    {
        RangeEventSystem RangeSystem { get; }
        public VisionSystem(ShanoEngine game, RangeEventSystem rangeSystem) : base(game)
        {
            RangeSystem = rangeSystem;

            game.Map.ItemAdded += map_ItemAdded;
        }

        void map_ItemAdded(GameObject obj)
        {
            var u = (obj as Unit);
            if (u == null) return;

            u.VisionRangeChanged += unit_VisionRangeChanged;
            unit_VisionRangeChanged(u);
        }

        void unit_VisionRangeChanged(Unit u)
        {
            //remove old handler
            if (u.ObjectVisionRangeEvent != null)
                RangeSystem.RemoveConstraint(u.ObjectVisionRangeEvent);

            //add a new handler
            var ev = new ObjectRangeEvent(u, u.VisionRange);
            ev.Triggered += u.AddObjectInVision;
            RangeSystem.AddConstraint(ev);
            u.ObjectVisionRangeEvent = ev;
        }
    }
}
