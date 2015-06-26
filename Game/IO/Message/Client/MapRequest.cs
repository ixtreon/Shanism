using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Client
{
    [ProtoContract]
    public class MapRequestMessage : IOMessage
    {
        [ProtoMember(1)]
        public MapChunkId Chunk;

        private MapRequestMessage() { }

        public MapRequestMessage(MapChunkId chunk)
        {
            this.Chunk = chunk;
        }
    }
}
