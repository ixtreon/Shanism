using Shanism.Engine.Maps;
using Shanism.Common;
using Shanism.Common.Performance;
using System.Collections.Concurrent;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.ScenarioLib;
using Shanism.Engine.Scripting;
using System.Threading.Tasks;
using System;

namespace Shanism.Engine
{
    /// <summary>
    /// Represents all things that belong to a game world. 
    /// This includes game objects, abilities, buffs, items, scripts (?)
    /// </summary>
    public abstract class GameObject : IGameObject
    {
        #region Static Members

        static readonly ConcurrentDictionary<uint, GameObject> idDict = new ConcurrentDictionary<uint, GameObject>();

        /// <summary>
        /// Gets the GameObject with the given Id, or null if it doesn't exist. 
        /// </summary>
        public static GameObject GetById(uint id)
        {
            return idDict.TryGet(id);
        }

        static ShanoEngine Game { get; set; }

        /// <summary>
        /// Tries to set the game engine instance all game objects are part of.
        /// Fails if there is another engine registered already. 
        /// </summary>
        public static bool TrySetEngine(ShanoEngine game)
        {
            if (Game != null)
                return false;

            Game = game;
            return true;
        }

        #endregion


        /// <summary>
        /// Gets the <see cref="Shanism.Common.Game.ObjectType"/> of this game object. 
        /// </summary>
        public abstract ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the unique ID of this object. 
        /// </summary>
        public readonly uint Id;

        uint IGameObject.Id => Id;

        /// <summary>
        /// Gets the map that contains the terrain and units in this scenario. 
        /// </summary>
        public IGameMap Map => Game?.Map;



        /// <summary>
        /// Gets the scenario this object is part of. 
        /// </summary>
        public Scenario Scenario => Game?.Scenario;



        internal PerfCounter UnitSystemPerfCounter => Game?.UnitPerfCounter;

        internal IScriptRunner Scripts => Game?.Scripts;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        protected GameObject()
        {
            Id = Shanism.Common.Util.GenericId<GameObject>.GetNew();
            idDict[Id] = this;
        }


        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }

        internal async Task Run(Action act)
        {
            await Task.Factory.StartNew(act);
        }
        
    }
}
