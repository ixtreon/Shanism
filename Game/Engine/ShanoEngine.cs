using Engine.Maps;
using Engine.Systems;
using IO;
using IO.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using IO.Message;

namespace Engine
{
    /// <summary>
    /// The game engine lies here. 
    /// </summary>
    public class ShanoEngine : IEngine
    {
        /// <summary>
        /// The frames per second we aim to run at. 
        /// </summary>
        const int FPS = 60;


        /// <summary>
        /// Gets whether this game server is running. 
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the thread running the game loop. 
        /// </summary>
        public Thread GameThread { get; private set; }

        private HashSet<MapChunkId> generatedChunks = new HashSet<MapChunkId>();

        /// <summary>
        /// The current world map containing the terrain info. 
        /// </summary>
        internal ITerrainMap TerrainMap { get; private set; }



        /// <summary>
        /// A list of all players currently in game. 
        /// </summary>
        internal ConcurrentBag<Player> Players { get; } = new ConcurrentBag<Player>();

        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal EntityMap EntityMap { get; }

        /// <summary>
        /// Gets the scenario this engine is playing. 
        /// </summary>
        internal Scenario Scenario { get; }

        /// <summary>
        /// Gets the time elapsed since the game started. 
        /// </summary>
        public double GameTime { get; private set; }

        /// <summary>
        /// Gets whether this engine is open for online play. 
        /// </summary>
        internal bool IsOnline { get; private set; }



        internal RangeEventSystem RangeSystem { get; }
        internal VisionSystem VisionSystem { get; }

        internal Network.LServer NetworkServer { get; private set; }

        internal ObjectMap Map
        {
            get
            {
                return EntityMap.Map;
            }
        }

        public ShanoEngine(int mapSeed, string scenarioDir)
        {

            //compile the scenario
            try
            {
                Scenario = Scenario.Load(Path.GetFullPath(scenarioDir));
            }
            catch(Exception e)
            {
                throw e;
            }

            ScenarioObject.Init(this);


            //create the terrain, object map from the scenario. 
            TerrainMap = Maps.Terrain.MapGod.Create(Scenario.MapConfig, mapSeed);
            EntityMap = new EntityMap();

            //create systems
            RangeSystem = new RangeEventSystem(this);
            VisionSystem = new VisionSystem(this, RangeSystem);

            //run scripts
            Scenario.RunScripts(cs => cs.OnGameStart());
        }

        #region IEngine implementation

        public INetReceptor AcceptClient(IClient c)
        {
            // TODO: do some checks on player join?x

            var pl = new Player(this, c);
            return pl;
        }

        public void StartPlaying(INetReceptor rec)
        {
            var pl = rec as Player;
            if (pl == null)
                throw new ArgumentException(nameof(rec), "You should supply a {0} of type {1} as returned by this engine!".F(nameof(IReceptor), nameof(Player)));

            pl.SendHandshake(true);

            AddPlayer(pl);
        }
        #endregion

        #region Server Controls
        /// <summary>
        /// Starts the game server by executing the main game loop. 
        /// </summary>
        /// <param name="spawnNewThread">If true spawns a new thread to run the loop on. Otherwise uses the current thread. </param>
        public void Start(bool spawnNewThread = false)
        {
            if (IsRunning)
                return;
            IsRunning = true;


            if (spawnNewThread)
            {
                // start the update thread
                GameThread = new Thread(mainLoop) { IsBackground = true };
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
        }
        #endregion

        /// <summary>
        /// Adds the given player to the game. 
        /// </summary>
        /// <param name="pl"></param>
        public void AddPlayer(Player pl)
        {
            Players.Add(pl);

            Scenario.RunScripts(s => s.OnPlayerJoined(pl));
        }


        /// <summary>
        /// Runs the game loop. 
        /// </summary>
        void mainLoop()
        {
            int frameStartTime, drawTime = 0;
            while (IsRunning)
            {
                var toSleep = 1000 / FPS - drawTime;    //to sleep
                var isThrottled = toSleep < 0;          //or not to sleep?

                if (!isThrottled)
                    Thread.Sleep(toSleep);
                else
                    Console.WriteLine("Warning: Server updating too slow!");

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

            GameTime += msElapsed;

            //update range system before locations 
            //so new units get properly detected
            RangeSystem.Update(msElapsed);
            VisionSystem.Update(msElapsed);


            //update the map along with all units. 
            EntityMap.Update(msElapsed);

            foreach (var p in Players)
                p.Update(msElapsed);
        }


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
            TerrainMap.GetMap(rect, ref tileMap);
            if (!generatedChunks.Contains(chunk))
            {
                lock(generatedChunks)
                    generatedChunks.Add(chunk);

                foreach (var dood in TerrainMap.GetNativeDoodads(rect))
                {
                    EntityMap.Add(dood);
                }
            }
        }
    }
}
