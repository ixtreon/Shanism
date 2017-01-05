using Shanism.Engine.Maps;
using Shanism.Common;
using Shanism.Common.Util;
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
    public abstract class GameObject : IGameObject, IDisposable
    {
        #region Static Members

        internal static ShanoEngine currentGame { get; private set; }

        /// <summary>
        /// Tries to set the game engine instance all game objects are part of.
        /// Fails if there is another engine registered already. 
        /// </summary>
        public static bool SetGame(ShanoEngine game)
        {
            currentGame = game;
            return true;
        }
        #endregion


        /// <summary>
        /// Gets the <see cref="Shanism.Common.ObjectType"/> of this game object. 
        /// </summary>
        public abstract ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the unique ID of the game object.
        /// </summary>
        public uint Id { get; }

        #region Game Shortcuts

        /// <summary>
        /// Gets the map that contains the terrain and units in this scenario. 
        /// </summary>
        public IGameMap Map => currentGame?.Map;

        /// <summary>
        /// Gets the scenario this object is part of. 
        /// </summary>
        public Scenario Scenario => currentGame?.Scenario;

        /// <summary>
        /// Gets the game this object is part of. 
        /// </summary>
        public IGame Game => currentGame;


        internal PerfCounter UnitSystemPerfCounter => currentGame?.UnitPerfCounter;

        internal IScriptRunner Scripts => currentGame?.Scripts;
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        protected GameObject()
        {
            Id = Allocator<GameObject>.Allocate();
        }


        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> points to an entity with the same id.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) =>
            (obj is Entity) && ((Entity)obj).Id == Id;

        /// <summary>
        /// Determines whether the two objects are the same.
        /// </summary>
        public static bool operator ==(GameObject a, GameObject b)
            => a?.Id == b?.Id;

        /// <summary>
        /// Determines whether the two objects are different.
        /// </summary>
        public static bool operator !=(GameObject a, GameObject b)
            => a?.Id != b?.Id;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"{ObjectType} #{Id}";

        /// <summary>
        /// Releases the ID of this object. 
        /// </summary>
        public void Dispose()
        {
            Allocator<GameObject>.Deallocate(Id);
        }
    }
}
