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
        internal ShanoEngine Game { get; set; }

        public EntityMap Map
        {
            get { return Game.EntityMap; }
        }

        public ITerrainMap Terrain
        {
            get { return Game.TerrainMap; }
        }

        public ScenarioObject()
        {
            Game = ShanoEngine.Current;     // the ugly hack bites back
        }

        internal bool IsDestroyed { get; private set; }


        /// <summary>
        /// Marks this GameObject for destruction, eventually removing it from the game. 
        /// </summary>
        public virtual void Destroy()
        {
            if (IsDestroyed)
                throw new InvalidOperationException("Trying to destroy an object twice!");

            IsDestroyed = true;
        }

        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }
    }
}
