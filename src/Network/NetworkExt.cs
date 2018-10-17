using Lidgren.Network;
using ProtoBuf;
using Shanism.Network.ProtoBuf;
using System;
using System.IO;

namespace Shanism.Network
{
    static class NetworkExt
    {
        static NetworkExt()
        {
            ProtoConfig.Initialize();
        }

        /// <summary>
        /// Serializes an object to a NetMessage.
        /// <para/>
        /// Puts a message header, too; see <see cref="NetMessageHeader"/>
        /// </summary>
        public static NetOutgoingMessage ToNetMessage<T>(this T ioMsg, NetPeer peer)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, ioMsg);
                bytes = ms.ToArray();
            }

            var netMsg = peer.CreateMessage(sizeof(int) + bytes.Length);
            netMsg.Write((byte)NetMessageHeader.ProtoMessage);
            netMsg.Write((int)bytes.Length);
            netMsg.Write(bytes);

            return netMsg;
        }

        /// <summary>
        /// Deserializes a message coming from the network library to an object.
        /// <para/>
        /// Assumes the message header is already read.
        /// </summary>
        public static T ProtoDeserialize<T>(this NetIncomingMessage msg)
        {
            try
            {
                var len = msg.ReadInt32();
                var bytes = msg.ReadBytes(len);

                using (var ms = new MemoryStream(bytes))
                    return Serializer.Deserialize<T>(ms);
            }
            catch (Exception e)
            {
                NetLog.Default.Error($"Error decoding a message: {e.Message}");
                return default(T);
            }

        }
    }
}
