using Shanism.Engine.Maps;
using Shanism.Common;
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
using Shanism.Engine.Scripting;
using Shanism.Engine.Common;
using Shanism.ScenarioLib;
using Shanism.Engine.Exceptions;
using Shanism.Common.Game;
using Shanism.Engine.GameSystems;
using System.Text;
using System.Diagnostics;

namespace Shanism.Engine
{
    /// <summary>
    /// The game engine lies here.
    /// </summary>
    /// <seealso cref="IShanoEngine" />
    public class ShanoEngine : IShanoEngine
    {
        /// <summary>
        /// The frames per second we aim to run at. 
        /// </summary>
        const int FPS = 60;

        /// <summary>
        /// Milliseconds per frame. 
        /// </summary>
        const int MSPF = 1000 / FPS;


        readonly ConcurrentSet<MapChunkId> generatedChunks = new ConcurrentSet<MapChunkId>();

        /// <summary>
        /// A list of all receptors (human players) currently in game. 
        /// </summary>
        internal readonly ConcurrentDictionary<IShanoClient, ShanoReceptor> Players = new ConcurrentDictionary<IShanoClient, ShanoReceptor>();


        internal readonly PerfCounter UnitPerfCounter = new PerfCounter();
        readonly PerfCounter GamePerfCounter = new PerfCounter();
        readonly Counter perfResetCounter = new Counter(250);


        readonly List<GameSystem> systems = new List<GameSystem>();

        readonly MapSystem map;
        readonly NetworkSystem network;
        readonly ScriptingSystem scripts;
        readonly RangeSystem rangeQt;
        readonly EntitiesSystem objects;


        Scenario scenario;



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

        public string PerformanceData { get; private set; }


        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal IGameMap Map => map;

        internal IScriptRunner Scripts => scripts;

        /// <summary>
        /// Gets the scenario this engine is playing. 
        /// </summary>
        internal Scenario Scenario => scenario;


        public ShanoEngine()
        {
            scripts = new ScriptingSystem(Thread.CurrentThread);
            map = new MapSystem();
            rangeQt = new RangeSystem(map);
            network = new NetworkSystem();
            objects = new EntitiesSystem(map);

            systems.Add(objects);
            systems.Add(rangeQt);
            systems.Add(map);
            systems.Add(scripts);
            systems.Add(network);

            //register this server as the parent of all objects. 
            //quite hacky by nature
            if (!GameObject.TrySetEngine(this))
                throw new SingleServerException();
        }


        public void LoadScenario(string scenarioDir, int? mapSeed = null)
        {
            //compile the scenario
            scenarioDir = Path.GetFullPath(scenarioDir);
            string scenarioCompileErrors;
            var result = Scenario.Load(scenarioDir, out scenarioCompileErrors, out scenario);
            if (result != Scenario.ScenarioCompilationResult.Success)
                throw new ScenarioLoadException(scenarioDir, scenarioCompileErrors);


            //initialize map + objects, scripts  
            map.LoadScenario(Scenario, mapSeed ?? Rnd.Next());
            scripts.LoadScenario(Scenario);

            //fire the OnGameStart script event
            Scripts.Run(cs => cs.OnGameStart());
        }
         
        static void createStartupObjects(Scenario sc, MapSystem map)
        {

        }


        #region IEngine implementation

        /// <summary>
        /// Accepts the given client to the server.
        /// Returns the network receptor responsible for it. 
        /// </summary>
        public INetReceptor AcceptClient(IShanoClient c)
        {
            var pl = new ShanoReceptor(this, c);

            // TODO: do some checks on player join?

            return pl;
        }

        public void StartPlaying(IReceptor rec)
        {
            var pl = rec as ShanoReceptor;
            if (pl == null)
                throw new ArgumentException(nameof(rec), $"The receptor must be of type `{nameof(ShanoReceptor)}` as returned by this engine!");
            pl.SendHandshake(true);

            addPlayer(pl);
        }

        #endregion

        #region Server Controls

        /// <summary>
        /// Starts running the main game loop on the current thread. 
        /// </summary>
        /// <exception cref="ServerRunningException">Thrown if the server has already been started. </exception>
        public void Start()
        {
            if (IsRunning) throw new ServerRunningException();
            IsRunning = true;

            mainLoop();
        }

        /// <summary>
        /// Starts running the main game loop on a new background thread. 
        /// </summary>
        /// <exception cref="ServerRunningException">Thrown if the server has already been started. </exception>
        public Thread StartBackground()
        {
            if (IsRunning) throw new ServerRunningException();
            IsRunning = true;

            GameThread = new Thread(mainLoop) { IsBackground = true };
            GameThread.Start();

            return GameThread;
        }

        /// <summary>
        /// Stops the game server as soon as the current frame has finished processing. 
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Starts the network module which allows remote players to connect to the server. 
        /// </summary>
        public void OpenToNetwork()
        {
            network.Restart(this);
        }


        #endregion


        /// <summary>
        /// Performs a single update of the game state. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last call of this function. </param>
        public void Update(int msElapsed)
        {
            GameTime += msElapsed;

            if (perfResetCounter.Tick(msElapsed))
            {
                PerformanceData = $"{GamePerfCounter.GetPerformanceData()}\n\n"
                    + $"{UnitPerfCounter.GetPerformanceData()}\n\n";

                UnitPerfCounter.Reset();
                GamePerfCounter.Reset();
            }

            foreach (var sys in systems)
                GamePerfCounter.RunAndLog(sys.SystemName, sys.Update, msElapsed);
        }


        /// <summary>
        /// Adds the given player to the game. 
        /// </summary>
        /// <param name="pl"></param>
        void addPlayer(ShanoReceptor pl)
        {
            Players[pl.Client] = pl;

            Scripts.Run(s => s.OnPlayerJoined(pl.Player));
        }


        /// <summary>
        /// Runs the main game loop. 
        /// </summary>
        void mainLoop()
        {
            while (IsRunning)
            {
                var frameStart = Environment.TickCount;

                Update(1000 / FPS);

                var msElapsed = Environment.TickCount - frameStart;
                var msSleep = MSPF - msElapsed;     //to sleep
                var isThrottled = msSleep < 0;      //or not to sleep?

                if (!isThrottled)
                    Thread.Sleep(msSleep);
            }
        }

    }
}
