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

        /// <summary>
        /// Gets the map that contains the units in this scenario. 
        /// </summary>
        public EntityMap Map
        {
            get { return Game.EntityMap; }
        }

        /// <summary>
        /// Gets the terrain map of this scenario. 
        /// </summary>
        public ITerrainMap Terrain
        {
            get { return Game.TerrainMap; }
        }

        public ScenarioObject()
        {
            Game = ShanoEngine.Current;     // the ugly hack bites back
        }

        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }
    }
}
