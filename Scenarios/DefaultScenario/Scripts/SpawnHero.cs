using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Systems;
using Engine.Objects.Game;
using IO.Common;

namespace DefaultScenario
{
    class SpawnHeroes : CustomScript
    {

        public override void OnHeroSpawned(Hero hero)
        {
            var spellz = new Ability[]
            {
                new Abilities.Haste(),
                new Teleport(),
                new Spark(),
                new Vacuum(),
                new DeathWardSpell(),
            };

            foreach (var s in spellz)
                hero.AddAbility(s);
        }

        public override void OnPlayerJoined(Player p)
        {
            if (!p.HasHero)
            {
                var h = new Hero(p, Terrain.Bounds.Center) { Name = p.Name ?? "?!" };
                Map.Add(h);

                p.SetMainHero(h);
            }

        }
    }
}