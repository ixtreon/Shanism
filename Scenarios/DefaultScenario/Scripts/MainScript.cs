using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Systems;
using Engine.Objects.Game;
using IO.Common;

class MainScript : CustomScript
{
    public override void LoadModels(ModelManager manager)
    {
        manager.Include("hero", 3, 1);
        manager.Include("mobche", 4, 1);
        manager.Include("lightning_ball", 20, 1, 100);
        manager.Include("pruchka");
        manager.Include("tree");
    }

    public override void OnHeroSpawned(Hero hero)
    {
        var spellz = new Ability[]
        {
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
            var h = new Hero(p, new Vector(0, 0));
            Map.Add(h);

            p.SetMainHero(h);
        }

    }
}
