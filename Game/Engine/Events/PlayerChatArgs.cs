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

        /// <summary>
        /// Gets or sets whether the chat message is further propagated to the engine for normal processing. 
        /// If set to false the chat message won't show to other players, for example. 
        /// </summary>
        public bool Propagate { get; set; } = true;


        internal PlayerChatArgs(Player pl, string msg)
        {
            Player = pl;
            Message = msg;
        }
    }
}
