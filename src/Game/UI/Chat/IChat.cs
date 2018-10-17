using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Chat
{
    /// <summary>
    /// A chat message consumer such as an UI chat-box.
    /// </summary>
    interface IChatConsumer
    {
        void SendChatMessage(string msg);
    }

    /// <summary>
    /// A chat message source, such as a game engine or an UI chat bar.
    /// </summary>
    interface IChatSource
    {
        event Action<string> ChatMessageReceived;
    }

    interface IChatProxy : IChatSource, IChatConsumer { }
}
