using Shanism.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems.Chat
{
    class ChatProvider
    {
        public void SendServerMessage(string message)
        {

        }

        /// <summary>
        /// Sends a message from the given unit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void SendChat(Player sender, string message)
        {
            //var pls = sender.VisibleObjects
            //    .OfType<Unit>()
            //    .Select(u => u.Owner)
            //    .Distinct();

            //var msg = new IO.Message.Server.ChatMessage(message, sender);
            //foreach (var pl in pls)
            //    pl.SendMessage(msg);
        }

        public void SendWhisper(Player sender, Player receiver, string message)
        {
            //var msg = new IO.Message.Server.ChatMessage(message, sender);
            //receiver.SendMessage(msg);
        }
    }
}
