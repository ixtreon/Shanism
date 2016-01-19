using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using IO;

namespace Engine
{
    /// <summary>
    /// Represents all things that belong to a scenario. 
    /// This includes game objects, abilities, buffs, items, scripts (?)
    /// </summary>
    public abstract class ScenarioObject
    {
        static ShanoEngine Game { get; set; }

        public static void Init(ShanoEngine game)
        {
            Game = game;
        }

        /// <summary>
        /// Gets the map that contains the units in this scenario. 
        /// </summary>
        public EntityMap Map
        {
            get { return Game?.EntityMap; }
        }

        /// <summary>
        /// Gets the terrain map of the scenario this object is part of. 
        /// </summary>
        public ITerrainMap Terrain
        {
            get { return Game?.TerrainMap; }
        }

        /// <summary>
        /// Gets the scenario this object is part of. 
        /// </summary>
        public Scenario Scenario
        {
            get { return Game?.Scenario; }
        }

        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }
    }
}
