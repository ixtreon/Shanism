using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using Engine.Entities;
using IO;
using IO.Common;
using Client;
using Engine.Entities.Objects;
using IO.Message;
using IO.Message.Client;
using System.IO;

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
        public readonly IEngine ShanoEngine;

        /// <summary>
        /// Gets the game client. 
        /// </summary>
        public readonly IShanoClient ShanoClient;

        /// <summary>
        /// Creates a new local game instance, putting the provided hero in the map with the specified seed. 
        /// </summary>
        /// <param name="mapSeed">The map seed. </param>
        /// <param name="h">The hero to play with. </param>
        public LocalShano(string playerName, int mapSeed, string scenarioPath)
        {
            //create the local server and client
            ShanoEngine = new ShanoEngine(mapSeed, scenarioPath);
            ShanoClient = ShanoGame.Create(playerName);

            var receptor = ShanoEngine.AcceptClient(ShanoClient);
            ShanoClient.SetServer(receptor);

            ShanoClient.GameLoaded += () => ShanoEngine.StartPlaying(receptor);

            //create the local player

            //var pl = new Player(ShanoEngine, ShanoClient);
            //ShanoEngine.AddPlayer(pl);

            //link them

            //start the client
            ShanoClient.Run();
        }
    }
}
