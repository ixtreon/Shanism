using Shanism.Common;
using Shanism.Common.Scenario;
using Shanism.Engine.Events;

namespace Shanism.Engine
{
    /// <summary>
    /// A base for the creation of custom game scripts. 
    /// </summary>
    public abstract class CustomScript : GameObject, IUserScript
    {
        /// <summary>
        /// Gets the <see cref="Shanism.Common.ObjectType"/> of this game object. 
        /// Always returns <see cref="ObjectType.Script"/>. 
        /// </summary>
        public override ObjectType ObjectType { get; } = ObjectType.Script;



        /// <summary>
        /// Executed when the game begins. 
        /// </summary>
        public virtual void OnGameStart() { }


        #region Player

        /// <summary>
        /// Executed when a player has joined the game. 
        /// </summary>
        /// <param name="pl">The player who joined the game.</param>
        public virtual void OnPlayerJoined(Player pl) { }

        /// <summary>
        /// Executed whenever a player's main hero is changed. 
        /// The new hero is available via the <see cref="Player.MainHero"/> property. 
        /// </summary>
        /// <param name="pl">The player who had his main hero changed.</param>
        public virtual void OnPlayerMainHeroChanged(Player pl) { }

        /// <summary>
        /// Executed whenever a player sends a chat message.
        /// </summary>
        /// <param name="e">The data about the player and their message.</param>
        public virtual void OnPlayerChatMessage(PlayerChatArgs e) { }

        #endregion

        #region Map

        /// <summary>
        /// The method executed whenever an entity is added to the game map. 
        /// </summary>
        /// <param name="e">The entity that was added to the map.</param>
        public virtual void OnEntityAdded(Entity e) { }

        #endregion

        #region Units

        /// <summary>
        /// The method executed when a unit has died. 
        /// </summary>
        public virtual void OnUnitDeath(UnitDyingArgs e) { }

        #endregion
    }
}
