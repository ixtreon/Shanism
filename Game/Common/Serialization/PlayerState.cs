using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Message.Client
{
    /// <summary>
    /// Represents the in-game state of a client. 
    /// Linked with (in SP) or beamed to (in MP) the server at the default frequency.
    /// </summary>
    [ProtoContract]
    public class PlayerState
    {
        const float byteAngleAdd = (float)Math.PI;
        const float byteAngleToFloat = (float)Math.PI * 2 / 256;

        [ProtoMember(1)]
        public bool IsMoving { get; set; }

        /// <summary>
        /// Gets or sets the angle at which this client's main hero is moving.
        /// </summary>
        public float MoveAngle
        {
            get { return (moveAngleByte * byteAngleToFloat) - byteAngleAdd; }
            set { moveAngleByte = (byte)((value + byteAngleAdd) / byteAngleToFloat); }
        }


        [ProtoMember(2)]
        byte moveAngleByte;

        [ProtoMember(3)]
        public uint ActionId { get; set; }

        [ProtoMember(4)]
        public uint ActionTargetId { get; set; }

        [ProtoMember(5)]
        public Vector ActionTargetLocation { get; set; }

        public void SetMovement(bool isMoving, float angle)
        {
            IsMoving = isMoving;
            MoveAngle = angle;
        }

        public void DoAction(uint id, uint targetId, Vector targetPos)
        {
            ActionId = id;
            ActionTargetId = targetId;
            ActionTargetLocation = targetPos;
        }
    }
}
