using IO.Content;
using IO.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    [ProtoContract]
    class ObjectAnimationMessage : IOMessage
    {
        [ProtoMember(1)]
        public readonly int UnitId;

        [ProtoMember(2)]
        public readonly string AnimationId;

        [ProtoMember(3)]
        public readonly bool IsLooping;


        public override MessageType Type { get { return MessageType.ObjectAnimation; } }

        ObjectAnimationMessage() { }

        public ObjectAnimationMessage(IUnit u, AnimationDef anim)
            : this()
        {

        }
    }
}
