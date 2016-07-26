using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Exceptions
{
    /// <summary>
    /// The exception thrown when a ShanoServer is already running. 
    /// </summary>
    /// <seealso cref="System.Exception" />
    class ServerRunningException : Exception
    {
        public override string Message 
            => $"This server is already running!";

    }
}
