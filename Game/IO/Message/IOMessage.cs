using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IxSerializer.Attributes;
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

    //client
    [SerialKid(typeof(ActionMessage), (short)MessageType.Action)]
    [SerialKid(typeof(MoveMessage), (short)MessageType.MoveUpdate)]
    [SerialKid(typeof(HandshakeInitMessage), (short)MessageType.HandshakeInit)]
    [SerialKid(typeof(MapRequestMessage), (short)MessageType.MapRequest)]

    //server
    [SerialKid(typeof(HandshakeReplyMessage), (short)MessageType.HandshakeReply)]
    [SerialKid(typeof(MapReplyMessage), (short)MessageType.MapReply)]
    [SerialKid(typeof(ObjectSeenMessage), (short)MessageType.ObjectSeen)]
    [SerialKid(typeof(PlayerStatusMessage), (short)MessageType.PlayerStatusUpdate)]
    [SerialKid(typeof(UnitDamagedMessage), (short)MessageType.UnitDamage)]

    //common
    [SerialKid(typeof(ChatMessage), (short)MessageType.SendChat)]
    public abstract class IOMessage
    {
        public abstract MessageType Type { get; }
    }

}
