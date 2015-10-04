using IO.Common;
using IO.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    /// <summary>
    /// A game engine as seen by an attached network controller guy. 
    /// 
    /// Provides the controller with the events necessary to update players. 
    /// </summary>
    public interface INetworkEngine
    {
        event Action<IEnumerable<IPlayer>, OrderType> AnyUnitOrderChanged;

        /// <summary>
        /// Decides whether to accept the given client to the server. 
        /// If the client is accepted returns the network recepter responsible for it. Otherwise returns null. 
        /// </summary>
        /// <returns></returns>
        INetworkReceptor HandleNetConnection(IGameClient c);
    }
}
