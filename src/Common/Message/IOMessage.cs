using ProtoBuf;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// Represents any command or message sent by the server to the client. 
    /// 
    /// Implements (de)serialization using ProtoBuf on registered classes. 
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude((int)ServerMessageType.HandshakeReply, typeof(HandshakeReply))]
    [ProtoInclude((int)ServerMessageType.PlayerStatus, typeof(PlayerStatus))]
    [ProtoInclude((int)ServerMessageType.MapData, typeof(MapData))]
    [ProtoInclude((int)ServerMessageType.DamageEvent, typeof(DamageEvent))]
    [ProtoInclude((int)ServerMessageType.Chat, typeof(ServerChat))]
    [ProtoInclude((int)ServerMessageType.Disconnected, typeof(Disconnected))]
    public abstract class ServerMessage
    {
        public abstract ServerMessageType Type { get; }
    }


    /// <summary>
    /// Represents any command or message sent by the client to the server. 
    /// 
    /// Implements (de)serialization using ProtoBuf on registered classes. 
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude((int)ClientMessageType.HandshakeInit, typeof(HandshakeInit))]
    [ProtoInclude((int)ClientMessageType.MapRequest, typeof(MapRequest))]
    [ProtoInclude((int)ClientMessageType.Chat, typeof(ClientChat))]
    public abstract class ClientMessage
    {
        public abstract ClientMessageType Type { get; }
    }

}
