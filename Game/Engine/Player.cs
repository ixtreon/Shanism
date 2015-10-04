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
    /// Represents any type of player inside the game. 
    /// There are two static members of this class to represent all NPC units. 
    /// 
    /// If the player does not have a main hero he is to be spectating. 
    /// </summary>
    public class Player : IGameReceptor, IPlayer, INetworkReceptor
    {
        const double SpectatorMoveSpeed = 5;

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
        readonly IGameClient InputDevice;


        Vector customCameraPosition;

        /// <summary>
        /// All units controlled by the player. 
        /// </summary>
        readonly HashSet<Unit> controlledUnits = new HashSet<Unit>();

        /// <summary>
        /// Gets or sets the movement state of the main hero. 
        /// </summary>
        public MovementState MovementState { get; set; }

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

        /// <summary>
        /// The event raised whenever a chunk is received. 
        /// </summary>
        public event Action<MapChunkId, TerrainType[,]> ChunkReceived;


        public event Action<IUnit> UnitInVisionRange;

        public event Action<IGameObject> ObjectInVisionRange;

        public event Action<IGameObject> ObjectLeavesVisionRange;

        public event Action<IHero> MainHeroChanged;


        //nyi
        /// <summary>
        /// The event raised whenever an object 
        /// </summary>
        public event Action<IGameObject> ObjectMoved;

        //nyi
        /// <summary>
        /// The event raised whenever a visible unit performs an action. 
        /// </summary>
        public event Action<IUnit> UnitActionPerformed;

        //nyi
        /// <summary>
        /// The event raised whenever a visible unit changes its movement state. 
        /// </summary>
        public event Action<IUnit> UnitMoveStateChanged;


        /// <summary>
        /// Gets whether the player has a hero. 
        /// </summary>
        public bool HasHero { get { return MainHero != null; } }

        public bool Connected { get { return true; } }

        IHero IGameReceptor.MainHero { get { return MainHero; } }

        public bool IsNeutralAggressive {  get { return this == NeutralAggressive; } }

        public bool IsNeutralFriendly {  get { return this == NeutralFriendly; } }

        public bool IsPlayer {  get { return !IsNeutralAggressive && !IsNeutralFriendly; } }

        internal IEnumerable<IUnit> ControlledUnits { get { return controlledUnits; } }

        public IEnumerable<IGameObject> VisibleObjects
        {
            get { return controlledUnits
                    .SelectMany(u => u.VisibleObjects)
                    .Concat(controlledUnits)
                    .Distinct(); }
        }

        /// <summary>
        /// Gets the custom camera position. Undefined if there is a hero. 
        /// </summary>
        public Vector CameraPosition
        {
            get { return MainHero?.Position ?? customCameraPosition; }
        }

        Player(string name)
        {
            PlayerId = Interlocked.Increment(ref LastPlayerId);
            Name = Name;
        }

        public Player(ShanoEngine engine, IGameClient inputDevice)
            : this(inputDevice.Name)
        {
            PlayerId = Interlocked.Increment(ref LastPlayerId);
            Name = inputDevice.Name;

            Engine = engine;
            InputDevice = inputDevice;
        }


        /// <summary>
        /// Listens to the input device for any updates. 
        /// </summary>
        public void Update(int msElapsed)
        {
            updateMovement(msElapsed);
        }

        void IGameReceptor.Update(int msElapsed)
        {
            Engine.Update(msElapsed);
        }

        public void SetMainHero(Hero h)
        {
            if (HasHero)
                throw new Exception("Player already has a hero!");

            MainHero = h;

            //fire custom scripts
            Engine.Scenario.RunScripts(s => s.OnHeroSpawned(h));    //should remove or change to 'OnPlayerMainHeroChanged' ...
        }

        public void RegisterAction(ActionMessage msg)
        {
            if (MainHero != null)
                MainHero.OnAction(msg);
        }

        public bool IsEnemy(Player p)
        {
            var oneIsPlayer = (p.IsPlayer || this.IsPlayer);
            var oneIsAggressive = (p.IsNeutralAggressive || this.IsNeutralAggressive);
            return oneIsPlayer && oneIsAggressive;
        }

        public bool IsEnemy(Unit u)
        {
            return IsEnemy(u.Owner);
        }

        public async void RequestChunk(MapChunkId chunk)
        {
            var mapTiles = new TerrainType[MapChunkId.ChunkSize.X, MapChunkId.ChunkSize.Y];
            await Task.Run(() =>
            {
                Engine.GetTiles(this, ref mapTiles, chunk);
            });

            if (ChunkReceived != null)
                ChunkReceived(chunk, mapTiles);
        }

        internal void OnObjectVisionRange(RangeArgs<GameObject> args)
        {
            if (args.EventType == Maps.EventType.EntersRange)
            {
                ObjectInVisionRange?.Invoke(args.TriggerObject);
            }
            else
            {
                ObjectLeavesVisionRange?.Invoke(args.TriggerObject);
            }
        }

        internal void raiseUnitInVisionRange(RangeArgs<Unit> args)
        {
            UnitInVisionRange?.Invoke(args.TriggerObject);
        }

        void updateMovement(int msElapsed)
        {
            //if no hero, allow spectating and ghost-walking
            if (MainHero == null)
            {
                customCameraPosition += MovementState.DirectionVector * SpectatorMoveSpeed * msElapsed / 1000;
                return;
            }

            //continue only if move state changed
            if (MainHero.MoveState == MovementState)
                return;

            //update the hero's current order
            if (MovementState.IsMoving)
                MainHero.SetOrder(new PlayerMoveOrder(MovementState));
            else
                MainHero.ClearOrder();
        }

        internal void AddUnit(Unit unit)
        {
            controlledUnits.Add(unit);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
