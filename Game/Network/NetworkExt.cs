using IO.Message;
using IxSerializer;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The header type used throughout. 
using HeaderType = System.Int32;

namespace Network
{
    static class NetworkExt
    {
        public static NetOutgoingMessage ToNetMessage(this IOMessage ioMsg, NetPeer peer)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var buf = new BinaryWriter(ms))
                    Serializer.TryWrite(buf, ioMsg);
                bytes = ms.ToArray();
            }

            var netMsg = peer.CreateMessage(sizeof(HeaderType) + bytes.Length);
            netMsg.Write((HeaderType)bytes.Length);
            netMsg.Write(bytes);

            return netMsg;
        }

        public static IOMessage ToIOMessage(this NetIncomingMessage msg)
        {
            var len = msg.ReadInt32();
            var bytes = msg.ReadBytes(len);

            using (var ms = new MemoryStream(bytes))
            using (var buf = new BinaryReader(ms))
                return Serializer.Read<IOMessage>(buf);
        }
    }
}
