using Shanism.Client.UI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Message;
using Shanism.Common.Message.Server;

namespace Shanism.Client.Systems
{
    class ChatSystem : ClientSystem, IChatProvider
    {
        public event Action<string> ChatSent;

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        public override void HandleMessage(IOMessage ioMsg)
        {
            if (ioMsg.Type == MessageType.ServerChat)
            {
                var msg = (ChatMessage)ioMsg;
                ChatSent?.Invoke(msg.Message);
            }
        }
    }
}
