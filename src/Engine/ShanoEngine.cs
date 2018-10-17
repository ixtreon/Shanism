using Shanism.Common;
using Shanism.Common.Util;
using Shanism.Engine.Models.Systems;
using Shanism.Engine.Systems;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Shanism.Engine
{

    /// <summary>
    /// The game engine lies here.
    /// </summary>
    /// <seealso cref="IEngine" />
    public partial class ShanoEngine : IEngine, IGame
    {

        // debug
        readonly PerfCounter debugPerfEngine = new PerfCounter("Engine");
        internal readonly PerfCounter debugPerfUnits = new PerfCounter("Units");
        readonly Counter debugUpdateCounter = new Counter(250);


        // systems
        SystemList systems;

        NetworkSystem network;
        PlayerSystem players;
        MapSystem map;
        ScriptingSystem scripts;
        EntitiesSystem objects;
        RangeSystem range;


        // other
        readonly ScenarioCompiler compiler = new ScenarioCompiler();
        readonly AssemblyLoaderDelegate assemblyLoader;

        public bool IsLocal { get; } = true;

        /// <summary>
        /// Gets the current state of the server.
        /// </summary>
        public ServerState State { get; private set; } = ServerState.Stopped;

        /// <summary>
        /// Gets the current scenario.
        /// </summary>
        public Scenario Scenario { get; private set; }

        /// <summary>
        /// Gets the server's debug string.
        /// </summary>
        public string DebugString { get; private set; }

        internal Allocator Allocator { get; private set; }

        public TimeSpan GameTime { get; private set; }


        /// <summary>
        /// Gets the current game map.
        /// </summary>
        public IGameMap Map => map;

        /// <summary>
        /// Gets all players currently connected to the game.
        /// </summary>
        public IReadOnlyList<IPlayer> Players => players.Players;

        internal IScriptRunner Scripts => scripts;


        public ShanoEngine(AssemblyLoaderDelegate assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;

            if(!System.Numerics.Vector.IsHardwareAccelerated)
                Console.WriteLine($"No SIMD support found. Game may be really sluggish...");
        }



        public bool TryLoadScenario(
            string scenarioDir,
            out string errors)
        {
            Console.WriteLine($"Loading `{scenarioDir}`...");

            // this engine is now the parent of all objects. 
            //  - quite hacky by nature
            //  - the only way to have `new Unit()` know its game immediately
            //  - what about Unit() knowing it when it's added to the map?..
            GameContext.SetGame(this);

            //reset state
            State = ServerState.Loading;
            GameTime = TimeSpan.Zero;
            Allocator = new Allocator();

            //compile the scenario
            scenarioDir = Path.GetFullPath(scenarioDir);
            var compileResult = compiler.TryCompile(assemblyLoader, scenarioDir);
            if(!compileResult.IsSuccessful)
            {
                State = ServerState.LoadFailure;
                GameContext.ResetGame();

                errors = string.Join("\n", compileResult.Errors);
                return false;
            }

            Scenario = compileResult.Value;

            // init systems
            systems = new SystemList(debugPerfEngine)
            {
                (network = new NetworkSystem()),
                (players = new PlayerSystem(this)),
                (scripts = new ScriptingSystem(Thread.CurrentThread, Scenario)),
                (map = new MapSystem(Scenario)),
                (objects = new EntitiesSystem(map)),
                (range = new RangeSystem(map)),
            };

            //fire the OnGameStart script event
            scripts.Run(cs => cs.OnGameStart());

            errors = null;
            State = ServerState.Playing;
            return true;
        }


        #region Server Controls

        /// <summary>
        /// Decides whether to accept the given client to the server.
        /// If the client is accepted returns the receptor to use for communication with the server. Otherwise returns null.
        /// </summary>
        IEngineReceptor IEngine.Connect(IClientReceptor c)
            => Connect(c, false);

        /// <summary>
        /// Decides whether to accept the given client to the server.
        /// If the client is accepted returns the receptor to use for communication with the server. Otherwise returns null.
        /// </summary>
        public IEngineReceptor Connect(IClientReceptor c, bool isHost)
        {
            if(players.TryConnect(c, isHost, out var receptor))
                return receptor;
            return null;
        }

        /// <summary>
        /// Disconnects the player with the specified name.
        /// Returns whether the player was found and removed.
        /// </summary>
        public bool Disconnect(string name)
        {
            var r = players.Disconnect(name);
            if(r.IsHost)
                Stop();
            return true;
        }

        /// <summary>
        /// Starts the network module, 
        /// letting other players connect to the game.
        /// </summary>
        public void OpenToNetwork()
        {
            network.Restart(this);
        }

        /// <summary>
        /// Stops the game server as soon as the current frame has finished processing. 
        /// </summary>
        public void Stop()
        {
            State = ServerState.Stopped;
            GameContext.ResetGame();
        }

        public bool TryRestartScenario(out string errors)
        {
            if(Scenario == null)
                throw new InvalidOperationException("No scenario to be restarted...");

            if(State != ServerState.Stopped)
                Stop();

            return TryLoadScenario(Scenario.Config.BaseDirectory, out errors);
        }

        #endregion


        #region In-Game Methods

        public void SendMessage(string message)
            => players.SendSystemMessage(message);

        public void SendMessageToPlayer(IPlayer player, string message)
            => players.SendSystemMessage(message, player);

        #endregion


        /// <summary>
        /// Performs a single update of the game engine. 
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last call of this function. </param>
        public void Update(int msElapsed)
        {
            if(State != ServerState.Playing)
                throw new InvalidOperationException($"Unable to start the server while in the {State} state.");

            GameTime += TimeSpan.FromMilliseconds(msElapsed);

            // debug 
            updateDebug(msElapsed);

            // systems
            systems.Update(msElapsed);
        }

        void updateDebug(int msElapsed)
        {
            if(debugUpdateCounter.Tick(msElapsed))
            {
                var timePeriod = debugUpdateCounter.MaxValue;
                DebugString = $"{debugPerfEngine.GetPerformanceData(timePeriod)}\n\n"
                    + $"{debugPerfUnits.GetPerformanceData(timePeriod)}\n\n";

                debugPerfUnits.Reset();
                debugPerfEngine.Reset();
            }
        }
    }
}
