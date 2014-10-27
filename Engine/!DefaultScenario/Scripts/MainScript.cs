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
        manager.Include("lightning_ball", 1, 20);
    }

    public override void OnPlayerJoined(Player p)
    {
        var javelinSpell = new Javelin();
        p.Hero.AddAbility(javelinSpell);
        //javelinSpell.$ganja = "das";
    }
}
