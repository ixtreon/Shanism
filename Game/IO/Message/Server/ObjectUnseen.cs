using IO.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    /// <summary>
    /// The server informs a client he won't receive updates for this gameobject cuz he can't see it. 
    /// </summary>
    [ProtoContract]
    public class ObjectUnseenMessage : IOMessage
    {
        
        /// <summary>
        /// Gets the GUID of the game object. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint ObjectId;

        public override MessageType Type { get { return MessageType.ObjectUnseen; } }

        ObjectUnseenMessage() { }

        public ObjectUnseenMessage(uint guid)
            :this()
        {
            ObjectId = guid;
        }
    }
}
