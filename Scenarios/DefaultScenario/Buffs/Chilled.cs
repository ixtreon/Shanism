using Engine.Objects;
using Engine.Systems.Buffs;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultScenario.Buffs
{
    class ChilledBuff : Buff
    {
        public ChilledBuff(int duration = 5000)
            : base(BuffType.NonStacking, duration)
        {
            Name = "Chilled";
            RawDescription = "Slows the unit's movement by {MoveSpeed:0;0}%, its attack speed by {AttackSpeed:0;0}%, but also provides {strength} strength. ";


            MoveSpeed = -190;
            AttackSpeed = -25;
            Strength = 5;
        }
    }
}
