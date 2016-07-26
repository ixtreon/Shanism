using Shanism.Common.Game;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Client
{
    [ProtoContract]
    public class MoveMessage : IOMessage
    {
        public override MessageType Type => MessageType.MoveUpdate;


        [ProtoMember(1)]
        public readonly bool IsMoving;

        [ProtoMember(2)]
        public readonly byte ByteAngle;


        public double AngleRad => ByteAngle * Math.PI * 2 / 256;

        MoveMessage() { }

        public MoveMessage(MovementState st)
            : this()
        {
            IsMoving = st.IsMoving;
            unchecked { ByteAngle = (byte)(st.AngleRad / Math.PI / 2 * 256); }
        }
    }
}
