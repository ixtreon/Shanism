using Ix.Math;
using ProtoBuf;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// A message containing the reply for a <see cref="MapRequest"/>. 
    /// Contains a flag which tells if there's map at the given chunk, and if so the actual data. 
    /// </summary>
    [ProtoContract]
    public class MapData : ServerMessage
    {
        public override ServerMessageType Type => ServerMessageType.MapData;


        [ProtoMember(1)]
        public readonly Rectangle Span;

        [ProtoMember(2)]
        public readonly TerrainType[] Data;

        MapData() { }


        /// <summary>
        /// Creates a new map reply message informing a client of the terrain in a chunk. 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="span">The in-game span of the requested data. </param>
        public MapData(Rectangle span, TerrainType[] data)
            : this()
        {
            Span = span;
            Data = data;
        }
    }
}
