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
    /// To register a class add it using the <see cref="AddMessageType{T}(MessageType)"/> static method. 
    /// </summary>
    [EnumMember(typeof(ActionMessage), (short)MessageType.Action)]
    [EnumMember(typeof(MoveMessage), (short)MessageType.MoveUpdate)]
    [EnumMember(typeof(HandshakeInitMessage), (short)MessageType.HandshakeInit)]
    [EnumMember(typeof(MapRequestMessage), (short)MessageType.MapRequest)]

    [EnumMember(typeof(MapReplyMessage), (short)MessageType.MapReply)]
    [EnumMember(typeof(HandshakeReplyMessage), (short)MessageType.HandshakeReply)]
    [EnumMember(typeof(PlayerStatusMessage), (short)MessageType.PlayerStatusUpdate)]
    [EnumMember(typeof(UnitDamageMessage), (short)MessageType.UnitDamage)]
    public abstract class IOMessage : EnumBase<IOMessage>
    {
        public MessageType Type { get { return (MessageType)TypeId; } }
    }

}
