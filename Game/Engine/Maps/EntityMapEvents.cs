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
    partial class EntityMap
    {
        //all global constraints
        List<IRangeConstraint> globalConstraints = new List<IRangeConstraint>();

        int addConstraint(RangeConstraint c)
        {
            var id = globalConstraints.Count;
            globalConstraints.Add(c);


            return id;
        }
        

        //internal IEnumerable<IRangeConstraint> GetRangeConstraints()
        //{
        //    return globalConstraints;
        //}

        /// <summary>
        /// Calls the provided method whenever any GameObject crosses at specified range from the specified GameObject. 
        /// Returns a handle to the registered event. 
        /// </summary>
        /// <param name="originObject">The origin GameObject</param>
        /// <param name="range">The range at which an event is triggered</param>
        /// <param name="evType">The subset of events to respond to. </param>
        /// <param name="act">The <see cref="Action"/> to be performed whenever a unit crosses the range threshold</param>
        public int RegisterAnyUnitInRangeEvent(GameObject originObject, double range, EventType evType, Action<RangeArgs<Unit>> act)
        {
            return addConstraint(new GlobalRangeConstraint<Unit>(originObject, range, evType, act));
        }

        /// <summary>
        /// Calls the provided method whenever any GameObject crosses at specified range from the given in-game location. 
        /// Returns a handle to the registered event. 
        /// </summary>
        /// <param name="origin">The origin location</param>
        /// <param name="range">The range at which an event is triggered</param>
        /// <param name="evType">The subset of events to respond to. </param>
        /// <param name="act">The <see cref="Action"/> to be performed whenever a unit crosses the range threshold</param>
        public int RegisterAnyUnitInRangeEvent(Vector origin, double range, EventType evType, Action<RangeArgs<Unit>> act)
        {
            return addConstraint(new GlobalRangeConstraint<Unit>(origin, range, evType, act));
        }

        public int RegisterAnyObjectInRangeEvent(GameObject origin, double range, EventType evType, Action<RangeArgs<GameObject>> act)
        {
            return addConstraint(new GlobalRangeConstraint<GameObject>(origin, range, evType, act));
        }

        public void UnregisterRangeEvent(int eventId)
        {
            globalConstraints.RemoveAt(eventId);
        }

        /// <summary>
        /// Performs all range checks for the given object. 
        /// </summary>
        /// <param name="obj"></param>
        void checkRangeConstraints(GameObject obj)
        {
            if (obj.IsDestroyed)
                throw new Exception("Trying to check a range constraint for a destroyed unit!");

            //check global constraints
            var globalConstraints = this.globalConstraints;

            foreach (var c in globalConstraints)    //check if this guy triggered any of the global constraints
                if(!(c.OriginType == OriginType.GameObject && c.Origin == obj))
                    c.Check(obj);

            if(obj is Unit)
            {
                var u = (Unit)obj;

                foreach (var c in u.GetRangeConstraints())
                    c.Check(u);
            }
        }
    }
}
