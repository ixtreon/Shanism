using IO.Common;
using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    /// <summary>
    /// The message sent by the server when a client sees an object. 
    /// 
    /// </summary>
    [ProtoContract]
    public class ObjectSeenMessage : IOMessage
    {

        /// <summary>
        /// The unique identifier of the object that came into range. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint Guid;

        /// <summary>
        /// The object that came in range. 
        /// </summary>
        public readonly IGameObject Object;

        ObjectSeenMessage() { Type = MessageType.ObjectSeen; }

        public ObjectSeenMessage(IGameObject obj)
            : this()
        {
            Object = obj;
            Guid = obj.Id;
        }
    }
}
