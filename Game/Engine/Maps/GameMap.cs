using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;
using Engine.Objects.Game;
using System.Diagnostics;

namespace Engine.Maps
{
    public partial class GameMap
    {
        static readonly Vector MapCellSize = new Vector(5, 5);

        public readonly ObjectMap<Unit> Units = new ObjectMap<Unit>();

        public readonly ObjectMap<Doodad> Doodads = new ObjectMap<Doodad>();

        public readonly ObjectMap<Effect> Effects = new ObjectMap<Effect>();

        public IEnumerable<GameObject> Objects
        {
            get {  return Units
                    .Cast<GameObject>()
                    .Concat(Doodads)
                    .Concat(Effects);
            }
        }

        public GameMap()
        {

        }


        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<Unit> GetUnitsInRect(Vector pos, Vector size, bool aliveOnly = true)
        {
            return Units.RangeQuery(pos, size)
                .Where(u => !(u.IsDead && aliveOnly));
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<GameObject> GetObjectsInRect(Vector pos, Vector size)
        {
            return Units.RangeQuery(pos, size).Cast<GameObject>()
                .Concat(Effects.RangeQuery(pos, size))
                .Concat(Doodads.RangeQuery(pos, size))
                .ToArray();
        }

        /// <summary>
        /// Returns all units with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <returns>All units within range of the given central point. </returns>
        public IEnumerable<Unit> GetUnitsInRange(Vector pos, double range, bool aliveOnly = true)
        {
            //get a rectangle around the query region. 
            var windowPos = pos - range;
            var windowSize = new Vector(range) * 2;

            //pick the units in this rectangle. 
            var us = GetUnitsInRect(windowPos, windowSize, aliveOnly);

            //get exactly the units within the circle. 
            var rsq = range * range;
            return us.Where(u => u.Location.DistanceToSquared(pos) <= rsq);
        }

        /// <summary>
        /// Calls the update function for all objects on the GameMap: units, doodads, special effects. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        internal void Update(int msElapsed)
        {
            var objs = Objects.ToArray();

            //shouldn't have destroyed units inside the map. 
            Debug.Assert(!objs.Any(o => o.IsDestroyed));

            //Parallel.ForEach(objs, (o) =>
            foreach(var o in objs)
            {
                if (o.MarkedForDestruction || o.IsDestroyed)
                    throw new Exception("Object was marked for destruction!");
                o.Update(msElapsed);
            }
            //);

            //update positions, remove dead units
            foreach (var o in objs)
            {
                o.SyncLocation();
                o.Finalise();
            }

            //check range constraints
            foreach (var o in objs.OfType<Unit>().Where(u => !u.IsDead))
                checkRangeConstraints(o);
        }
    }
}
