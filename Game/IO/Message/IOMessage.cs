using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using IO.Message.Client;
using System.Reflection;
using IO.Message.Server;
using System.Threading;

namespace IO.Message
{
    /// <summary>
    /// Represents any command or message sent between the server and the client. 
    /// 
    /// Implements (de)serialization using ProtoBuf on registered classes. 
    /// </summary>
    [ProtoContract(SkipConstructor = true)]

    //client
    [ProtoInclude((int)MessageType.Action, typeof(ActionMessage))]
    [ProtoInclude((int)MessageType.MoveUpdate, typeof(MoveMessage))]
    [ProtoInclude((int)MessageType.HandshakeInit, typeof(HandshakeInitMessage))]
    [ProtoInclude((int)MessageType.MapRequest, typeof(MapRequestMessage))]

    //server
    [ProtoInclude((int)MessageType.HandshakeReply, typeof(HandshakeReplyMessage))]
    [ProtoInclude((int)MessageType.MapReply, typeof(MapReplyMessage))]
    [ProtoInclude((int)MessageType.ObjectSeen, typeof(ObjectSeenMessage))]
    [ProtoInclude((int)MessageType.PlayerStatusUpdate, typeof(PlayerStatusMessage))]
    [ProtoInclude((int)MessageType.DamageEvent, typeof(DamageEventMessage))]

    //common
    [ProtoInclude((int)MessageType.SendChat, typeof(ChatMessage))]
    public class IOMessage
    {
        [ProtoMember(1)]
        public MessageType Type { get; protected set; }
    }

}
