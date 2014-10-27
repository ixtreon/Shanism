using Engine.Objects;
using Engine.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IO;
using ScriptLib;
using Engine.Systems;
using IO.Common;
using Engine.Network;
//using Input;

namespace Engine
{
    public class ShanoRpg
    {
        public static ShanoRpg Current { get; private set; }

        /// <summary>
        /// The frames per second we aim to run at. 
        /// </summary>
        const int FPS = 60;


        private readonly Thread MainThread;

        /// <summary>
        /// The current world map containing the terrain info. 
        /// </summary>
        internal WorldMap WorldMap { get; private set; }

        /// <summary>
        /// The current game map containing unit/doodad/sfx info. 
        /// </summary>
        internal GameMap GameMap { get; private set; }


        /// <summary>
        /// A list of all players currently in game. 
        /// </summary>
        internal List<Player> Players;


        internal Scenario Scenario { get; private set; }

        internal NetworkManager Network;

        /// <summary>
        /// Gets whether this game is open to online play. 
        /// </summary>
        /// <returns></returns>
        public bool IsOnline
        {
            get { return Network != null; }
        }


        public ShanoRpg(int mapSeed, Player localPlayer)
        {
            // f!@k hacks like this.. 
            // TODO: fix it somehow :|
            if (Current != null)
                throw new Exception("Please run only one instance of the server!");
            Current = this;

            this.WorldMap = new WorldMap(mapSeed);

            this.Players = new List<Player>();

            this.GameMap = new GameMap();

            Scenario = new Scenario("!DefaultScenario");

            if (!Scenario.TryCompile())
            {
                throw new Exception("Unable to compile the scenario!");
            }

            //run scripts
            Scenario.RunScripts(s => s.GameStart());

            // add the player
            AddPlayer(localPlayer);

            // start the update thread
            MainThread = new Thread(updateLoop)
            {
                IsBackground = true
            };
            MainThread.Start();

        }

        private void AddPlayer(Player p)
        {
            this.Players.Add(p);
            //run scripts
            Scenario.RunScripts(s => s.OnPlayerJoined(p));

            //add his hero to the map. 
            spawnHero(p.Hero);
        }

        private void AddClient(IClient d)
        {
            throw new NotImplementedException();
        }

        private void spawnHero(Hero h)
        {
            //next line is important for local heroes
            h.Game = this;

            GameMap.AddUnit(h);

            //run scripts
            Scenario.RunScripts(s => s.OnHeroSpawned(h));
        }

        public void OpenToNetwork(int port = 18881)
        {
            if (IsOnline)
            {
                Console.WriteLine("Trying to open the server for network play but it is already online!");
                return;
            }

            Network = new NetworkManager(port);
        }

        /// <summary>
        /// Performs the basic game loop. 
        /// </summary>
        private void updateLoop()
        {
            int frameStartTime, drawTime = 0;
            while(true)
            {
                var toSleep = 1000 / FPS - drawTime;    //time to sleep
                var isThrottled = toSleep < 0;          //or no sleep?

                if (!isThrottled)
                    Thread.Sleep(toSleep);
                else
                    Console.WriteLine("Warning: Drawing too slow!");

                frameStartTime = Environment.TickCount;
                this.Update(1000 / FPS);
                drawTime = Environment.TickCount - frameStartTime;
            }
        }

        private void Update(int msElapsed)
        {
            GameMap.Update(msElapsed);


            foreach (var p in Players)
                p.Update();
        }

        public IEnumerable<IUnit> GetNearbyUnits(Hero h)
        {
            var unitRange = (Vector)Constants.ClientParams.WindowSize / 2 + 1;
            return GameMap.GetUnitsInRect(h.Location - unitRange, unitRange * 2);
        }

        public IEnumerable<IGameObject> GetNearbyObjects(Hero h)
        {
            var unitRange = (Vector)Constants.ClientParams.WindowSize / 2 + 1;
            return GameMap.GetObjectsInRect(h.Location - unitRange, unitRange * 2);
        }

        public void GetNearbyTiles(IHero h, ref MapTile[,] tileMap, out double heroX, out double heroY)
        {
            heroX = h.Location.X;
            heroY = h.Location.Y;

            const int xRange = Constants.ClientParams.WindowWidth / 2;
            const int yRange = Constants.ClientParams.WindowHeight / 2;

            const int xSendSize = xRange * 2 + 1;
            const int ySendSize = yRange * 2 + 1;

            var x = (int)Math.Floor(heroX - xRange);
            var y = (int)Math.Floor(heroY - yRange);

            if (tileMap.GetLength(0) < xSendSize ||tileMap.GetLength(1) < ySendSize)
                throw new ArgumentOutOfRangeException("Tile array should be larger than the given. ");

            WorldMap.GetMap(x, y, xSendSize, ySendSize, ref tileMap);
        }
    }
}
