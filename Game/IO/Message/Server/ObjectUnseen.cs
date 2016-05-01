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
        /// Gets the type of the message. 
        /// Always returns <see cref="MessageType.ObjectUnseen"/>. 
        /// </summary>
        public override MessageType Type => MessageType.ObjectUnseen;


        /// <summary>
        /// Gets the GUID of the game object. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint ObjectId;

        /// <summary>
        /// Gets whether the unit was removed from the map, 
        /// in addition to being unseen. 
        /// </summary>
        [ProtoMember(2)]
        public readonly bool IsDestroyed;


        ObjectUnseenMessage() { }

        public ObjectUnseenMessage(uint guid, bool isDestroyed)
            :this()
        {
            IsDestroyed = isDestroyed;
            ObjectId = guid;
        }
    }
}
