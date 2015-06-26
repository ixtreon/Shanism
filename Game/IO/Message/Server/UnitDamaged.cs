using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    class UnitDamageMessage : IOMessage
    {
        [ProtoMember(1)]
        public readonly int UnitId;

        [ProtoMember(2)]
        public readonly float ValueChange;

        [ProtoMember(3)]
        public readonly float NewValue;

        [ProtoMember(4)]
        public readonly bool IsHealth;


        public float OldValue
        {
            get { return NewValue - ValueChange; }
        }

        private UnitDamageMessage() { }

        public UnitDamageMessage(IUnit u, double change, bool isHealth)
        {
            UnitId = u.Guid;
            IsHealth = isHealth;
            ValueChange = (float)change;

            if (isHealth)
                NewValue = (float)u.Life;
            else
                NewValue = (float)u.Mana;
        }
    }
}
