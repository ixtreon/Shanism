using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.UI.Chat;

namespace Shanism.Client
{
    partial class ClientEngine : IChatConsumer, IChatSource
    {
        //called by the engine whenever a message is received.
        public event Action<string> ChatMessageSent;


        //called by the UI whenever the player sends a message.
        public void SendChatMessage(string msg)
        {
            sendMessage(new Common.Message.Client.PlayerChatMessage("", msg));
        }
    }
}
