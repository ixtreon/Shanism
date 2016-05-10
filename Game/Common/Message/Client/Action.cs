using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common.Game;
using ProtoBuf;
using System.IO;

namespace Shanism.Common.Message.Client
{
    /// <summary>
    /// The message passed whenever the client wishes to perform an action
    /// </summary>
    [ProtoContract]
    public class ActionMessage : IOMessage
    {


        public bool HasTarget;


        /// <summary>
        /// The string id of the action being performed. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint AbilityId = 0;

        /// <summary>
        /// The Guid of the target, if there is one. 
        /// </summary>
        [ProtoMember(2)]
        public readonly uint TargetGuid = 0;

        /// <summary>
        /// The target location, if there is one. 
        /// </summary>
        [ProtoMember(3)]
        public readonly Vector TargetLocation;

        public override MessageType Type { get { return MessageType.Action; } }

        ActionMessage() { }

        /// <summary>
        /// Creates a new message for the specified action. 
        /// Validity of targets is determined by the server. 
        /// </summary>
        /// <param name="abilityId">The string id of the action to perform. </param>
        /// <param name="targetGuid">The target of the ability, if any. </param>
        /// <param name="targetLoc">The location this ability is cast towards. </param>
        public ActionMessage(uint abilityId, uint targetGuid, Vector targetLoc)
            : this()
        {
            TargetGuid = targetGuid;
            TargetLocation = targetLoc;
            AbilityId = abilityId;
        }
    }
}
