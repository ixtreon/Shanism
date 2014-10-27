using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Systems;

class MainScript : CustomScript
{
    public override void LoadModels(ModelManager manager)
    {
        manager.Include("hero", 1, 3);
        manager.Include("lightning_ball", 1, 20, 100);
    }

    public override void OnPlayerJoined(Player p)
    {
        var spellz = new[]
        {
            new Spark(),
        };
        foreach(var s in spellz)
            p.Hero.AddAbility(s);
    }
}
