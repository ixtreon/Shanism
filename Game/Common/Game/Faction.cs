using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Game
{
    /// <summary>
    /// Represents a faction (such as a tribe of gnolls) or (TODO) a guild. 
    /// </summary>
    class Faction
    {
        public static readonly Faction Player = new Faction("Player");

        public static readonly Faction Neutral = new Faction("Neutral");
        public static readonly Faction Aggressive = new Faction("Aggressive");
        public static readonly Faction Friendly = new Faction("Friendly");

        public readonly string Name;


        //NYI
        readonly List<string> PlayerIds = new List<string>();
        readonly List<Faction> Friends = new List<Faction>();
        readonly List<Faction> Enemies = new List<Faction>();

        public Faction(string title)
        {
            this.Name = title;
        }
        

        /// <summary>
        /// Gets whether the given faction is an enemy of this one. 
        /// </summary>
        public bool IsEnemy(Faction f)
        {
            return (f != this) && f == Aggressive || this == Aggressive || Enemies.Contains(f);
        }

        /// <summary>
        /// Gets whether the given faction is allied to this one. 
        /// </summary>
        public bool IsFriend(Faction f)
        {
            return (f == this) || f == Friendly || this == Friendly || Friends.Contains(f);
        }
    }
}
