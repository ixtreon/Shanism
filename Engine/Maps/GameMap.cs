using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Common;

namespace Engine.Maps
{
    public class GameMap
    {
        static readonly Vector MapCellSize = new Vector(5, 5);

        HashMap<Unit> units;

        HashMap<Doodad> doodads;

        HashMap<Effect> effects;

        public IEnumerable<Unit> Units
        {
            get { return units.Items; }
        }

        public IEnumerable<Doodad> Doodads
        {
            get { return doodads.Items; }
        }

        public IEnumerable<Effect> Effects
        {
            get { return effects.Items; }
        }

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
            units = new HashMap<Unit>((e => e.Location), MapCellSize, 100);
            doodads = new HashMap<Doodad>((e => e.Location), MapCellSize, 1000);
            effects = new HashMap<Effect>((e => e.Location), MapCellSize, 1000);
        }


        // updates objects' positions in this GameMap as they change. 
        private void Unit_LocationChanged(GameObject o)
        {
            units.UpdateItem(o as Unit);
        }


        // updates objects' positions in this GameMap as they change. 
        private void Doodad_LocationChanged(GameObject o)
        {
            doodads.UpdateItem(o as Doodad);
        }

        // removes objects from the GameMap as they change. 
        private void Unit_Destroyed(GameObject o)
        {
            units.Remove(o as Unit);
        }

        // removes objects from the GameMap as they change. 
        private void Doodad_Destroyed(GameObject o)
        {
            doodads.Remove(o as Doodad);
        }

        /// <summary>
        /// Adds the specified unit to this GameMap. 
        /// </summary>
        /// <param name="u">The unit to add to the map. </param>
        public void AddUnit(Unit u)
        {
            units.Add(u);
            u.LocationChanged += Unit_LocationChanged;
            u.Destroyed += Unit_Destroyed;

            //run scripts
            u.Game.Scenario.RunScripts(s => s.OnUnitSpawned(u));
        }

        public void AddDoodad(Doodad d)
        {
            doodads.Add(d);
            d.LocationChanged += Doodad_LocationChanged;
            d.Destroyed += Doodad_Destroyed;
        }


        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<Unit> GetUnitsInRect(Vector pos, Vector size)
        {
            return units.RangeQuery(pos, size);
        }

        /// <summary>
        /// Returns all units with locations within the specified rectangle. 
        /// </summary>
        /// <param name="pos">The coordinates of the upper-left (min) corner of the rectangle. </param>
        /// <param name="size">The size of the rectangle. </param>
        /// <returns>All units within the specified rectangle. </returns>
        public IEnumerable<GameObject> GetObjectsInRect(Vector pos, Vector size)
        {
            return units.RangeQuery(pos, size).Cast<GameObject>()
                .Concat(effects.RangeQuery(pos, size))
                .Concat(doodads.RangeQuery(pos, size));
        }

        /// <summary>
        /// Returns all units with locations in a given range from the specified position. 
        /// </summary>
        /// <param name="pos">The coordinates of the central point. </param>
        /// <param name="range">The maximum range of a unit from the central point. </param>
        /// <returns>All units within range of the given central point. </returns>
        public IEnumerable<Unit> GetUnitsInRange(Vector pos, double range)
        {
            //get a rectangle around the query region. 
            var windowPos = pos - range;
            var windowSize = new Vector(range) * 2;

            //pick the units in this rectangle. 
            var us = units.RangeQuery(windowPos, windowSize);

            //get exactly the units within the circle. 
            var rsq = range * range;
            return us
                .Where(e => e.Location.DistanceToSquared(pos) <= rsq);
        }

        /// <summary>
        /// Calls the update function for all objects on the GameMap: units, doodads, special effects. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last update, in milliseconds. </param>
        internal void Update(int msElapsed)
        {
            var objs = Objects.ToArray();

            foreach (var o in objs)
                if (o.IsDestroyed)
                    throw new Exception("Destroyed unit inside! :(");

            foreach (var o in objs)
                o.UpdateLocation(msElapsed);

            foreach (var o in objs)
                o.UpdateEffects(msElapsed);

            //foreach (var e in Doodads.ToArray())
            //    e.Update(msElapsed);
        }
    }
}
