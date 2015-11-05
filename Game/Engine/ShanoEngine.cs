using Engine.Maps;
using Engine.Systems;
using IO;
using IO.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Engine
{
    /// <summary>
    /// The game engine lies here. 
    /// </summary>
    public class ShanoEngine : IEngine
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
        internal List<Player> Players { get; } = new List<Player>();

        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal EntityMap EntityMap { get; }

        internal Scenario Scenario { get; private set; }

        internal Network.LServer NetworkServer { get; private set; }

        /// <summary>
        /// Gets whether this engine is open for online play. 
        /// </summary>
        internal bool IsOnline { get; private set; }

        #region Subsystems
        RangeEventProvider rangeSystem = new RangeEventProvider();
        #endregion

        public ShanoEngine(int mapSeed, string scenarioDir)
        {
            
            // allow only one instance of the server. An ugly hack..
            if (Current != null)
                throw new Exception("Please run only one instance of the server!");
            Current = this;

            //compile the scenario
            try
            {
                Scenario = Scenario.Load(Path.GetFullPath(scenarioDir));
            }
            catch(Exception e)
            {
                throw;
            }


            //create the terrain map from the scenario. 
            TerrainMap = Maps.Terrain.MapGod.Create(Scenario.MapConfig, mapSeed);

            //create systems, entity map
            rangeSystem = new RangeEventProvider();

            EntityMap = new EntityMap(rangeSystem);


            //run scripts
            Scenario.RunScripts(cs => cs.LoadModels(Scenario.Models));
            Scenario.RunScripts(cs => cs.GameStart());
        }

        #region IEngine implementation

        public IReceptor AcceptClient(IClient c)
        {
            //TODO: do some checks???!?

            var pl = new Player(this, c);
            return pl;
        }

        public void StartPlaying(IReceptor rec)
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
            this.Players.Add(pl);

            Scenario.RunScripts(s => s.OnPlayerJoined(pl));
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

            //update range system before locations 
            //so new units get properly detected


            //update entity locations
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
