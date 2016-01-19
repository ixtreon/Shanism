using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// A game receptor as exposed by the engine to the network module. 
    /// </summary>
    public interface INetReceptor : IReceptor
    {
        void SendHandshake(bool isSuccessful);
    }
}
