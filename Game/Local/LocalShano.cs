using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using Engine.Objects;
using IO;
using IO.Common;
using Client;
using Engine.Objects.Game;
using IO.Message;
using IO.Message.Client;

namespace Local
{
    /// <summary>
    /// Represents a locally played game. 
    /// Automatically starts both the engine and the client. 
    /// </summary>
    public class LocalShano
    {
        /// <summary>
        /// Gets the game engine. 
        /// </summary>
        public readonly ShanoEngine ShanoEngine;

        /// <summary>
        /// Gets the game client. 
        /// </summary>
        public readonly MainGame ShanoClient;

        /// <summary>
        /// Creates a new local game instance, putting the provided hero in the map with the specified seed. 
        /// </summary>
        /// <param name="mapSeed">The map seed. </param>
        /// <param name="h">The hero to play with. </param>
        public LocalShano(string playerName, int mapSeed)
        {
            //create the local server and client
            ShanoEngine = new ShanoEngine(mapSeed);
            ShanoClient = new MainGame(playerName);

            //create the local player
            var pl = new Player(ShanoEngine, ShanoClient);

            //link them
            ShanoEngine.AddPlayer(pl);
            ShanoClient.SetServer(pl);

            //start the client
            ShanoClient.Running = true;
        }
    }
}
