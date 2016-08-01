using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shanism.Engine;
using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Client;
using Shanism.Common.Message;
using Shanism.Common.Message.Client;
using System.IO;

namespace Shanism.Local
{
    /// <summary>
    /// Represents a locally played game. 
    /// Automatically starts both the engine and the client. 
    /// </summary>
    public class LocalShano
    {
        readonly ShanoEngine engine;

        readonly IClientInstance client;

        readonly IReceptor receptor;

        /// <summary>
        /// Creates a new local game instance, putting the provided hero in the map with the specified seed. 
        /// </summary>
        /// <param name="mapSeed">The map seed. </param>
        public LocalShano(string playerName, int mapSeed, string scenarioPath)
        {
            //create the local server and client
            engine = new ShanoEngine();
            engine.LoadScenario(scenarioPath, mapSeed);

            client = ShanoGame.CreateClient(playerName);

            receptor = engine.AcceptClient(client.Engine);

            client.GameLoaded += () =>
            {
                client.SetServer(receptor);
                engine.StartPlaying(receptor);
            };

            client.Run();
        }

        public void OpenToNetwork()
        {
            engine.OpenToNetwork();
        }
    }
}
