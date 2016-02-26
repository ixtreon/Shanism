using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using IO;
using ProtoBuf;
using Engine.Objects;
using IO.Performance;
using System.Reflection;
using System.Collections.Concurrent;
using IO.Common;
using IO.Interfaces.Engine;

namespace Engine
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
        /// Sets the game engine instance all game objects are part of.
        /// </summary>
        public static void SetEngine(ShanoEngine game)
        {
            Game = game;
        }

        #endregion


        /// <summary>
        /// Gets the object type of this object. 
        /// </summary>
        public abstract ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the unique ID of this object. 
        /// </summary>
        public readonly uint Id;

        uint IGameObject.Id => Id;

        /// <summary>
        /// Gets the map that contains the units in this scenario. 
        /// </summary>
        public IGameMap Map => Game?.map;


        /// <summary>
        /// Gets the terrain map of the scenario this object is part of. 
        /// </summary>
        public ITerrainMap Terrain => Game?.TerrainMap;

        /// <summary>
        /// Gets the scenario this object is part of. 
        /// </summary>
        public Scenario Scenario => Game?.Scenario;

        internal PerfCounter PerfCounter => Game?.PerfCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        protected GameObject()
        {
            Id = IO.Util.GenericId<GameObject>.GetNew();
            idDict[Id] = this;
        }


        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }
    }
}
