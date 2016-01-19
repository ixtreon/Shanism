using Engine.Maps;
using Engine.Objects;
using Engine.Objects.Game;
using Engine.Systems.RangeEvents;
using IO;
using IO.Common;
using IO.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems
{
    class RangeEventSystem : ShanoSystem
    {
        //all global constraints
        //HashMap<PointRangeEvent> mapConstraints = new HashMap<PointRangeEvent>(new IO.Common.Vector(Constants.GameMap.ChunkSize));


        readonly object _mapLock = new object();
        readonly object _objectLock = new object();



        public RangeEventSystem(ShanoEngine game) : base(game)
        {
        }


        #region Public add/remove methods
        public void AddConstraint(PointRangeEvent c)
        {
            throw new NotImplementedException();
        }

        public void AddConstraint(ObjectRangeEvent c)
        {
            c.Origin.RangeEvents.Add(c);
        }


        public bool RemoveConstraint(PointRangeEvent c)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified ObjectConstraint from this unit's list of range constriants. 
        /// </summary>
        public bool RemoveConstraint(ObjectRangeEvent c)
        {
            return c.Origin.RangeEvents.Remove(c);
        }
        #endregion

        internal override void Update(int msElapsed)
        {
            foreach (var obj in Game.Map)
                CheckAllConstraints(obj);

            foreach (var obj in Game.Map)
                obj.UpdatePosition();
        }


    public void CheckAllConstraints(GameObject origin)
    {
        if (!origin.RangeEvents.Any())
            return;

        var maxConstraintRange = origin.RangeEvents.Max?.Range ?? 0;
        var maxRangeSq = maxConstraintRange * maxConstraintRange;
        var nearbyObjs = Game.Map.RawQuery(new RectangleF(origin.Position - maxConstraintRange, new Vector(2 * maxConstraintRange)));

        foreach (var target in nearbyObjs)
        {
            var objDistSq = target.Position.DistanceToSquared(origin.Position);
            if (objDistSq > maxRangeSq)
                continue;

            var oldDistSq = target.OldPosition.DistanceToSquared(origin.OldPosition);

            foreach (var c in origin.RangeEvents)
                c.Check(target, objDistSq, oldDistSq);
        }

        //TODO: check for global constraints
    }
}
}
