using Engine.Events;
using Engine.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps
{
    partial class GameMap
    {
        private List<GlobalRangeConstraint> globalConstraints = new List<GlobalRangeConstraint>();

        private int addConstraint(GlobalRangeConstraint c)
        {
            globalConstraints.Add(c);
            return globalConstraints.Count - 1;
        }

        internal IEnumerable<GlobalRangeConstraint> GetRangeConstraints()
        {
            return globalConstraints;
        }

        public int RegisterRangeEvent(Unit unit, double range, EventType evType, Action<RangeArgs> act)
        {
            return addConstraint(new GlobalRangeConstraint(unit, range, evType, act));
        }

        public int RegisterRangeEvent(Vector origin, double range, EventType evType, Action<RangeArgs> act)
        {
            return addConstraint(new GlobalRangeConstraint(origin, range, evType, act));
        }

        public void UnregisterRangeEvent(int eventId)
        {
            globalConstraints.RemoveAt(eventId);
        }

        private void checkRangeConstraints(Unit u)
        {
            Debug.Assert(!u.IsDead);
            Debug.Assert(!u.IsDestroyed);
            var globalConstraints = this.globalConstraints;

            foreach (var c in globalConstraints)    //check if this guy triggered any of the global constraints
                c.Check(u);

            foreach (var c in u.GetRangeConstraints())
                c.Check(u);
        }
    }
}
