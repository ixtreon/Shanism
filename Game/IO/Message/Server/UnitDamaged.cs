using IxSerializer.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Message.Server
{
    [SerialKiller]
    class UnitDamageMessage : IOMessage
    {
        public override MessageType Type
        {
            get { return MessageType.UnitDamage; }
        }

        [SerialMember]
        public readonly int UnitId;

        [SerialMember]
        public readonly float ValueChange;

        [SerialMember]
        public readonly float NewValue;

        [SerialMember]
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
