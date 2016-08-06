using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// The game as seen by a custom scenario object. 
    /// Keeps track of the players connected to the game. 
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets all human players currently in the game.
        /// </summary>
        IEnumerable<IPlayer> Players { get; }

        /// <summary>
        /// Gets the number of human players currently in the game.
        /// </summary>
        int PlayerCount { get; }

        /// <summary>
        /// Sends a system message to all currently connected players.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        void SendSystemMessage(string msg);

        /// <summary>
        /// Sends a system message to the specified player.
        /// </summary>
        /// <param name="pl">The player to send the message to.</param>
        /// <param name="msg">The message to send.</param>
        void SendSystemMessage(IPlayer pl, string msg);
    }
}
