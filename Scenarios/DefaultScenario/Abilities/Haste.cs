using Engine.Objects.Game;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Events;
using Engine.Systems.Buffs;

namespace DefaultScenario.Abilities
{
    class Haste : Ability
    {
        public Haste()
            : base(AbilityTargetType.PointTarget)
        {
            Name = "Haste";
            Icon = "enchant-orange-3";
            Description = "Cast yourself some haste!";
            CastRange = 10;
            ManaCost = 1;
            Cooldown = 10000;
        }

        public override void OnCast(AbilityCastArgs e)
        {
            var b = new Buff(BuffType.NonStacking, 5000)
            {
                Icon = "enchant-orange-3",
                Name = "Haste",
                RawDescription = "Increases the unit's movement speed by {MoveSpeedPercentage:0;0}% and its attack speed by {AttackSpeed:0;0}%. ",

                MoveSpeedPercentage = 400,
                AttackSpeed = 25,
            };

            Owner.Buffs.Add(b);
        }
    }
}
