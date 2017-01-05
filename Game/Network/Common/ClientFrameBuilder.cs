using Shanism.Common.Message.Network;
using Shanism.Common.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Shanism.Common.Message.Client;
using Lidgren.Network;

namespace Shanism.Network.Common
{
    /// <summary>
    /// Reads and writes <see cref="ClientState"/> frames.
    /// </summary>
    public class ClientFrameBuilder
    {
        public NetOutgoingMessage Write(NetPeer srv, uint gameFrame, ClientState state)
        {
            NetOutgoingMessage msg;
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, state);

                msg = srv.CreateMessage(4 + (int)ms.Length);
                msg.Write(gameFrame);
                msg.Write(ms.ToArray());
            }
            return msg;
        }

        public bool TryRead(NetBuffer msg,
            out uint clientFrameId,
            out ClientState state)
        {
            try
            {
                ClientState state2;
                using (var ms = new MemoryStream(msg.Data))
                {
                    clientFrameId = ms.ReadUint24();
                    state2 = ProtoBuf.Serializer.Deserialize<ClientState>(ms);
                }

                //var state3 = new ClientState();
                //using (var ms = new MemoryStream(msg.Data))
                //{
                //    ProtoBuf.Serializer.Merge(ms, state3);
                //}

                clientFrameId = 0;
                state = state2;
                return true;
            }
            catch
            {
                clientFrameId = 0;
                state = null;
                return false;
            }
        }

    }
}
