using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Engine.Exceptions;
using Shanism.Engine.GameSystems;
using Shanism.Engine.Maps;
using Shanism.Engine.Network;
using Shanism.Engine.Players;
using Shanism.Engine.Scripting;
using Shanism.ScenarioLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Shanism.Engine
{
    /// <summary>
    /// The game engine lies here.
    /// </summary>
    /// <seealso cref="IShanoEngine" />
    public class ShanoEngine : IShanoEngine, IGame
    {
        /// <summary>
        /// The frames per second we aim to run at. 
        /// </summary>
        const int FPS = 60;

        /// <summary>
        /// Milliseconds per frame. 
        /// </summary>
        const int MSPF = 1000 / FPS;


        /// <summary>
        /// A list of all receptors (human players) currently in game. 
        /// </summary>
        internal readonly ConcurrentDictionary<string, ShanoReceptor> Players = new ConcurrentDictionary<string, ShanoReceptor>();


        internal readonly PerfCounter UnitPerfCounter = new PerfCounter();
        readonly PerfCounter GamePerfCounter = new PerfCounter();
        readonly Counter perfResetCounter = new Counter(250);


        readonly List<GameSystem> systems = new List<GameSystem>();

        internal readonly MapSystem map;
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

        public string DebugString { get; private set; }


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
            if (!GameObject.TrySetGame(this))
                throw new SingleServerException();
        }


        public bool TryLoadScenario(string scenarioDir, int mapSeed, out string errors)
        {
            //compile the scenario
            scenarioDir = Path.GetFullPath(scenarioDir);
            var result = Scenario.Load(scenarioDir, out errors, out scenario);
            if (result != Scenario.ScenarioCompilationResult.Success)
                return false;

            //initialize map + objects, scripts  
            map.LoadScenario(Scenario, mapSeed);
            scripts.LoadScenario(Scenario);

            //fire the OnGameStart script event
            Scripts.Run(cs => cs.OnGameStart());

            return true;
        }


        #region IShanoEngine implementation
        /// <summary>
        /// Accepts the given client to the server.
        /// Returns the network receptor responsible for it. 
        /// </summary>
        public IReceptor AcceptClient(IShanoClient c)
        {
            if (!Players.TryAdd(c.Name, null))
                return null;

            return new ShanoReceptor(this, c);
        }

        public void StartPlaying(IReceptor rec)
        {
            var pl = rec as ShanoReceptor;
            if (pl == null)
                throw new ArgumentException(nameof(rec), $"The receptor must be of type `{nameof(ShanoReceptor)}` as returned by this engine!");

            addPlayer(pl);
            pl.SendHandshake(true);
        }

        /// <summary>
        /// Starts the network module which allows remote players to connect to the server. 
        /// </summary>
        public void OpenToNetwork()
        {
            network.Restart(this);
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
        #endregion


        #region IGame implementation
        /// <summary>
        /// Gets all players currently connected to the game.
        /// </summary>
        IEnumerable<IPlayer> IGame.Players
            => Players.Select(kvp => kvp.Value.Player);

        int IGame.PlayerCount => Players.Count;

        /// <summary>
        /// Sends a system message to all currently connected players.
        /// </summary>
        /// <param name="msg">The message to send.</param>
        public void SendSystemMessage(string msg)
        {
            foreach (var kvp in Players)
                kvp.Value.SendSystemMessage(msg);
        }

        /// <summary>
        /// Sends a system message to the specified player.
        /// </summary>
        /// <param name="pl">The player to send the message to.</param>
        /// <param name="msg">The message to send.</param>
        public void SendSystemMessage(IPlayer pl, string msg)
        {
            ShanoReceptor receptor;
            if (!Players.TryGetValue(pl.Name, out receptor))
                return;     //silently fail?!
            receptor.SendSystemMessage(msg);
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
                DebugString = $"{GamePerfCounter.GetPerformanceData()}\n\n"
                    + $"{UnitPerfCounter.GetPerformanceData()}\n\n";

                UnitPerfCounter.Reset();
                GamePerfCounter.Reset();
            }

            foreach (var sys in systems)
                GamePerfCounter.RunAndLog(sys.SystemName, sys.Update, msElapsed);

            foreach (var kvp in Players)
                kvp.Value.Update(msElapsed);
        }


        /// <summary>
        /// Adds the given player to the game. 
        /// </summary>
        /// <param name="pl"></param>
        void addPlayer(ShanoReceptor pl)
        {
            if (!Players.ContainsKey(pl.Name) || Players[pl.Name] != null)
                throw new Exception();

            Players[pl.Name] = pl;

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

        public void RestartScenario()
        {
            throw new NotImplementedException();
        }
    }
}
