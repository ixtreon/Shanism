using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Engine.Objects.Range;
using System.Threading;

namespace Shanism.Engine.Systems
{
    class RangeSystem : UnitSystem
    {
        static readonly GenericComparer<RangeEvent> eventComparer = new GenericComparer<RangeEvent>((a, b) => a.Range.CompareTo(b.Range));


        readonly Unit Owner;

        readonly SortedSet<RangeEvent> events = new SortedSet<RangeEvent>(eventComparer);


        public RangeSystem(Unit origin)
        {
            Owner = origin;
        }


        public IEnumerable<RangeEvent> SortedEvents => events;

        public double MaxEventRange => events.Max.Range;

        public bool AddEvent(RangeEvent e) => events.Add(e);

        public bool RemoveEvent(RangeEvent e) => events.Remove(e);
    }
}
