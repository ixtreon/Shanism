using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IxSerializer.Modules;
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
    [SerialKiller]
    [SerialKid(typeof(ActionMessage), (short)MessageType.Action)]
    [SerialKid(typeof(MoveMessage), (short)MessageType.MoveUpdate)]
    [SerialKid(typeof(HandshakeInitMessage), (short)MessageType.HandshakeInit)]
    [SerialKid(typeof(MapRequestMessage), (short)MessageType.MapRequest)]

    [SerialKid(typeof(HandshakeReplyMessage), (short)MessageType.HandshakeReply)]
    [SerialKid(typeof(MapReplyMessage), (short)MessageType.MapReply)]
    [SerialKid(typeof(ObjectSeenMessage), (short)MessageType.ObjectSeen)]
    [SerialKid(typeof(PlayerStatusMessage), (short)MessageType.PlayerStatusUpdate)]
    [SerialKid(typeof(UnitDamageMessage), (short)MessageType.UnitDamage)]
    public abstract class IOMessage
    {
        public abstract MessageType Type { get; }
    }

}
