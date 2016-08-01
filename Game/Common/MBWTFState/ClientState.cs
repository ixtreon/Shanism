using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Client
{
    [ProtoContract]
    public class ClientState
    {
        [ProtoMember(1)]
        public bool IsMoving { get; set; }

        [ProtoMember(2)]
        public float MoveAngle { get; set; }


        [ProtoMember(3)]
        public uint ActionId { get; set; }

        [ProtoMember(4)]
        public uint ActionTargetId { get; set; }

        [ProtoMember(5)]
        public Vector ActionTargetLoc { get; set; }


        public void SetMovement(bool isMoving, float angle)
        {
            IsMoving = isMoving;
            MoveAngle = angle;
        }

        public void DoAction(uint id, uint targetId, Vector targetPos)
        {
            ActionId = id;
            ActionTargetId = targetId;
            ActionTargetLoc = targetPos;
        }
    }
}
