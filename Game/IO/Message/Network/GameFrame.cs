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
    public class GameFrameMessage : IOMessage
    {
        [ProtoMember(1)]
        readonly byte[] Data;


        GameFrameMessage() { Type = MessageType.GameFrame; }

        public GameFrameMessage(uint objGuid, ObjectType objType, byte[] objData)
            : this()
        {
            Data = objData;
        }
    }
}
