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

namespace Engine
{
    /// <summary>
    /// Represents a player connected to the game
    /// There are 2 players for all NPC characters, 
    /// see <see cref="NeutralAggressive"/> and <see cref="NeutralFriendly"/>. 
    /// </summary>
    public class Player : IPlayer, IReceptor
    {
        public static int LastPlayerId = 0;

        /// <summary>
        /// The neutral aggressive NPC player. Attacks players on sight. 
        /// </summary>
        public static readonly Player NeutralAggressive = new Player("Neutral Aggressive");

        /// <summary>
        /// The neutral friendly NPC player. It's just chilling. 
        /// </summary>
        public static readonly Player NeutralFriendly = new Player("Neutral Friendly");



        /// <summary>
        /// The engine this player is part of. 
        /// </summary>
        readonly ShanoEngine Engine;

        /// <summary>
        /// Gets the client handle of this player. 
        /// </summary>
        readonly IClient InputDevice;


        /// <summary>
        /// All units controlled by the player. 
        /// </summary>
        readonly HashSet<Unit> controlledUnits = new HashSet<Unit>();
        
        /// <summary>
        /// Gets the id of this player. 
        /// </summary>
        public readonly int PlayerId;

        /// <summary>
        /// Gets the player's hero, if he has one. 
        /// </summary>
        public Hero MainHero { get; private set; }

        /// <summary>
        /// Gets the name of the player. 
        /// </summary>
        public string Name { get; private set; }

        #region IReceptor Implementation
        public event Action<IGameObject> ObjectSeen;

        public event Action<IGameObject> ObjectUnseen;

        public event Action<IUnit, string> AnyUnitAction;

        public event Action<HandshakeReplyMessage> HandshakeReplied;

        public event Action<PlayerStatusMessage> MainHeroChanged;

        public event Action<MapReplyMessage> MapChunkReceived;
        #endregion

        /// <summary>
        /// Gets whether the player has a hero. 
        /// </summary>
        public bool HasHero { get { return MainHero != null; } }

        public bool Connected { get { return true; } }

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

        internal IEnumerable<IUnit> ControlledUnits { get { return controlledUnits; } }

        public IEnumerable<IGameObject> VisibleObjects
        {
            get { return controlledUnits
                    .SelectMany(u => u.VisibleObjects)
                    .Concat(controlledUnits)
                    .Distinct(); }
        }

        internal void SendHandshake(bool isSuccessful)
        {
            HandshakeReplied(new HandshakeReplyMessage(isSuccessful));
        }

        /// <summary>
        /// Gets the custom camera position. Undefined if there is a hero. 
        /// </summary>
        public Vector CameraPosition
        {
            get { return MainHero?.Position ?? Vector.Zero; }
        }

        Player(string name)
        {
            PlayerId = Interlocked.Increment(ref LastPlayerId);
            Name = Name;
        }

        public Player(ShanoEngine engine, IClient inputDevice)
            : this(inputDevice.Name)
        {
            PlayerId = Interlocked.Increment(ref LastPlayerId);
            Name = inputDevice.Name;

            Engine = engine;

            InputDevice = inputDevice;

            InputDevice.ActionActivated += inputDevice_ActionActivated;
            InputDevice.MapRequested += InputDevice_MapRequested;
            InputDevice.MovementStateChanged += inputDevice_MovementStateChanged;
            InputDevice.HandshakeInit += inputDevice_HandshakeInit;
        }

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

            MapChunkReceived(new MapReplyMessage(chunkId, chunkData));
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

            MainHeroChanged(new PlayerStatusMessage(h));

            //fire custom scripts
            Engine.Scenario.RunScripts(s => s.OnHeroSpawned(h));    //should remove or change to 'OnPlayerMainHeroChanged' ...
        }

        public void RegisterAction(ActionMessage msg)
        {
            if (MainHero != null)
                MainHero.OnAction(msg);
        }

        /// <summary>
        /// Gets whether this player is an enemy to the given player. 
        /// </summary>
        public bool IsEnemy(Player p)
        {
            var oneIsPlayer = (p.IsHuman || this.IsHuman);
            var oneIsAggressive = (p.IsNeutralAggressive || this.IsNeutralAggressive);
            return oneIsPlayer && oneIsAggressive;
        }

        /// <summary>
        /// Gets whether the given unit is an enemy to this player. 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public bool IsEnemy(Unit u)
        {
            return IsEnemy(u.Owner);
        }
        
        internal void OnObjectVisionRange(RangeArgs<GameObject> args)
        {
            if (!this.IsHuman)
                return;
            if (args.EventType == Maps.EventType.EntersRange)
            {
                ObjectSeen(args.TriggerObject);
            }
            else
            {
                ObjectUnseen(args.TriggerObject);
            }
        }
        

        internal void AddControlledUnit(Unit unit)
        {
            controlledUnits.Add(unit);

            ObjectSeen?.Invoke(unit);
        }

        public void UpdateServer(int msElapsed)
        {
            Engine.Update(msElapsed);
        }
    }
}
