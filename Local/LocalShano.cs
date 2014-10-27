using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using Engine.Objects;
using IO;
using IO.Commands;
using IO.Common;
using ShanoRpgWinGl;

namespace Local
{
    /// <summary>
    /// Represents a locally played game. 
    /// Automatically starts both the engine and the client. 
    /// </summary>
    public class LocalShano : IClient, IServer
    {
        public Hero LocalHero { get; private set; }

        public MovementState MovementState { get; set; }

        IHero IServer.LocalHero
        {
            get { return LocalHero; }
        }

        public event Action<ActionArgs> OnSpecialAction;

        /// <summary>
        /// Gets the game engine. 
        /// </summary>
        public readonly ShanoRpg ShanoGame;

        /// <summary>
        /// Gets the game client. 
        /// </summary>
        public readonly MainGame ShanoClient;

        /// <summary>
        /// Creates a new local game instance, putting the provided hero in the map with the specified seed. 
        /// </summary>
        /// <param name="mapSeed">The map seed. </param>
        /// <param name="h">The hero to play with. </param>
        public LocalShano(int mapSeed, Hero h)
        {
            MovementState = new MovementState();
            LocalHero = h;

            //create the game engine
            //hack: should accept IHero?
            ShanoGame = new ShanoRpg(mapSeed, new Player(h, this));

            //create the local game client
            ShanoClient = new MainGame(h);


            //link them
            ShanoClient.Server = this;

            //start the client
            ShanoClient.Running = true;
        }

        public void OpenToNetwork(int port)
        {
            ShanoGame.OpenToNetwork(port);
        }


        public void GetNearbyTiles(ref MapTile[,] tiles, out double x, out double y)
        {
            ShanoGame.GetNearbyTiles(LocalHero, ref tiles, out x, out y);
        }

        public IEnumerable<IUnit> GetUnits()
        {
            return ShanoGame.GetNearbyUnits(LocalHero);
        }

        public IEnumerable<IGameObject> GetGameObjects()
        {
            return ShanoGame.GetNearbyObjects(LocalHero);
        }

        public void RegisterAction(ActionArgs arg)
        {
            if (OnSpecialAction != null)
                OnSpecialAction(arg);
        }
    }
}
