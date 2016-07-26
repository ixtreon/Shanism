﻿using Shanism.Common.Message;
using Lidgren.Network;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The header type used throughout. 
using HeaderType = System.Int32;

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

            var netMsg = peer.CreateMessage(sizeof(HeaderType) + bytes.Length);
            netMsg.Write((HeaderType)bytes.Length);
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
                msg.Position = 0;
                var len = msg.ReadInt32();
                var bytes = msg.ReadBytes(len);

                using (var ms = new MemoryStream(bytes))
                    return Serializer.Deserialize<IOMessage>(ms);
            }
            catch
            {
                return null;
            }

        }
    }
}
