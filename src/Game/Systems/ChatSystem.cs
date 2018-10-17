using Shanism.Client.UI.Chat;
using Shanism.Common.Messages;
using System;

namespace Shanism.Client.Game.Systems
{
    /// <summary>
    /// Acts as a proxy for chat, 
    /// </summary>
    class ChatProxy : IChatProxy
    {
        /// <summary>
        /// Raised whenever the engine sends us a message.
        /// </summary>
        public event Action<string> ChatMessageReceived;

        public event Action<ClientMessage> MessageSent;

        //called by the UI whenever the player sends a message.
        public void SendChatMessage(string msg)
            => MessageSent?.Invoke(new ClientChat("", msg));

        public void ParseMessage(ServerChat msg)
            => ChatMessageReceived?.Invoke(msg.Message);
    }
}
