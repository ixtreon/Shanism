using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Objects.Game;
using ScenarioLib;

namespace Engine.Systems
{
    /// <summary>
    /// A base for the creation of custom game scripts. 
    /// </summary>
    public abstract class CustomScript : ScenarioObject, IScript
    {
        /// <summary>
        /// Uses the <paramref name="manager"/> object to declare the models used in this scenario. 
        /// </summary>
        public virtual void LoadModels(ModelManager manager) { }

        /// <summary>
        /// The method executed when the game has started. 
        /// </summary>
        public virtual void GameStart() { }

        /// <summary>
        /// The method executed when a player has joined the game. 
        /// </summary>
        /// <param name="pl">The player who joined the game. </param>
        public virtual void OnPlayerJoined(Player pl) { }

        /// <summary>
        /// The method executed when a hero is created. 
        /// </summary>
        /// <param name="hero"></param>
        public virtual void OnHeroSpawned(Hero hero) { }

        /// <summary>
        /// NYI
        /// </summary>
        /// <param name="unit"></param>
        public virtual void OnUnitAdded(Unit unit) { }

        /// <summary>
        /// NYI
        /// </summary>
        /// <param name="d"></param>
        public virtual void OnDoodadAdded(Doodad d) { }

        /// <summary>
        /// NYI
        /// </summary>
        /// <param name="d"></param>
        public virtual void OnUnitDeath(Unit unit) { }
    }
}
