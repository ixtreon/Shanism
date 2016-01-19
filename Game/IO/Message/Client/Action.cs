using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using IxSerializer.Attributes;
using System.IO;

namespace IO.Message.Client
{
    /// <summary>
    /// The message passed whenever the client wishes to perform an action
    /// </summary>
    [SerialKiller]
    public class ActionMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.Action; }
        }

        public bool HasTarget;


        /// <summary>
        /// The string id of the action being performed. 
        /// </summary>
        [SerialMember]
        public readonly string AbilityId = string.Empty;

        /// <summary>
        /// The Guid of the target, if there is one. 
        /// </summary>
        [SerialMember]
        public readonly uint TargetGuid = 0;

        /// <summary>
        /// The target location, if there is one. 
        /// </summary>
        [SerialMember]
        public readonly Vector TargetLocation;

        ActionMessage() { }

        /// <summary>
        /// Creates a new message for the specified action. 
        /// Validity of targets is determined by the server. 
        /// </summary>
        /// <param name="abilityId">The string id of the action to perform. </param>
        /// <param name="targetGuid">The target of the ability, if any. </param>
        /// <param name="targetLoc">The location this ability is cast towards. </param>
        public ActionMessage(string abilityId, uint targetGuid, Vector targetLoc)
        {
            TargetGuid = targetGuid;
            TargetLocation = targetLoc;
            AbilityId = abilityId;
        }
    }
}
