using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Systems;
using IO.Common;
using Engine.Common;

class ReviveMonsters : CustomScript
{
    public override void GameStart()
    {
        var m1 = new Monster("Goshko", 2)
        {
            Location = new Vector(2, 2),
        };

        Map.AddUnit(m1);
    }
    public override async void OnUnitDeath(Unit unit)
    {
        if (unit.GetType() == typeof(Monster))
        {
            const double revive_range = 3;

            var nu = new Monster((Monster)unit);

            var pos = unit.Location;
            var dx = Rnd.NextDouble(-revive_range, revive_range);
            var dy = Rnd.NextDouble(-revive_range, revive_range);
            pos += new Vector(dx, dy);
            nu.Location = pos;

            await Task.Delay(3000);

            Map.AddUnit(nu);
        }
    }
}
