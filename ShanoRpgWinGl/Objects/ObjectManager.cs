using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
using ShanoRpgWinGl.UI;

namespace ShanoRpgWinGl.Objects
{

    /// <summary>
    /// Keeps an up-to-date list of all nearby units (TODO: objects too!) and displays them on the screen. 
    /// </summary>
    class ObjectManager : UserControl
    {

        public UnitControl LocalHero { get; private set; }

        /// <summary>
        /// A dictionary of all units indexed by their id. 
        /// </summary>
        Dictionary<int, ObjectControl> Objects = new Dictionary<int, ObjectControl>();

        public event Action<UnitControl> UnitClicked;

        public ObjectManager(IHero h)
        {
            this.LocalHero = new UnitControl(h);
            Objects.Add(h.Guid, LocalHero);
            this.Add(LocalHero);
        }

        public void Update(int msElapsed, IEnumerable<IGameObject> newObjectz)
        {
            // initially mark all units for removal
            var toRemove = new HashSet<int>(Objects.Values.Select(o => o.Object.Guid));

            var newObjects = newObjectz.ToArray();
            //add new units, or mark existing ones as not-to-remove
            foreach(var o in newObjects)
            {
                if (this.Objects.ContainsKey(o.Guid))
                    toRemove.Remove(o.Guid);            // don't remove if its in the new list
                else
                    addObject(o);                         // an (actually) new guy 
            }
            
            //remove old units
            foreach (var guid in toRemove)
                    removeObject(guid);

            // update everyone who is still around
            foreach (var u in Objects.Values)
            {
                u.Update(msElapsed);
            }

            base.Update(msElapsed);
        }

        private void removeObject(int guid)
        {
            //never remove the local hero
            if (guid == LocalHero.Unit.Guid)
                return;

            this.Remove(Objects[guid]);
            Objects.Remove(guid);
        }

        private ObjectControl addObject(IGameObject o)
        {
            //TODO: fix for doodads, sfx
            ObjectControl gameObject = null;
            if (typeof(IUnit).IsAssignableFrom(o.GetType()))
                gameObject = new UnitControl((IUnit)o);
            else if (typeof(IDoodad).IsAssignableFrom(o.GetType()))
                gameObject = new DoodadControl((IDoodad)o);
            else
                throw new Exception("Some type of object we don't recognize yet!");

            Objects.Add(o.Guid, gameObject); 

            //gameObject.MouseDown += (p) =>
            //{
            //    if (UnitClicked != null)
            //        UnitClicked(gameObject);
            //};

            this.Add(gameObject);
            return gameObject;
        }

    }
}
