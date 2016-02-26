using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Message.Server
{
    /// <summary>
    /// A message containing the reply for a <see cref="Client.MapRequestMessage"/>. 
    /// Contains a flag which tells if there's map at the given chunk, and if so the actual data. 
    /// </summary>
    [ProtoContract]
    public class MapDataMessage : IOMessage
    {
        

        [ProtoMember(1)]
        public readonly Rectangle Span;

        [ProtoMember(3)]
        public readonly TerrainType[] Data;

        [ProtoMember(2)]
        public readonly bool HasMap;

        public override MessageType Type { get { return MessageType.MapReply; } }

        MapDataMessage() { }

        /// <summary>
        /// Creates a new map reply message informing a client there's no map on the given chunk. 
        /// </summary>
        public MapDataMessage(Rectangle span)
        {
            Span = span;
            HasMap = false;
        }

        /// <summary>
        /// Creates a new map reply message informing a client of the terrain in a chunk. 
        /// </summary>
        /// <param name="data"></param>
        public MapDataMessage(Rectangle span, TerrainType[] data)
            : this()
        {
            Span = span;
            HasMap = true;
            Data = data;
        }
    }
}
