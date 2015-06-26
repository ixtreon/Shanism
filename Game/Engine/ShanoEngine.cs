using Engine.Objects;
using Engine.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IO;
using Engine.Systems;
using IO.Common;
using System.Diagnostics;
using Engine.Objects.Game;
using System.Security;
using System.IO;
using IO.Objects;

namespace Engine
{
    /// <summary>
    /// The game engine lies here. 
    /// </summary>
    public class ShanoEngine : IGameEngine
    {
        public static ShanoEngine Current { get; private set; }

        /// <summary>
        /// The frames per second we aim to run at. 
        /// </summary>
        const int FPS = 60;


        /// <summary>
        /// Gets whether this game server is running. 
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the thread running the game loop, if <see cref="HasOwnThread"/> is true. 
        /// </summary>
        public Thread GameThread { get; private set; }

        /// <summary>
        /// Gets whether this game server is running on a separate thread. 
        /// </summary>
        public bool HasOwnThread
        {
            get { return GameThread != null; }
        }

        /// <summary>
        /// The current world map containing the terrain info. 
        /// </summary>
        internal RandomMap WorldMap { get; private set; }

        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal GameMap GameMap { get; private set; }


        /// <summary>
        /// A list of all players currently in game. 
        /// </summary>
        internal List<Player> Players;


        internal Scenario Scenario { get; private set; }


        internal Network.LServer NetworkServer { get; private set; }

        internal bool IsOnline { get; private set; }

        public ShanoEngine(int mapSeed)
        {

            // allow only one instance of the server. 
            // an ugly hack..
            if (Current != null)
                throw new Exception("Please run only one instance of the server!");

            Current = this;

            this.Players = new List<Player>();

            this.GameMap = new GameMap();

            this.WorldMap = new RandomMap(mapSeed);

            Scenario = new Scenario(Path.GetFullPath(@"DefaultScenario"));
            if (!Scenario.TryCompile())
                throw new Exception("Unable to compile the scenario!");

            //run startup scripts
            Scenario.RunScripts(s => s.GameStart());
        }

        event Action<IUnit, OrderType> IGameEngine.AnyUnitOrderChanged
        {
            add { Unit.AnyOrderChanged += (u, o) => value(u, o.Type); }
            remove { Unit.AnyOrderChanged += (u, o) => value(u, o.Type); }
        }

        event Action<IUnit, IUnit> IGameEngine.AnyUnitEntersVisionRange
        {
            add { Unit.AnyUnitInVisionRange += (args) => value(args.OriginUnit, args.TriggerUnit); }
            remove { Unit.AnyUnitInVisionRange -= (args) => value(args.OriginUnit, args.TriggerUnit); }
        }


        /// <summary>
        /// Starts the game server by executing the main game loop. 
        /// </summary>
        /// <param name="newThread">If set to true starts a new thread to run the loop on. </param>
        public void Start(bool newThread = false)
        {
            if (IsRunning)
                return;
            IsRunning = true;


            if (newThread)
            {
                // start the update thread
                GameThread = new Thread(mainLoop)
                {
                    IsBackground = true
                };
                GameThread.Start();
            }
            else
            {
                mainLoop();
            }
        }

        /// <summary>
        /// Stops the game server. 
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Adds the given player to the game. 
        /// </summary>
        /// <param name="p"></param>
        public void AddPlayer(Player p)
        {
            this.Players.Add(p);

            //run scripts
            Scenario.RunScripts(s => s.OnPlayerJoined(p));
            
            ////run scripts -> not here though!
            //Scenario.RunScripts(s => s.OnHeroSpawned(p.MainHero));
        }

        /// <summary>
        /// Allows for network connections to be established to this game. 
        /// </summary>
        /// <param name="port"></param>
        public void OpenToNetwork()
        {
            if (IsOnline)
            {
                Console.WriteLine("Trying to open the server for network play but it is already online!");
                return;
            }

            IsOnline = true;
            NetworkServer = new Network.LServer(this);
            NetworkServer.ClientConnectHandler = handleNetClient;
        }

        private IGameReceptor handleNetClient(IGameClient c)
        {
            var playerName = c.Name;

            //TODO: do some checks
            
            var pl = new Player(this, c);

            AddPlayer(pl);

            return pl;
        }

        /// <summary>
        /// Starts the basic game loop. 
        /// </summary>
        private void mainLoop()
        {
            int frameStartTime, drawTime = 0;
            while (IsRunning)
            {
                var toSleep = 1000 / FPS - drawTime;    //to sleep
                var isThrottled = toSleep < 0;          //or not to sleep?

                if (!isThrottled)
                    Thread.Sleep(toSleep);
                else
                    Console.WriteLine("Warning: Updating too slow!");

                frameStartTime = Environment.TickCount;
                this.Update(1000 / FPS);
                drawTime = Environment.TickCount - frameStartTime;
            }
        }

        /// <summary>
        /// Performs a single update of the game state. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last call of this function. </param>
        public void Update(int msElapsed)
        {
            if (IsOnline)
                NetworkServer.Update(msElapsed);

            GameMap.Update(msElapsed);

            foreach (var p in Players)
                p.Update(msElapsed);


        }

        /// <summary>
        /// Returns all objects in proximity of the given hero. 
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        public IEnumerable<IGameObject> GetNearbyObjects(Player pl)
        {
            var pos = pl.CameraPosition;
            var range = (Vector)IO.Constants.Client.WindowSize / 2 + 1;

            return GameMap.GetObjectsInRect(pos - range, range * 2);
        }


        private HashSet<MapChunkId> generatedChunks = new HashSet<MapChunkId>();

        /// <summary>
        /// Writes the map data for the given rectangle in the given array. 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="tileMap"></param>
        /// <param name="rect"></param>
        public void GetTiles(Player pl, ref TerrainType[,] tileMap, MapChunkId chunk)
        {
            //TODO: check if chunk is valid for player pl

            var rect = new Rectangle(chunk.BottomLeft, MapChunkId.ChunkSize);
            WorldMap.GetMap(rect, ref tileMap);
            if (!generatedChunks.Contains(chunk))
            {
                lock(generatedChunks)
                    generatedChunks.Add(chunk);

                foreach (var dood in WorldMap.GenerateNativeDoodads(rect))
                {
                    GameMap.Doodads.Add(dood);
                }
            }
        }


    }
}
