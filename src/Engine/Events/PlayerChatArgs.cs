using Shanism.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Events
{
    /// <summary>
    /// Contains information about a chat message, such as its text and sender.
    /// </summary>
    public class PlayerChatArgs : EventArgs
    {
        /// <summary>
        /// Gets the player who wrote the message. 
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public string Message { get; }


        internal PlayerChatArgs(Player pl, string msg)
        {
            Player = pl;
            Message = msg;
        }
    }
}
