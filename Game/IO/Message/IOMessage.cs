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
using IO.Message.Network;

namespace IO.Message
{
    /// <summary>
    /// Represents any command or message sent between the server and the client. 
    /// 
    /// Implements (de)serialization using ProtoBuf on registered classes. 
    /// </summary>
    [ProtoContract(SkipConstructor = true)]

    //client
    [ProtoInclude((int)MessageType.HandshakeInit, typeof(HandshakeInitMessage))]
    [ProtoInclude((int)MessageType.MapRequest, typeof(MapRequestMessage))]
    [ProtoInclude((int)MessageType.Action, typeof(ActionMessage))]
    [ProtoInclude((int)MessageType.MoveUpdate, typeof(MoveMessage))]

    //server
    [ProtoInclude((int)MessageType.HandshakeReply, typeof(HandshakeReplyMessage))]
    [ProtoInclude((int)MessageType.PlayerStatusUpdate, typeof(PlayerStatusMessage))]
    [ProtoInclude((int)MessageType.MapReply, typeof(MapDataMessage))]
    [ProtoInclude((int)MessageType.ObjectSeen, typeof(ObjectSeenMessage))]
    [ProtoInclude((int)MessageType.ObjectUnseen, typeof(ObjectUnseenMessage))]
    [ProtoInclude((int)MessageType.DamageEvent, typeof(DamageEventMessage))]

    //common?
    [ProtoInclude((int)MessageType.SendChat, typeof(ChatMessage))]

    //network
    [ProtoInclude((int)MessageType.GameFrame, typeof(GameFrameMessage))]
    public abstract class IOMessage
    {
        public abstract MessageType Type { get; }
    }

}
