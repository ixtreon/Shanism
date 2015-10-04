using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Message;

namespace IO
{
    /// <summary>
    /// A prpospective client looking to play on the server.
    ///  
    /// It could be either a local or remote (network'd) one. 
    /// </summary>
    public interface IGameClient
    {
        /// <summary>
        /// Gets the name of the client. 
        /// </summary>
        string Name { get; }

    }
}
