using IO.Common;
using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    /// <summary>
    /// A message containing map data reply
    /// </summary>
    [SerialKiller]
    public class MapReplyMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.MapReply; }
        }

        [SerialMember]
        public readonly MapChunkId Chunk;

        [SerialMember]
        public readonly TerrainType[,] Data;

        private MapReplyMessage() { }

        public MapReplyMessage(MapChunkId chunkId, TerrainType[,] data)
            : this()
        {
            Chunk = chunkId;
            Data = data;
        }
    }
}
