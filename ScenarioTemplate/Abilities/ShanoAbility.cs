using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO;
using IO.Common;
using Engine.Events;
using Engine.Systems.Abilities;

namespace ScenarioTemplate.Abilities
{
    class ShanoAbility : Ability
    {
        public ShanoAbility()
        {
            TargetType = AbilityTargetType.PointTarget;

            Name = "Dummy Ability";
            Description = "Dummy Description";

            Cooldown = 1000;
            ManaCost = 5;
        }

        protected override void OnCast(AbilityCastArgs e)
        {
            //This code will be executed when the spell is cast. 
        }
    }
}
