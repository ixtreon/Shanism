using IO.Common;
using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Client
{
    [SerialKiller]
    public class MapRequestMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.MapRequest; }
        }

        [SerialMember]
        public MapChunkId Chunk;

        private MapRequestMessage() { }

        public MapRequestMessage(MapChunkId chunk)
        {
            this.Chunk = chunk;
        }
    }
}
