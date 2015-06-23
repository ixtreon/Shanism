using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using IO.Message.Client;
using System.Reflection;
using IO.Message.Server;
namespace IO.Message
{
    /// <summary>
    /// Represents any command or message sent between the server and the client. 
    /// 
    /// Implements (de)serialization using ProtoBuf on registered classes. 
    /// To register a class add it using the <see cref="AddMessageType{T}(MessageType)"/> static method. 
    /// </summary>
    public abstract class IOMessage
    {
        /// <summary>
        /// Link message types here. 
        /// </summary>
        static IOMessage()
        {
            //client
            AddMessageType<ActionMessage>(MessageType.Action);
            AddMessageType<MoveMessage>(MessageType.MovementUpdate);
            AddMessageType<HandshakeInitMessage>(MessageType.HandshakeInit);
            AddMessageType<MapRequestMessage>(MessageType.MapRequest);

            //server
            AddMessageType<MapReplyMessage>(MessageType.MapReply);
            AddMessageType<HandshakeReplyMessage>(MessageType.HandshakeReply);
            AddMessageType<PlayerStatusMessage>(MessageType.PlayerStatusUpdate);
            AddMessageType<MapReplyMessage>(MessageType.MapReply);
            AddMessageType<UnitDamageMessage>(MessageType.UnitDamage);
        }

        #region Static methods and fields
        /// <summary>
        /// The length of the header. Currently contains only <see cref="Type"/> which is serialized to a short. 
        /// </summary>
        const int HEADER_SZ = 2;

        static readonly Dictionary<MessageType, Func<Stream, IOMessage>> deserializeDict = new Dictionary<MessageType, Func<Stream, IOMessage>>();
        static readonly Dictionary<MessageType, Action<Stream, IOMessage>> serializeDict = new Dictionary<MessageType, Action<Stream, IOMessage>>();

        /// <summary>
        /// Specifies that the given MessageType should be deserialized to the provided class inheriting from IOMessage. 
        /// </summary>
        /// <typeparam name="T">The type to deserialize to. </typeparam>
        /// <param name="ty">The message corresponding to the type. </param>
        public static void AddMessageType<T>(MessageType ty)
            where T : IOMessage
        {
            deserializeDict[ty] = (ms) => Serializer.Deserialize<T>(ms);
            serializeDict[ty] = (ms, it) => Serializer.Serialize<T>(ms, (T)it);
        }

        static void Serialize(MemoryStream ms, IOMessage val)
        {
            if (!serializeDict.ContainsKey(val.Type))
                throw new InvalidOperationException("Unable to serialize the message type '{0}'. Please add the message type using the AddMessageType method!");

            short sType = (short)val.Type;
            ms.Write(BitConverter.GetBytes(sType), 0, HEADER_SZ);

            serializeDict[val.Type](ms, val);
        }

        public static IOMessage Deserialize(byte[] bytes)
        {
            var type = (MessageType)BitConverter.ToInt16(bytes, 0);

            using (var ms = new MemoryStream(bytes, HEADER_SZ, bytes.Length - HEADER_SZ))
                return deserializeDict[type](ms);
        }

        #endregion


        public MessageType Type { get; private set; }


        protected IOMessage(MessageType ty)
        {
            this.Type = ty;
        }


        public byte[] Serialize()
        {
            using(var ms = new MemoryStream())
            {
                Serialize(ms, this);
                return ms.ToArray();
            }
        }

    }
}
