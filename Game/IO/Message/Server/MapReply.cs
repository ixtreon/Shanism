using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    /// <summary>
    /// A message containing map data reply
    /// </summary>
    [ProtoContract]
    public class MapReplyMessage : IOMessage
    {
        [ProtoMember(1)]
        public readonly MapChunkId Chunk;

        [ProtoMember(2)]
        public readonly ProtoArray<TerrainType> Data;

        private MapReplyMessage()
            : base(MessageType.MapReply)
        {

        }

        public MapReplyMessage(MapChunkId chunkId, TerrainType[,] data)
            : this()
        {
            Chunk = chunkId;
            Data = data.ToProtoArray<TerrainType>();
        }

        public TerrainType[,] GetData()
        {
            return (TerrainType[,])Data.ToArray();
        }
    }
}
