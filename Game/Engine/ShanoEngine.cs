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
using IO.Performance;
using IO.Util;
using Engine.Network;

namespace Engine
{
    /// <summary>
    /// The game engine lies here. 
    /// </summary>
    public class ShanoEngine : IShanoEngine
    {
        /// <summary>
        /// The frames per second we aim to run at. 
        /// </summary>
        const int FPS = 60;


        ConcurrentSet<MapChunkId> generatedChunks { get; } = new ConcurrentSet<MapChunkId>();


        /// <summary>
        /// A list of all players currently in game. 
        /// </summary>
        internal ConcurrentBag<Player> Players { get; } = new ConcurrentBag<Player>();


        internal PerfCounter PerfCounter { get; } = new PerfCounter();


        /// <summary>
        /// Gets the scenario this engine is playing. 
        /// </summary>
        internal Scenario Scenario { get; }


        /// <summary>
        /// The current world map containing the terrain info. 
        /// </summary>
        internal ITerrainMap TerrainMap { get; private set; }


        /// <summary>
        /// Gets whether this game server is running. 
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the thread running the game loop. 
        /// </summary>
        public Thread GameThread { get; private set; }

        /// <summary>
        /// Gets the time elapsed since the game started. 
        /// </summary>
        public double GameTime { get; private set; }


        #region Systems

        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal MapSystem map { get; } = new MapSystem();

        NetworkSystem network { get; } = new NetworkSystem();


        List<GameSystem> systems { get; } = new List<GameSystem>();
        #endregion


        public ShanoEngine(int mapSeed, string scenarioDir)
        {
            //compile the scenario
            try
            {
                Scenario = Scenario.Load(Path.GetFullPath(scenarioDir));
            }
            catch (Exception e)
            {
                throw e;
            }

            //register the server as the parent of all objects. 
            ScenarioObject.Init(this);

            //create the terrain, object map from the scenario. 
            TerrainMap = Maps.Terrain.MapGod.Create(Scenario.MapConfig, mapSeed);

            //create all subsystems
            systems.Add((map = new MapSystem()));
            systems.Add(network = new NetworkSystem());

            //run scripts
            Scenario.RunScripts(cs => cs.OnGameStart());
        }

        #region IEngine implementation

        public INetReceptor AcceptClient(IShanoClient c)
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
            network.Start(this);
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
                //else
                //    Console.Write("T");

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
            PerfCounter.Reset();

            foreach (var sys in systems)
                sys.Update(msElapsed);


            GameTime += msElapsed;

            foreach (var p in Players)
                p.Update(msElapsed);

        }


        /// <summary>
        /// Writes the map data for the given rectangle in the given array. 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="tileMap"></param>
        /// <param name="rect"></param>
        public void GetTiles(Player pl, ref TerrainType[] tileMap, MapChunkId chunk)
        {
            //TODO: check if chunk is valid
            var rect = new Rectangle(chunk.BottomLeft, MapChunkId.ChunkSize);
            TerrainMap.GetMap(rect, ref tileMap);
            if (!generatedChunks.TryAdd(chunk))
            {
                foreach (var dood in TerrainMap.GetNativeDoodads(rect))
                    map.Add(dood);
            }
        }

        const int perfBarLength = 20;
        public string GetPerfData()
        {
            return string.Empty;

            var totalTimeTaken = PerfCounter.Stats.Sum(kvp => kvp.Value);

            var text = PerfCounter.Stats
                .OrderByDescending(kvp => kvp.Key)
                .Select(kvp => new
                {
                    Pluses = (int)(kvp.Value * perfBarLength / totalTimeTaken),
                    Name = kvp.Key,
                })
                .Select(n => "{0}{1}   {2}".F(new string('+', n.Pluses), new string('-', perfBarLength - n.Pluses), n.Name))
                .Aggregate("", (a, b) => a + '\n' + b);

            return text;
        }
    }
}
