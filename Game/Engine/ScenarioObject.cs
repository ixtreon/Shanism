using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;

namespace Engine
{
    /// <summary>
    /// Represents all things that belong to a scenario. 
    /// This includes game objects, abilities, buffs, items, scripts (?)
    /// </summary>
    public abstract class ScenarioObject
    {
        private Dictionary<string, object> customData = new Dictionary<string, object>();

        public object this[string s]
        {
            get { return customData[s]; }
            set { customData[s] = value; }
        }

        //todo: make this readonly
        public ShanoEngine Game { get; internal set; }

        public GameMap Map
        {
            get { return Game.GameMap; }
        }

        public RandomMap Terrain
        {
            get { return Game.WorldMap; }
        }

        public ScenarioObject()
        {
            //if (ShanoRpg.Current == null)
            //    throw new Exception("Creating an instance of an object without an engine!");
            this.Game = ShanoEngine.Current;
        }

        internal bool MarkedForDestruction { get; private set; }

        internal bool IsDestroyed { get; private set; }

        internal event Action<ScenarioObject> Destroyed;

        /// <summary>
        /// Marks this GameObject for destruction, eventually removing it from the game. 
        /// </summary>
        public virtual void Destroy(bool finalize = false)
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Trying to destroy an object twice!");
            this.MarkedForDestruction = true;
            if (finalize)
                this.Finalise();
        }

        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }

        internal void Finalise()
        {
            if (this.MarkedForDestruction)
            {
                this.IsDestroyed = true;

                //fire the event. 
                if (Destroyed != null)
                    Destroyed(this);
            }
        }
    }
}
