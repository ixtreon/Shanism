using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Messages
{
    [ProtoContract]
    public class MapRequest : ClientMessage
    {
        
        [ProtoMember(1)]
        public ChunkId Chunk;

        public override ClientMessageType Type => ClientMessageType.MapRequest;
        MapRequest() { }

        public MapRequest(ChunkId chunk)
            : this()
        {
            this.Chunk = chunk;
        }
    }
}
