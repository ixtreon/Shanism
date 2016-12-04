using Shanism.Common.Message.Network;
using Shanism.Common.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Shanism.Common.Message.Client;

namespace Shanism.Network.Common
{
    public class ClientFrameBuilder
    {
        readonly GameSerializer serializer = new GameSerializer();

        ObjectBitmaskBuilder bitmaskBuilder = new ObjectBitmaskBuilder();

        public GameFrameMessage Write(uint gameFrame, ClientState state)
        {
            using (var ms = new MemoryStream())
            {
                ms.WriteUint24(gameFrame);
                ProtoBuf.Serializer.Serialize(ms, state);

                var bytes = ms.ToArray();
                return new GameFrameMessage(0, bytes);
            }
        }

        public bool TryRead(GameFrameMessage msg,
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
