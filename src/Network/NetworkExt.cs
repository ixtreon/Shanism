using Shanism.Common.Message;
using Lidgren.Network;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Network
{
    static class NetworkExt
    {
        /// <summary>
        /// Converts a shano message of type <see cref="IOMessage"/> to one suitable for use by the network library. 
        /// </summary>
        /// <param name="ioMsg">The IOMessage.</param>
        /// <param name="peer">The peer.</param>
        /// <returns></returns>
        public static NetOutgoingMessage ToNetMessage(this IOMessage ioMsg, NetPeer peer)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, ioMsg);
                bytes = ms.ToArray();
            }

            var netMsg = peer.CreateMessage(sizeof(int) + bytes.Length);
            netMsg.Write((byte)0);
            netMsg.Write((int)bytes.Length);
            netMsg.Write(bytes);

            return netMsg;
        }

        /// <summary>
        /// Converts a message coming from the network library to a shano message of type <see cref="IOMessage"/>. 
        /// </summary>
        /// <param name="msg">The network message.</param>
        /// <returns></returns>
        public static IOMessage ToIOMessage(this NetIncomingMessage msg)
        {
            try
            {
                var len = msg.ReadInt32();
                var bytes = msg.ReadBytes(len);

                using (var ms = new MemoryStream(bytes))
                    return Serializer.Deserialize<IOMessage>(ms);
            }
            catch(Exception e)
            {
                Log.Default.Error("Received a faulty message...");
                return null;
            }

        }
    }
}
