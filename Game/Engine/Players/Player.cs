using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using Shanism.Engine.Players;
using Shanism.Engine.Systems.Orders;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using Shanism.Common.Message.Network;
using Shanism.Common.Message.Server;
using Shanism.Common.Objects;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Engine
{
    /// <summary>
    /// Represents a player connected to the game
    /// <para/>
    /// There are 2 default players for all NPC characters, 
    /// see <see cref="NeutralAggressive"/> and <see cref="NeutralFriendly"/>. 
    /// </summary>
    public class Player : IPlayer
    {
        /// <summary>
        /// The neutral aggressive NPC player. Attacks players on sight. 
        /// </summary>
        public static Player NeutralAggressive { get; } = new Player("Neutral Aggressive");

        /// <summary>
        /// The neutral friendly NPC player. It's just chilling. 
        /// </summary>
        public static Player NeutralFriendly { get; } = new Player("Neutral Friendly");



        internal readonly ConcurrentSet<Unit> controlledUnits = new ConcurrentSet<Unit>();

        internal readonly ConcurrentSet<Entity> objectsSeen = new ConcurrentSet<Entity>();

        Hero _mainHero;


        internal ShanoReceptor Receptor { get; }

        /// <summary>
        /// Gets the identifier of this player. 
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name { get; }


        #region Events
        /// <summary>
        /// The event raised whenever this player's main hero changes. 
        /// </summary>
        public event Action<Hero> MainHeroChanged;

        /// <summary>
        /// The event raised whenever this player sees an object. 
        /// </summary>
        public event Action<Entity> ObjectSeen;

        /// <summary>
        /// The event raised whenever an object stops being visible to this player. 
        /// </summary>
        public event Action<Entity> ObjectUnseen;
        #endregion


        #region Property Shortcuts

        /// <summary>
        /// Gets the player's hero, if he has one. 
        /// </summary>
        public Hero MainHero => _mainHero;

        /// <summary>
        /// Gets all objects visible by this player. 
        /// </summary>
        public IEnumerable<Entity> VisibleObjects => objectsSeen;

        /// <summary>
        /// Gets whether the player has a hero. 
        /// </summary>
        public bool HasHero => (MainHero != null);

        /// <summary>
        /// Gets all units controlled by the player. 
        /// </summary>
        internal IEnumerable<IUnit> ControlledUnits => controlledUnits;

        /// <summary>
        /// Gets whether this player is the neutral aggressive player (see <see cref="NeutralAggressive"/>). 
        /// </summary>
        public bool IsNeutralAggressive => (this == NeutralAggressive);

        /// <summary>
        /// Gets whether this player is the neutral friendly player (see <see cref="NeutralFriendly"/>). 
        /// </summary>
        public bool IsNeutralFriendly => (this == NeutralFriendly);

        /// <summary>
        /// Gets whether this player is an actual human player. 
        /// </summary>
        public bool IsHuman => (Receptor != null);
        #endregion



        #region Constructors
        /// <summary>
        /// Creates a new human player from the given receptor. 
        /// </summary>
        /// <param name="receptor"></param>
        /// <param name="name"></param>
        internal Player(ShanoReceptor receptor, string name) : this(name)
        {
            if (receptor == null) throw new ArgumentNullException(nameof(receptor));

            Receptor = receptor;
        }
        
        /// <summary>
        /// Creates a new computer player with the given name. 
        /// </summary>
        /// <param name="name"></param>
        Player(string name)
        {
            Id = GenericId<Player>.GetNew();
            Name = name;
        }
        #endregion


        /// <summary>
        /// Gets whether the given player is an enemy of this player. 
        /// Currently all players are friends. 
        /// </summary>
        public bool IsEnemyOf(Player p)
        {
            var oneIsPlayer = (p.IsHuman || this.IsHuman);
            var oneIsAggressive = (p.IsNeutralAggressive || this.IsNeutralAggressive);
            var bothAreAggressive = (p.IsNeutralAggressive && this.IsNeutralAggressive);
            return (oneIsPlayer && oneIsAggressive) || bothAreAggressive;
        }

        /// <summary>
        /// Gets whether the given unit is an enemy of this player. 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsEnemyOf(Unit u)
        {
            return IsEnemyOf(u.Owner);
        }


        public void SetMainHero(Hero h)
        {
            if (HasHero)
                throw new Exception("Player already has a hero!");

            _mainHero = h;
            MainHeroChanged?.Invoke(_mainHero);
        }


        /// <summary>
        /// Adds a unit owned by this player to the player's list. 
        /// </summary>
        /// <param name="unit"></param>
        internal void AddControlledUnit(Unit unit)
        {
            //update the current player, call ObjectSeen
            controlledUnits.TryAdd(unit);
            if(objectsSeen.TryAdd(unit))
                ObjectSeen?.Invoke(unit);

            //register events
            unit.ObjectSeen += ownedUnit_ObjectSeen;
            unit.ObjectUnseen += ownedUnit_ObjectUnseen;
            unit.Death += ownedUnit_Death;
        }

        void ownedUnit_Death(Events.UnitDyingArgs e)
        {
            foreach(var obj in objectsSeen)
                if(!obj.seenByUnits.Any(u => u.Owner == this && u != e.DyingUnit) && objectsSeen.TryRemove(obj))
                    ObjectUnseen?.Invoke(obj);
        }

        void ownedUnit_ObjectUnseen(Entity obj)
        {
            //if noone else can see this unit, remove it
            if ((obj as Unit)?.Owner != this && !obj.seenByUnits.Any(u => u.Owner == this) && objectsSeen.TryRemove(obj))
                    ObjectUnseen?.Invoke(obj);
        }

        //fired whenever an owned unit sees an object. 
        void ownedUnit_ObjectSeen(Entity obj)
        {
            if (objectsSeen.TryAdd(obj))
                    ObjectSeen?.Invoke(obj);
        }
    }
}
