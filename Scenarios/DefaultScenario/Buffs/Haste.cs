using Engine.Entities;
using Engine.Systems.Buffs;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultScenario.Buffs
{
    class Haste : Buff
    {
        public Haste(int duration = 5000)
        {
            Type = BuffType.NonStacking;
            FullDuration = duration;

            Name = "Haste";
            RawDescription = "Increases the unit's movement speed by {MoveSpeed:0;0}% and its attack speed by {AttackSpeed:0;0}%. ";
            Icon = "enchant-orange-3";

            MoveSpeedPercentage = 400;
            AttackSpeed = 25;
        }
    }
}
