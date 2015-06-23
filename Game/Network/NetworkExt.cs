using IO.Message;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HeaderType = System.Int32;

namespace Network
{
    static class NetworkExt
    {
        public static NetOutgoingMessage ToNetMessage(this IOMessage ioMsg, NetPeer peer)
        {
            var bytes = ioMsg.Serialize();
            var netMsg = peer.CreateMessage(sizeof(HeaderType) + bytes.Length);

            netMsg.Write((HeaderType)bytes.Length);
            netMsg.Write(bytes);

            return netMsg;
        }

        public static IOMessage ToIOMessage(this NetIncomingMessage msg)
        {
            var len = msg.ReadInt32();
            var bytes = msg.ReadBytes(len);
            return IOMessage.Deserialize(bytes);
        }
    }
}
