using Engine.Events;
using Engine.Objects.Game;
using Engine.Systems.Abilities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultScenario.Abilities
{
    class SpawnTree : Ability
    {
        public SpawnTree()
        {
            TargetType = AbilityTargetType.PointTarget;

            Name = "Plant a tree";
            Description = "Plants a tree on the target spot on the ground. ";

            Cooldown = 1000;
            ManaCost = 5;
        }

        protected override void OnCast(AbilityCastArgs e)
        {
            //This code will be executed when the spell is cast. 
            var dood = new Doodad(e.TargetLocation) { ModelName = "tree-1" };
            Map.Add(dood);
        }
    }
}
