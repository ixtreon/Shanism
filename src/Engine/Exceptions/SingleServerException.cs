using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Exceptions
{
    /// <summary>
    /// The exception thrown when more than one ShanoServer is started
    /// from the same assembly. 
    /// </summary>
    class SingleServerException : Exception
    {
        public override string Message
            => $"Only one instance of a server can be running at a time!";
    }
}
