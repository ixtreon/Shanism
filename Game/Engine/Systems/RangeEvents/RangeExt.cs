using Engine.Maps;
using Engine.Objects;
using Engine.Systems.RangeEvents;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class MapRangeEvents
    {
        ///// <summary>
        ///// Calls the provided method whenever any GameObject comes at a given distance from the given GameObject. 
        ///// </summary>
        ///// <param name="originObject">The origin GameObject</param>
        ///// <param name="evType">The subset of events to respond to. </param>
        ///// <param name="act">The <see cref="Action"/> to be performed whenever a unit crosses the range threshold</param>
        //public static Constraint RegisterAnyUnitInRangeEvent(this EntityMap map, GameObject originObject, double range, Action<Unit> act)
        //{
        //    return map.RangeProvider.AddConstraint(new GlobalRangeConstraint<Unit>(originObject, range, evType, act));
        //}

        ///// <summary>
        ///// Calls the provided method whenever any GameObject crosses at specified range from the given in-game location. 
        ///// Returns a handle to the registered event. 
        ///// </summary>
        ///// <param name="origin">The origin location</param>
        ///// <param name="range">The range at which an event is triggered</param>
        ///// <param name="evType">The subset of events to respond to. </param>
        ///// <param name="act">The <see cref="Action"/> to be performed whenever a unit crosses the range threshold</param>
        //public static int RegisterAnyUnitInRangeEvent(this EntityMap map, Vector origin, double range, EventType evType, Action<RangeArgs<Unit>> act)
        //{
        //    return map.RangeProvider.AddConstraint(new GlobalRangeConstraint<Unit>(origin, range, evType, act));
        //}

        ///// <summary>
        ///// Calls the provided method whenever any GameObject crosses at specified range from the given origin game object. 
        ///// Returns a handle to the registered event. 
        ///// </summary>
        ///// <param name="origin">The origin object. </param>
        ///// <param name="range">The range at which an event is triggered</param>
        ///// <param name="evType">The subset of events to respond to. </param>
        ///// <param name="act">The <see cref="Action"/> to be performed whenever a unit crosses the range threshold</param>
        //public static int RegisterAnyObjectInRangeEvent(this EntityMap map, GameObject origin, double range, EventType evType, Action<RangeArgs<GameObject>> act)
        //{
        //    return map.RangeProvider.AddConstraint(new GlobalRangeConstraint<GameObject>(origin, range, evType, act));
        //}

        ///// <summary>
        ///// Unregisters an existing event given its id. 
        ///// </summary>
        ///// <param name="eventId"></param>
        //public static void UnregisterRangeEvent(this EntityMap map, int eventId)
        //{
        //    map.RangeProvider.RemoveConstraint(eventId);
        //}
    }

    public static class UnitRangeEvents
    {
        //public static ObjectRangeEvent RegisterAnyObjectInRange(this Unit u, double range, Action<GameObject> triggerFunc)
        //{
        //    var c = new ObjectRangeEvent(u, range);
        //    c.Triggered += triggerFunc;

        //    u.Map.RangeProvider.AddConstraint(c);
        //    return c;
        //}

        //public static bool UnregisterRangeEvent(this Unit u, ObjectRangeEvent c)
        //{
        //    return u.Map.RangeProvider.RemoveConstraint(c);
        //}
    }
}
