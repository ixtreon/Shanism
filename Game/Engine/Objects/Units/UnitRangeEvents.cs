using Engine.Events;
using Engine.Maps;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    partial class Unit
    {

        static List<UnitRangeConstraint> unitConstraints = new List<UnitRangeConstraint>();


        private static int addConstraint(UnitRangeConstraint c)
        {
            unitConstraints.Add(c);
            return unitConstraints.Count - 1;
        }


        internal IEnumerable<UnitRangeConstraint> GetRangeConstraints()
        {
            return unitConstraints;
        }

        public int RegisterRangeEvent(Vector pos, double range, EventType evType, Action<RangeArgs> act)
        {
            return addConstraint(new UnitRangeConstraint(this, pos, range, evType, act));
        }

        public int RegisterRangeEvent(Unit other, double range, EventType evType, Action<RangeArgs> act)
        {
            return addConstraint(new UnitRangeConstraint(this, other, range, evType, act));
        }

        public void UnregisterRangeEvent(int eventId)
        {
            unitConstraints.RemoveAt(eventId);
        }


        public int RegisterInRangeOf(Vector pos, double range, Action<RangeArgs> act)
        {
            return addConstraint(new UnitRangeConstraint(this, pos, range, EventType.EntersRange, act));
        }

        public int RegisterLeavesRangeOf(Vector pos, double range, Action<RangeArgs> act)
        {
            return addConstraint(new UnitRangeConstraint(this, pos, range, EventType.LeavesRange, act));
        }


        public int RegisterInRangeOf(Unit other, double range, Action<RangeArgs> act)
        {
            return addConstraint(new UnitRangeConstraint(this, other, range, EventType.EntersRange, act));
        }

        public int RegisterLeavesRangeOf(Unit other, double range, Action<RangeArgs> act)
        {
            return addConstraint(new UnitRangeConstraint(this, other, range, EventType.LeavesRange, act));
        }
    }
}
