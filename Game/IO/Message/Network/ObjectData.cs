using IO.Common;
using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Network
{
    [ProtoContract]
    public class ObjectDataMessage : IOMessage
    {
        
        [ProtoMember(1)]
        readonly uint Guid;

        [ProtoMember(2)]
        readonly ObjectType ObjectType;

        [ProtoMember(3)]
        readonly byte[] Data;


        public override MessageType Type { get { return MessageType.ObjectData; } }

        ObjectDataMessage() { }

        public ObjectDataMessage(uint objGuid, ObjectType objType, byte[] objData)
            : this()
        {
            Guid = objGuid;
            //ObjectType = objType;
            Data = objData;
        }
    }
}
