using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using IxSerializer.Modules;
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
        /// The type of the action that is being performed. 
        /// </summary>
        [SerialMember]
        public readonly AbilityTargetType TargetType;

        /// <summary>
        /// The string id of the action being performed. 
        /// </summary>
        [SerialMember]
        public readonly string AbilityId;

        /// <summary>
        /// The Guid of the target, if there is one. 
        /// </summary>
        [SerialMember]
        public readonly int TargetGuid;

        /// <summary>
        /// The target location, if there is one. 
        /// </summary>
        [SerialMember]
        public readonly Vector TargetLocation;

        private ActionMessage() { }

        /// <summary>
        /// Creates a new message for the specified unit-targeted action. 
        /// </summary>
        /// <param name="abilityId">The string id of the action to perform. </param>
        /// <param name="targetGuid"></param>
        public ActionMessage(string abilityId, int targetGuid)
        {
            this.HasTarget = true;
            TargetType = AbilityTargetType.UnitTarget;
            this.TargetGuid = targetGuid;

            this.AbilityId = abilityId;
        }

        /// <summary>
        /// Creates a new message for the specified point-targeted action. 
        /// </summary>
        /// <param name="abilityId">The string id of the action to perform. </param>
        /// <param name="targetLoc"></param>
        public ActionMessage(string abilityId, Vector targetLoc)
            : this()
        {
            this.HasTarget = true;
            TargetType = AbilityTargetType.PointTarget;
            this.TargetLocation = targetLoc;

            this.AbilityId = abilityId;
        }

        /// <summary>
        /// Creates a new message for the specified no-target action. 
        /// </summary>
        /// <param name="abilityId">The string id of the action to perform. </param>
        public ActionMessage(string abilityId)
            : this()
        {
            this.HasTarget = false;

            this.AbilityId = abilityId;
        }
    }
}
