using Shanism.Common.Message.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Events
{
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
