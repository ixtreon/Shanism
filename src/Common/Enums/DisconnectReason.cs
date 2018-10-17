using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    public enum DisconnectReason
    {
        /// <summary>
        /// Server dislikes you.
        /// </summary>
        Kicked,
        /// <summary>
        /// No response from server.
        /// </summary>
        TimeOut,
        /// <summary>
        /// Server died.
        /// </summary>
        Stopped,
        /// <summary>
        /// Client stopped caring.
        /// </summary>
        Manual,
    }
}
