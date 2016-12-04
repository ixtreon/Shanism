using Shanism.Common.Game;
using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// A game engine as seen by the clients. 
    /// 
    /// Acts as a gateway to new players willing to join the game. 
    /// Pairs each accepted client to a corresponding <see cref="IReceptor"/> to play. 
    /// </summary>
    public interface IShanoEngine
    {

        ServerState State { get; }

        /// <summary>
        /// Decides whether to accept the given client to the server. 
        /// If the client is accepted returns the receptor to use for communication with the server. Otherwise returns null. 
        /// </summary>
        IReceptor Connect(IShanoClient c);

        void StartPlaying(IReceptor rec);



        void OpenToNetwork();

        void RestartScenario();

        void Update(int msElapsed);
    }
}
