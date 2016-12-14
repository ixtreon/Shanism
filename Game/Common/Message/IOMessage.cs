using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using Shanism.Common.Message.Client;
using System.Reflection;
using Shanism.Common.Message.Server;
using System.Threading;
using Shanism.Common.Message.Network;

namespace Shanism.Common.Message
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
    [ProtoInclude((int)MessageType.ClientChat, typeof(Client.ChatMessage))]

    //server
    [ProtoInclude((int)MessageType.HandshakeReply, typeof(HandshakeReplyMessage))]
    [ProtoInclude((int)MessageType.PlayerStatusUpdate, typeof(PlayerStatusMessage))]
    [ProtoInclude((int)MessageType.MapReply, typeof(MapDataMessage))]
    [ProtoInclude((int)MessageType.ServerChat, typeof(Server.ChatMessage))]
    [ProtoInclude((int)MessageType.DamageEvent, typeof(DamageEventMessage))]
    [ProtoInclude((int)MessageType.Disconnected, typeof(DisconnectedMessage))]

    //common?

    public abstract class IOMessage
    {
        public abstract MessageType Type { get; }
    }

}
