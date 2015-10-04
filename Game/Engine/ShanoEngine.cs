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
using ScriptLib;

namespace Engine
{
    /// <summary>
    /// The game engine lies here. 
    /// </summary>
    public class ShanoEngine : INetworkEngine
    {
        public static ShanoEngine Current { get; private set; } // ugly hax :|


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
        internal ITerrainMap TerrainMap { get; private set; }

        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal EntityMap EntityMap { get; private set; }


        /// <summary>
        /// A list of all players currently in game. 
        /// </summary>
        internal List<Player> Players { get; } = new List<Player>();


        internal Scenario Scenario { get; private set; }

        internal ScenarioCompiler ScenarioCompiler { get; private set; }

        internal Network.LServer NetworkServer { get; private set; }

        internal bool IsOnline { get; private set; }

        private HashSet<MapChunkId> generatedChunks = new HashSet<MapChunkId>();

        public ShanoEngine(int mapSeed)
        {
            // allow only one instance of the server. 
            // an ugly hack..
            if (Current != null)
                throw new Exception("Please run only one instance of the server!");

            Current = this;

            this.Players = new List<Player>();

            this.EntityMap = new EntityMap();

            this.TerrainMap = new RandomTerrainMap(mapSeed);

            ScenarioCompiler = new ScenarioCompiler();
            ScenarioCompiler.ScenarioDir = Path.GetFullPath(@"DefaultScenario");

            Scenario = ScenarioCompiler.TryCompile<Scenario>();

            if(Scenario == null)
            {
                throw new Exception();
            }

            Scenario.LoadTypes(ScenarioCompiler.Assembly);
            Scenario.RunScripts(cs => cs.LoadModels(Scenario.Models));

            //run startup scripts
            Scenario.RunScripts(cs => cs.GameStart());
        }

        #region Network Engine implementation
        event Action<IEnumerable<IPlayer>, OrderType> INetworkEngine.AnyUnitOrderChanged
        {
            add { Unit.AnyOrderChanged += (u, o) => value(new[] { u.Owner }, o?.Type ?? OrderType.Stand); }
            remove { Unit.AnyOrderChanged -= (u, o) => value(new[] { u.Owner }, o.Type); }
        }

        public INetworkReceptor HandleNetConnection(IGameClient c)
        {
            var playerName = c.Name;

            //TODO: do some checks???!?

            var pl = new Player(this, c);

            AddPlayer(pl);

            return pl;
        }
        #endregion

        #region Server Controls
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
        /// <param name="p"></param>
        public void AddPlayer(Player p)
        {
            this.Players.Add(p);

            //run scripts
            Scenario.RunScripts(s => s.OnPlayerJoined(p));

            ////run scripts -> TODO: not here though!
            //Scenario.RunScripts(s => s.OnHeroSpawned(p.MainHero));
        }


        /// <summary>
        /// Starts the basic game loop. 
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
