using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Systems;
using Engine.Entities.Objects;
using IO.Common;
using DefaultScenario.Abilities;
using Engine.Systems.Abilities;

namespace DefaultScenario
{
    class SpawnHeroes : CustomScript
    {

        public override void OnHeroSpawned(Hero hero)
        {
            var spellz = new Ability[]
            {
                new Hook(),
                new SpawnTree(),
                new LudHook(),
                new Flameshit(),
                new Haste(),
                new Teleport(),
                new Spark(),
                new Vacuum(),
                new DeathWardSpell(),
            };

            foreach (var s in spellz)
                hero.Abilities.Add(s);
        }

        public override void OnPlayerJoined(Player p)
        {
            if (!p.HasHero)
            {
                var h = new Hero(p)
                {
                    Position = Terrain.Bounds.Center,
                    Name = p.Name ?? "?!"
                };

                Map.Add(h);
                p.SetMainHero(h);
            }

        }
    }
}