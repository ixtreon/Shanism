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

namespace Engine
{
    /// <summary>
    /// Represents a player connected to the game. 
    /// 
    /// If the player does not have a main hero he is considered to be spectating. 
    /// </summary>
    public class Player : IGameReceptor
    {
        public static readonly Player NeutralAggressive = new Player(0);
        public static readonly Player NeutralFriendly = new Player(1);

        public static int LastPlayerId = 1;

        readonly ShanoEngine Engine;


        Vector customCameraPosition;

        /// <summary>
        /// All units controlled by the player. 
        /// </summary>
        private HashSet<Unit> controlledUnits = new HashSet<Unit>();


        /// <summary>
        /// Gets or sets the movement state of the main hero. 
        /// </summary>
        public MovementState MovementState { get; set; }

        /// <summary>
        /// Gets the client handle for the player. 
        /// </summary>
        public readonly IGameClient InputDevice;

        public readonly int PlayerId;


        /// <summary>
        /// Gets the player's hero, if he has one. 
        /// </summary>
        public Hero MainHero { get; private set; }

        /// <summary>
        /// The event raised whenever a chunk is received. 
        /// </summary>
        public event Action<MapChunkId, TerrainType[,]> ChunkReceived;

        /// <summary>
        /// Gets whether the player has a hero. 
        /// </summary>
        public bool HasHero { get { return MainHero != null; } }

        //a player is always connected to the server
        public bool Connected { get { return true; } }

        IHero IGameReceptor.MainHero
        {
            get { return MainHero; }
        }

        /// <summary>
        /// Gets the custom camera position. Undefined if there is a hero. 
        /// </summary>
        public Vector CameraPosition
        {
            get { return MainHero?.Location ?? customCameraPosition; }
        }

        private Player(int id) { PlayerId = id; }

        public Player(ShanoEngine engine, IGameClient inputDevice)
        {
            PlayerId = Interlocked.Increment(ref LastPlayerId);

            this.Engine = engine;
            this.InputDevice = inputDevice;
        }

        /// <summary>
        /// Listens to the input device for any updates. 
        /// </summary>
        public void Update(int msElapsed)
        {
            updateMovement(msElapsed);
        }

        public void SetMainHero(Hero h)
        {
            if (HasHero)
                throw new Exception("Player already has a hero!");

            MainHero = h;
            Engine.GameMap.Units.Add(h);

            //fire custom scripts
            Engine.Scenario.RunScripts(s => s.OnHeroSpawned(h));    //should remove or change to 'OnPlayerMainHeroChanged' ...
        }

        private void updateMovement(int msElapsed)
        {
            //if no hero, allow spectating and ghost-walking
            if (MainHero == null)
            {
                const double ghostSpeed = 10;
                customCameraPosition += MovementState.DirectionVector * ghostSpeed * msElapsed / 1000;
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

        public void RegisterAction(ActionMessage p)
        {
            if (MainHero != null)
                MainHero.OnAction(p);
        }

        public IEnumerable<IGameObject> GetNearbyGameObjects()
        {
            return Engine.GetNearbyObjects(this);
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
    }
}
