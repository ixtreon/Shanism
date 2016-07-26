using Shanism.Common.Game;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Common.Message.Server
{
    [ProtoContract]
    public class DamageEventMessage : IOMessage
    {
        

        /// <summary>
        /// The ID of the unit that was damaged. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint UnitId;

        /// <summary>
        /// The change in zzz.
        /// </summary>
        [ProtoMember(2)]
        public readonly float ValueChange;

        [ProtoMember(3)]
        public readonly float NewValue;

        [ProtoMember(4)]
        public readonly bool IsHealth;

        [ProtoMember(5)]
        public readonly DamageType DamageType;


        public float OldValue
        {
            get { return NewValue - ValueChange; }
        }

        public override MessageType Type { get { return MessageType.DamageEvent; } }

        DamageEventMessage() { }

        public DamageEventMessage(IUnit target, DamageType dmgType, double change, bool isHealth)
            : this()
        {
            UnitId = target.Id;
            DamageType = dmgType;
            IsHealth = isHealth;
            ValueChange = (float)change;

            if (isHealth)
                NewValue = (float)target.Life;
            else
                NewValue = (float)target.Mana;
        }
    }
}
