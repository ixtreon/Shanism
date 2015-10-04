﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
using Client.UI;
using Client.Objects;

namespace Client
{

    /// <summary>
    /// Keeps an up-to-date list of all nearby units (TODO: objects too!) and displays them on the screen. 
    /// </summary>
    class ObjectManager : Control
    {

        public UnitControl LocalHero { get; private set; }

        /// <summary>
        /// A dictionary of all objects indexed by their guid. 
        /// </summary>
        Dictionary<int, ObjectControl> Objects = new Dictionary<int, ObjectControl>();

        /// <summary>
        /// The event raised whenever a unit was clicked. 
        /// </summary>
        public event Action<UnitControl> UnitClicked;

        public void Update(int msElapsed, IEnumerable<IGameObject> newObjects)
        { 
            // mark all units for removal at first
            var toRemove = new HashSet<int>(Objects.Values.Select(o => o.Object.Guid));

            //add new units, or mark existing ones as not-to-remove
            foreach(var o in newObjects.ToArray())
            {
                if (this.Objects.ContainsKey(o.Guid))
                    toRemove.Remove(o.Guid);            // don't remove if it's here now
                else
                    addObject(o);                       // an (actually) new guy 
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

        void removeObject(int guid)
        {
            //never remove the local hero
            if (LocalHero != null && guid == LocalHero.Unit.Guid)
                return;

            this.Remove(Objects[guid]);
            Objects.Remove(guid);
        }

        ObjectControl addObject(IGameObject o)
        {
            //TODO: fix for doodads, sfx
            ObjectControl gameObject = null;
            if (typeof(IUnit).IsAssignableFrom(o.GetType()))
                gameObject = new UnitControl((IUnit)o);
            else if (typeof(IDoodad).IsAssignableFrom(o.GetType()))
                gameObject = new DoodadControl((IDoodad)o);
            else
                throw new Exception("Some type of object we don't recognize yet!");

            gameObject.MouseDown += gameObject_MouseDown;

            Objects.Add(o.Guid, gameObject); 

            this.Add(gameObject);
            return gameObject;
        }

        void gameObject_MouseDown(Control c, Microsoft.Xna.Framework.Vector2 pos)
        {
            if (c is UnitControl)
                if (UnitClicked != null)
                    UnitClicked((UnitControl)c);
        }

    }
}
