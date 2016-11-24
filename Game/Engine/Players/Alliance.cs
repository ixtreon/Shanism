using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Engine.Players
{
    /// <summary>
    /// Represents a faction such as a tribe of gnolls or a bunch of players. 
    /// </summary>
    public class Alliance
    {

        readonly HashSet<Player> _players = new HashSet<Player>();


        /// <summary>
        /// Gets the name of this alliance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets all players that are part of this alliance.
        /// </summary>
        public IEnumerable<Player> Players => _players;

        public Alliance(string title)
        {
            this.Name = title;

            _players.Add(Player.Friendly);
        }

        /// <summary>
        /// Adds the given player to this alliance. 
        /// </summary>
        public void Add(Player pl)
        {
            if (pl.Alliance != null)
                pl.Alliance.Remove(pl);

            pl.Alliance = this;
            _players.Add(pl);
        }

        /// <summary>
        /// Removes the given player from this alliance.
        /// </summary>
        public void Remove(Player pl)
        {
            if (pl.Alliance == this)
            {
                pl.Alliance = null;
                _players.Remove(pl);
            }
        }

        /// <summary>
        /// Gets whether the given player is an enemy of this alliance. 
        /// </summary>
        public bool IsEnemy(Player pl)
            => !_players.Contains(pl);

        /// <summary>
        /// Gets whether the given player is allied to this alliance. 
        /// </summary>
        public bool IsFriend(Player pl)
            => _players.Contains(pl);
    }
}
