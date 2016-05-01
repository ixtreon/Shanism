using IO.Common;
using IO.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// A game engine as seen by the clients. 
    /// 
    /// Acts as a gateway to new players willing to join the game. 
    /// Pairs each accepted client to a corresponding <see cref="INetReceptor"/> to play. 
    /// </summary>
    public interface IShanoEngine
    {

        /// <summary>
        /// Decides whether to accept the given client to the server. 
        /// If the client is accepted returns the network receptor responsible for it. Otherwise returns null. 
        /// </summary>
        /// <returns></returns>
        INetReceptor AcceptClient(IShanoClient c);

        void StartPlaying(IReceptor rec);


        void OpenToNetwork();
    }
}
