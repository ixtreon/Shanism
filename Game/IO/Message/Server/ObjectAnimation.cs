using IO.Content;
using IxSerializer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    [SerialKiller]
    class ObjectAnimationMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.ObjectAnimation; }
        }

        [SerialMember]
        public readonly int UnitId;

        [SerialMember]
        public readonly string AnimationId;

        [SerialMember]
        public readonly bool IsLooping;


        private ObjectAnimationMessage() { }

        public ObjectAnimationMessage(IUnit u, AnimationDefOld anim)
        {

        }
    }
}
