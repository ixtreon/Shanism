using Shanism.Common.ObjectStubs;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// The server sends or relays chat to the client.
    /// </summary>
    [ProtoContract]
    public class ServerChat : ServerMessage
    {
        const uint SystemMessageId = Util.GenericId<bool>.None;

        public override ServerMessageType Type => ServerMessageType.Chat;


        /// <summary>
        /// The Guid of the sender, or 0 if this is a system message. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint SenderGuid;

        [ProtoMember(2)]
        public readonly string Message;

        public bool IsSystem => SenderGuid == SystemMessageId;

        ServerChat() { }

        public ServerChat(IPlayer sender, string message)
            : this()
        {
            if (sender == null)
                SenderGuid = SystemMessageId;
            else
                SenderGuid = sender.Id;

            Message = message;
        }
    }
}
