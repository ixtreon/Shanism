using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Chat
{
    interface IChatConsumer
    {
        void SendChatMessage(string msg);
    }
}
