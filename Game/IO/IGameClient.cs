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
    /// Represents a client connected to the server. 
    /// It could be either a local or remote (network'd) one. 
    /// </summary>
    public interface IGameClient
    {
        string Name { get; }

    }
}
