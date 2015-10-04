using IO.Common;
using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    //Currently used to send game object data since serializer is coupled with stub objects implemented in network. 
    [SerialKiller]
    public class ObjectSeenMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.ObjectSeen; }
        }

        [SerialMember]
        public readonly ObjectType ObjectType;

        [SerialMember]
        public readonly int Guid;

        [SerialMember]
        public readonly byte[] Data;

        public ObjectSeenMessage() { }

        public ObjectSeenMessage(ObjectType objectType, int guid, byte[] data)
        {
            ObjectType = objectType;
            Guid = guid;
            Data = data;
        }
    }
}
