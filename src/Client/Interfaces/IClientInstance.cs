using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client
{
    /// <summary>
    /// A local game instance. 
    /// Contains the client engine along with the code necessary to launch it standalone. 
    /// </summary>
    public interface IClientInstance
    {
        /// <summary>
        /// Starts the engine.
        /// </summary>
        void Run();
    }
}
