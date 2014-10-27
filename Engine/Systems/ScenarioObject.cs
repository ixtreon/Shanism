using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;

namespace Engine.Systems
{
    /// <summary>
    /// Represents all things that belong in a scenario. 
    /// This includes game objects, abilities, buffs, items, scripts
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
        public  ShanoRpg Game { get; internal set; }

        public GameMap Map
        {
            get { return Game.GameMap; }
        }

        public WorldMap Terrain
        {
            get { return Game.WorldMap; }
        }

        public ScenarioObject()
        {
            //if (ShanoRpg.Current == null)
                //throw new Exception("Creating an instance of an object without an engine!");
            this.Game = ShanoRpg.Current;
        }
    }
}
