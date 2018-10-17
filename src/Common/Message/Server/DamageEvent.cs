using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Entities;

namespace Shanism.Common.Messages
{
    [ProtoContract]
    public class DamageEvent : ServerMessage
    {
        public override ServerMessageType Type => ServerMessageType.DamageEvent;

        /// <summary>
        /// The ID of the unit that was damaged. 
        /// </summary>
        [ProtoMember(1)]
        public readonly uint UnitId;

        /// <summary>
        /// The change in the tracked property.
        /// </summary>
        [ProtoMember(2)]
        public readonly float ValueChange;

        /// <summary>
        /// The value of the property after it changed.
        /// </summary>
        [ProtoMember(3)]
        public readonly float NewValue;

        /// <summary>
        /// Whether the change was in the unit's health or mana.
        /// </summary>
        [ProtoMember(4)]
        public readonly bool IsHealth;

        /// <summary>
        /// The type of damage that caused this event.
        /// </summary>
        [ProtoMember(5)]
        public readonly DamageType DamageType;

        /// <summary>
        /// The value of the property before it changed.
        /// </summary>
        public float OldValue => NewValue - ValueChange;

        DamageEvent() { }

        public DamageEvent(IUnit target, DamageType dmgType, float change, bool isHealth)
            : this()
        {
            UnitId = target.Id;
            DamageType = dmgType;
            IsHealth = isHealth;
            ValueChange = change;

            if (isHealth)
                NewValue = target.Life;
            else
                NewValue = target.Mana;
        }
    }
}
