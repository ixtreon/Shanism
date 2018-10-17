using Shanism.Common;
using Shanism.Engine.Systems;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine
{
    /// <summary>
    /// A game as seen by the scripting subsystem. 
    /// Provides access to the current scenario, in-game map, connected players and other stuff.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets the current game map.
        /// </summary>
        IGameMap Map { get; }

        /// <summary>
        /// Gets the currently loaded scenario.
        /// </summary>
        Scenario Scenario { get; }

        /// <summary>
        /// Gets all human players currently in the game.
        /// </summary>
        IReadOnlyList<IPlayer> Players { get; }

        /// <summary>
        /// Gets the time elapsed since the start of the game.
        /// </summary>
        TimeSpan GameTime { get; }

        /// <summary>
        /// Sends a message to all players in the game.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(string message);

        /// <summary>
        /// Sends a message to a specified player.
        /// </summary>
        /// <param name="player">The player to send the message to.</param>
        /// <param name="message">The message to send.</param>
        void SendMessageToPlayer(IPlayer player, string message);
    }
}
