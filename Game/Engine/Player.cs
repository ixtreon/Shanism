using Engine.Entities;
using Engine.Entities.Objects;
using Engine.Systems.Orders;
using IO;
using IO.Common;
using IO.Message;
using IO.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Message.Server;
using System.Threading;
using IO.Objects;
using Engine.Events;
using Engine.Systems.Range;
using System.Collections.Concurrent;
using IO.Util;
using ProtoBuf;
using System.IO;

namespace Engine
{
    /// <summary>
    /// Represents a player connected to the game
    /// <para/>
    /// There are 2 default players for all NPC characters, 
    /// see <see cref="NeutralAggressive"/> and <see cref="NeutralFriendly"/>. 
    /// </summary>
    public class Player : IPlayer, INetReceptor, IReceptor
    {
        static int LastPlayerId = 0;

        /// <summary>
        /// The neutral aggressive NPC player. Attacks players on sight. 
        /// </summary>
        public static Player NeutralAggressive { get; } = new Player("Neutral Aggressive");

        /// <summary>
        /// The neutral friendly NPC player. It's just chilling. 
        /// </summary>
        public static Player NeutralFriendly { get; } = new Player("Neutral Friendly");



        /// <summary>
        /// The engine this player is part of. 
        /// </summary>
        ShanoEngine Engine { get; }

        /// <summary>
        /// Gets the client handle of this player. 
        /// </summary>
        IClient InputDevice { get; }

        /// <summary>
        /// Gets the id of this player. 
        /// </summary>
        public int PlayerId { get; }

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// All units controlled by the player. 
        /// </summary>
        ConcurrentSet<Unit> controlledUnits { get; } = new ConcurrentSet<Unit>();

        ConcurrentSet<GameObject> objectsSeen { get; } = new ConcurrentSet<GameObject>();

        /// <summary>
        /// Gets the player's hero, if he has one. 
        /// </summary>
        public Hero MainHero { get; private set; }


        #region IReceptor Implementation
        public event Action<IOMessage> MessageSent;


        public event Action<IUnit, string> AnyUnitAction;

        internal void SendMessage(IOMessage msg)
        {
            MessageSent(msg);
        }
        #endregion


        /// <summary>
        /// Gets whether the player has a hero. 
        /// </summary>
        public bool HasHero { get { return MainHero != null; } }

        /// <summary>
        /// Gets all units controlled by the player. 
        /// </summary>
        internal IEnumerable<IUnit> ControlledUnits { get { return controlledUnits; } }

        /// <summary>
        /// Gets whether this player is the neutral aggressive player. 
        /// </summary>
        public bool IsNeutralAggressive {  get { return this == NeutralAggressive; } }

        /// <summary>
        /// Gets whether this player is the neutral friendly player. 
        /// </summary>
        public bool IsNeutralFriendly {  get { return this == NeutralFriendly; } }

        /// <summary>
        /// Gets whether this player is an actual human player. 
        /// </summary>
        public bool IsHuman {  get { return !IsNeutralAggressive && !IsNeutralFriendly; } }

        /// <summary>
        /// Gets all objects visible by this player. 
        /// </summary>
        public IEnumerable<IGameObject> VisibleObjects
        {
            get { return objectsSeen; }
        }


        #region Constructors
        private Player(string name)
        {
            PlayerId = Interlocked.Increment(ref LastPlayerId);
            Name = name;
        }

        public Player(ShanoEngine engine, IClient inputDevice)
            : this(inputDevice.Name)
        {
            Engine = engine;
            InputDevice = inputDevice;

            InputDevice.ActionActivated += inputDevice_ActionActivated;
            InputDevice.MapRequested += InputDevice_MapRequested;
            InputDevice.MovementStateChanged += inputDevice_MovementStateChanged;
            InputDevice.HandshakeInit += inputDevice_HandshakeInit;
        }
        #endregion


        #region GameClient listeners
        void inputDevice_ActionActivated(ActionMessage obj)
        {
            MainHero.TryCastAbility(obj);
        }

        void inputDevice_HandshakeInit()
        {
            //run scripts
        }

        void inputDevice_MovementStateChanged(MoveMessage msg)
        {
            if (MainHero == null)
                return;

            var newState = msg.Direction;
            if (newState.IsMoving)
                MainHero.SetOrder(new PlayerMoveOrder(msg.Direction));
            else
                MainHero.ClearOrder();
        }

        async void InputDevice_MapRequested(MapRequestMessage msg)
        {
            var chunkId = msg.Chunk;
            var chunkData = new TerrainType[MapChunkId.ChunkSize.X * MapChunkId.ChunkSize.Y];
            await Task.Run(() =>
            {
                Engine.GetTiles(this, ref chunkData, chunkId);
            });

            SendMessage(new MapReplyMessage(chunkId, chunkData));
        }

        #endregion

        public void Update(int msElapsed)
        {
            
        }


        public void SetMainHero(Hero h)
        {
            if (HasHero)
                throw new Exception("Player already has a hero!");

            MainHero = h;

            SendMessage(new PlayerStatusMessage(h));

            //fire custom scripts
            Engine.Scenario.RunScripts(s => s.OnHeroSpawned(h));    //should remove or change to 'OnPlayerMainHeroChanged' ...
        }
        

        /// <summary>
        /// Gets whether the given player is an enemy of this player. 
        /// Currently all players are friends. 
        /// </summary>
        public bool IsEnemyOf(Player p)
        {
            var oneIsPlayer = (p.IsHuman || this.IsHuman);
            var oneIsAggressive = (p.IsNeutralAggressive || this.IsNeutralAggressive);
            return (oneIsPlayer && oneIsAggressive) || (p.IsNeutralAggressive && this.IsNeutralAggressive);
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

        public void SendHandshake(bool isSuccessful)
        {
            var sc = Engine.Scenario;

            SendMessage(new HandshakeReplyMessage(isSuccessful, sc.GetBytes(), sc.ZipContent()));
        }

        /// <summary>
        /// Adds a unit owned by this player to the player's list. 
        /// </summary>
        /// <param name="unit"></param>
        internal void AddControlledUnit(Unit unit)
        {
            //update the current player, call ObjectSeen
            controlledUnits.TryAdd(unit);
            if(objectsSeen.TryAdd(unit))        //should always be true, but hey..
                sendObjectSeenMessage(unit);

            //register events
            unit.ObjectSeen += ownedUnit_ObjectSeen;
            unit.ObjectUnseen += ownedUnit_ObjectUnseen;
        }

        void ownedUnit_ObjectUnseen(GameObject obj)
        {
            if (!obj.SeenBy.Any(u => u.Owner == this))
            {
                if (objectsSeen.TryRemove(obj))
                    sendObjectUnseenMessage(obj);
            }
        }

        //fired whenever an owned unit sees an object. 
        void ownedUnit_ObjectSeen(GameObject obj)
        {
            if (objectsSeen.TryAdd(obj))
                sendObjectSeenMessage(obj);
        }


        void sendObjectSeenMessage(GameObject obj)
        {
            if (!IsHuman) return;
            SendMessage(new ObjectSeenMessage(obj));
        }

        void sendObjectUnseenMessage(GameObject obj)
        {
            if (!IsHuman) return;
            SendMessage(new ObjectUnseenMessage(obj.Guid));
        }

        public void UpdateServer(int msElapsed)
        {
            Engine.Update(msElapsed);
        }

        public string GetPerfData()
        {
            return Engine.GetPerfData();
        }
    }
}
