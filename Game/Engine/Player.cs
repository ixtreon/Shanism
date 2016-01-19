using Engine.Objects;
using Engine.Objects.Game;
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
using Engine.Systems.RangeEvents;
using System.Collections.Concurrent;
using IO.Util;

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

        public event Action<IGameObject> ObjectSeen;

        public event Action<IGameObject> ObjectUnseen;

        public event Action<IUnit, string> AnyUnitAction;
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

        /// <summary>
        /// Gets the custom camera position. Undefined if there is a hero. 
        /// </summary>
        public Vector CameraPosition
        {
            get { return MainHero?.Position ?? Vector.Zero; }
        }


        #region Constructors
        Player(string name)
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
            var chunkData = new TerrainType[MapChunkId.ChunkSize.X, MapChunkId.ChunkSize.Y];
            await Task.Run(() =>
            {
                Engine.GetTiles(this, ref chunkData, chunkId);
            });

            MessageSent(new MapReplyMessage(chunkId, chunkData));
        }

        #endregion

        /// <summary>
        /// Listens to the input device for any updates. 
        /// </summary>
        public void Update(int msElapsed)
        {
            
        }


        public void SetMainHero(Hero h)
        {
            if (HasHero)
                throw new Exception("Player already has a hero!");

            MainHero = h;

            MessageSent(new PlayerStatusMessage(h));

            //fire custom scripts
            Engine.Scenario.RunScripts(s => s.OnHeroSpawned(h));    //should remove or change to 'OnPlayerMainHeroChanged' ...
        }

        public void RegisterAction(ActionMessage msg)
        {
            if (MainHero != null)
                MainHero.OnAction(msg);
        }

        /// <summary>
        /// Gets whether the given player is an enemy of this player. 
        /// Currently all players are friends. 
        /// </summary>
        public bool IsEnemyOf(Player p)
        {
            var oneIsPlayer = (p.IsHuman || this.IsHuman);
            var oneIsAggressive = (p.IsNeutralAggressive || this.IsNeutralAggressive);
            return oneIsPlayer && oneIsAggressive;
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

            MessageSent(new HandshakeReplyMessage(isSuccessful, sc.GetBytes(), sc.ZipContent()));
        }

        /// <summary>
        /// Adds a unit owned by this player to the player's list. 
        /// </summary>
        /// <param name="unit"></param>
        internal void AddControlledUnit(Unit unit)
        {
            //update the current player
            controlledUnits.TryAdd(unit);
            if(objectsSeen.TryAdd(unit))
                ObjectSeen?.Invoke(unit);

            //register events
            unit.ObjectSeen += ownedUnit_ObjectSeen;
            unit.ObjectUnseen += ownedUnit_ObjectUnseen;

            
        }

        void ownedUnit_ObjectUnseen(GameObject obj)
        {
            if (!obj.SeenBy.Any(u => u.Owner == this))
            {
                if(objectsSeen.TryRemove(obj))
                    ObjectUnseen?.Invoke(obj);
            }
        }

        //fired whenever an owned unit sees an object. 
        void ownedUnit_ObjectSeen(GameObject obj)
        {
            if (objectsSeen.TryAdd(obj))
                ObjectSeen?.Invoke(obj);
        }

        public void UpdateServer(int msElapsed)
        {
            Engine.Update(msElapsed);
        }
    }
}
