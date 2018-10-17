using ProtoBuf;
using System;
using System.Numerics;

namespace Shanism.Common.Messages
{
    /// <summary>
    /// The game state of a client once playing on a server.
    /// Can be serialized into a client game frame. 
    /// </summary>
    [ProtoContract]
    public class PlayerState
    {
        const float byteAngleAdd = (float)Math.PI;
        const float byteAngleToFloat = (float)Math.PI * 2 / 256;

        /// <summary>
        /// Gets or sets whether the client is currently moving.
        /// </summary>
        [ProtoMember(1)]
        public bool IsMoving { get; private set; }

        /// <summary>
        /// Gets or sets the angle at which this client's main hero is moving.
        /// </summary>
        public float MoveAngle
        {
            get => (moveAngleByte * byteAngleToFloat) - byteAngleAdd;
            private set => moveAngleByte = (byte)((value + byteAngleAdd) / byteAngleToFloat);
        }


        [ProtoMember(2)]
        byte moveAngleByte;

        /// <summary>
        /// Gets the ID of the action (spell) currently being performed (cast).
        /// Set to zero if no action is currently being performed.
        /// </summary>
        [ProtoMember(3)]
        public uint ActionId { get; private set; }

        /// <summary>
        /// Gets the ID of the current action.
        /// Ignored if no action is currently being performed.
        /// </summary>
        [ProtoMember(4)]
        public uint ActionTargetId { get; private set; }

        /// <summary>
        /// Gets the target location of the current 
        /// Ignored if no action is currently being performed.
        /// </summary>
        [ProtoMember(5)]
        public Vector2 ActionTargetLocation { get; private set; }

        [ProtoMember(6)]
        public Vector2 CursorPosition { get; set; }

        public void SetMovement(float? angle)
        {
            IsMoving = angle != null;
            MoveAngle = angle ?? 0;
        }

        public void DoAction(uint id, uint targetId, Vector2 targetPos)
        {
            ActionId = id;
            ActionTargetId = targetId;
            ActionTargetLocation = targetPos;
        }

        public void CancelAction()
        {
            ActionId = 0;
        }
    }
}
