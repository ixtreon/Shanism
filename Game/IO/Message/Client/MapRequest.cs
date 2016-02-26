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

        public override MessageType Type { get { return MessageType.MapRequest; } }

        MapRequestMessage() { }

        public MapRequestMessage(MapChunkId chunk)
            : this()
        {
            this.Chunk = chunk;
        }
    }
}
