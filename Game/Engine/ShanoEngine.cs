using Shanism.Engine.Maps;
using Shanism.Engine.Systems;
using Shanism.Common;
using Shanism.Common.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Shanism.Common.Performance;
using Shanism.Common.Util;
using Shanism.Engine.Network;
using Shanism.Engine.Players;

namespace Shanism.Engine
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
        /// A list of all receptors to players currently in game. 
        /// </summary>
        internal ConcurrentBag<ShanoReceptor> Players { get; } = new ConcurrentBag<ShanoReceptor>();


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
            string scenarioCompileErrors;
            Scenario = Scenario.Load<Scenario>(Path.GetFullPath(scenarioDir), out scenarioCompileErrors);
            if (Scenario == null)
                throw new FileLoadException($"Unable to load the scenario: \n\n{scenarioCompileErrors}");

            //register the server as the parent of all objects. 
            GameObject.SetEngine(this);

            //create the terrain, object map from the scenario. 
            TerrainMap = Maps.Terrain.MapGod.Create(Scenario.Config.Map, mapSeed);

            //create all subsystems
            systems.Add((map = new MapSystem()));
            systems.Add(network = new NetworkSystem());

            //add startup units n doodads
            Scenario.CreateStartupObjects(map);

            //run scripts
            Scenario.RunScripts(cs => cs.OnGameStart());
        }

        #region IEngine implementation

        public INetReceptor AcceptClient(IShanoClient c)
        {
            // TODO: do some checks on player join?x

            var pl = new ShanoReceptor(this, c);
            return pl;
        }

        public void StartPlaying(IReceptor rec)
        {
            var pl = rec as ShanoReceptor;
            if (pl == null)
                throw new ArgumentException(nameof(rec), "You should supply a {0} of type {1} as returned by this engine!".F(nameof(IReceptor), nameof(ShanoReceptor)));

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
        public void OpenToNetwork()
        {
            network.Start(this);
        }
        #endregion

        /// <summary>
        /// Adds the given player to the game. 
        /// </summary>
        /// <param name="pl"></param>
        void AddPlayer(ShanoReceptor pl)
        {
            Players.Add(pl);

            Scenario.RunScripts(s => s.OnPlayerJoined(pl.Player));
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
            GameTime += msElapsed;
            PerfCounter.Reset();

            //update systems
            foreach (var sys in systems)
                sys.Update(msElapsed);

            ////update players
            //foreach (var p in Players)
            //    p.Update(msElapsed);

        }


        /// <summary>
        /// Writes the map data for the given rectangle in the given array. 
        /// </summary>
        public void GetTiles(IReceptor pl, ref TerrainType[] tileMap, MapChunkId chunk)
        {
            //TODO: check if chunk is valid for player pl


            TerrainMap.GetMap(chunk.Span, ref tileMap);
            if (!generatedChunks.TryAdd(chunk))
            {
                foreach (var dood in TerrainMap.GetNativeEntities(chunk.Span))
                    map.Add(dood);
            }
        }

        const int perfBarLength = 20;
        public string GetPerfData()
        {
            //return string.Empty;

            var totalTimeTaken = PerfCounter.Stats.Sum(kvp => kvp.Value);

            var text = PerfCounter.Stats
                .OrderByDescending(kvp => kvp.Key)
                .Select(kvp => new
                {
                    Pluses = (int)(kvp.Value * perfBarLength / totalTimeTaken),
                    Name = kvp.Key,
                })
                .Select(n => "{0}{1}   {2}".F(new string('+', n.Pluses), new string('_', perfBarLength - n.Pluses), n.Name))
                .Aggregate("", (a, b) => a + '\n' + b);

            return text;
        }
    }
}
