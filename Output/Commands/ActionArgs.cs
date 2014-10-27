using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO.Common;
using ProtoBuf;

namespace IO.Commands
{
    [ProtoContract]
    public class ActionArgs : CommandArgs
    {
        [ProtoMember(1)]
        public readonly AbilityType Type;
        [ProtoMember(2)]
        public readonly string AbilityId;
        [ProtoMember(3)]
        public readonly int TargetGuid;
        [ProtoMember(4)]
        public readonly Vector TargetLocation;

        /// <summary>
        /// Create args for object-target spells
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="targetGuid"></param>
        public ActionArgs(string abilityId, int targetGuid)
            : base(CommandType.Ability)
        {
            Type = AbilityType.UnitTarget;
            this.AbilityId = abilityId;
            this.TargetGuid = targetGuid;
        }

        /// <summary>
        /// Create args for point-target spells
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="targetLoc"></param>
        public ActionArgs(string abilityId, Vector targetLoc)
            : base(CommandType.Ability)
        {
            Type = AbilityType.PointTarget;
            this.AbilityId = abilityId;
            this.TargetLocation = targetLoc;
        }

        /// <summary>
        /// Create args for instant cast or passive spells. 
        /// </summary>
        /// <param name="abilityId"></param>
        /// <param name="passive"></param>
        public ActionArgs(string abilityId, bool passive)
            : base(CommandType.Ability)
        {
            Type = passive ? AbilityType.Passive : AbilityType.NoTarget;
            this.AbilityId = abilityId;
        }
    }
}
