using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Events;
using Engine.Systems.Buffs;
using Engine.Systems.Abilities;

namespace DefaultScenario.Abilities
{
    class Haste : Ability
    {

        public Haste()
        {
            TargetType = AbilityTargetType.NoTarget;

            Name = "Haste";
            Icon = "enchant-orange-3";
            Description = "Grants the caster increased movement and attack speed for a short duration. ";
            CastRange = 10;
            ManaCost = 1;
            Cooldown = 10000;
        }

        protected override void OnCast(AbilityCastArgs e)
        {

            var b = new Buff(BuffType.NonStacking, 5000)
            {
                Icon = "enchant-orange-3",
                Name = "Haste",
                RawDescription = "Increases the unit's movement speed by {MoveSpeedPercentage:0;0}% and its attack speed by {AttackSpeed:0;0}%. ",

                MoveSpeedPercentage = 400,
                AttackSpeed = 25,
            };

            e.TargetUnit.Buffs.Add(b);
        }
    }
}
