using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Commands;

namespace IO
{
    /// <summary>
    /// Represents a client connected to the server. 
    /// It could be either a local or remote (network'd) one. 
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// The event which is raised whenever a command is received. 
        /// </summary>
        event Action<ActionArgs> OnSpecialAction;

        /// <summary>
        /// Gets the current movement state of the player. 
        /// </summary>
        /// <returns></returns>
        MovementState MovementState { get; }


    }
}
