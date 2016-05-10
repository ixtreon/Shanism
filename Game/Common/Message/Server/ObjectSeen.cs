using Shanism.Common.Game;
using Shanism.Common.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Server
{
    /// <summary>
    /// The message sent by the server when a client sees an object. 
    /// When playing local the field <see cref="Object"/> is set so the client can use it directly. 
    /// <para/>
    /// </summary>
    [ProtoContract]
    public class ObjectSeenMessage : IOMessage
    {

        /// <summary>
        /// The unique identifier of the object that came into range. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint ObjectId;

        [ProtoMember(2)]
        public readonly ObjectType ObjectType;

        /// <summary>
        /// The object that came in range. 
        /// </summary>
        public readonly IEntity Object;

        public override MessageType Type { get { return MessageType.ObjectSeen; } }

        ObjectSeenMessage() { }

        public ObjectSeenMessage(IEntity obj)
            : this()
        {
            Object = obj;
            ObjectId = obj.Id;
            ObjectType = obj.ObjectType;
        }
    }
}
