using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Engine.Players
{
    /// <summary>
    /// Represents a faction (such as a tribe of gnolls) or a player alliance. 
    /// </summary>
    class Alliance
    {
        /// <summary>
        /// Gets the name of this alliance.
        /// </summary>
        public string Name { get; }


        readonly HashSet<Player> PlayerMembers = new HashSet<Player>();

        public Alliance(string title)
        {
            this.Name = title;

            PlayerMembers.Add(Player.Friendly);
        }

        /// <summary>
        /// Adds the specified player to this alliance. 
        /// A player can be part of multiple alliances.
        /// </summary>
        public void Add(Player pl) => PlayerMembers.Add(pl);


        /// <summary>
        /// Gets whether the given player is an enemy of this alliance. 
        /// </summary>
        public bool IsEnemy(Player pl)
            => !PlayerMembers.Contains(pl);

        /// <summary>
        /// Gets whether the given player is allied to this alliance. 
        /// </summary>
        public bool IsFriend(Player pl)
            => PlayerMembers.Contains(pl);
    }
}
