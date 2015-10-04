using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Systems;
using IO.Common;
using Engine.Common;
using Engine.Objects.Game;

class SpawnMonsters : CustomScript
{
    private int N_UNITS = 1;
    private int MAX_DIST = 3;
    static readonly Vector center = new Vector(0);

    public override void GameStart()
    {

        for (int i = 0; i < N_UNITS; i++)
        {
            var dist = Rnd.NextDouble(0, MAX_DIST);
            var ang = Rnd.NextDouble() * Math.PI * 2;
            var pos = center.PolarProjection(ang, dist);

            var m = new Monster("mobche", pos, 10)
            {
                Life = 123,
            };
            Map.Add(m);

        }

    }

    public override async void OnUnitDeath(Unit unit)
    {
        return;

        if (unit.GetType() == typeof(Monster))
        {
            const double revive_range = 0.01;

            var nu = new Monster((Monster)unit);

            var pos = unit.Position;
            var dx = Rnd.NextDouble(-revive_range, revive_range);
            var dy = Rnd.NextDouble(-revive_range, revive_range);
            pos += new Vector(dx, dy);
            nu.Position = pos;

            await Task.Delay(3000);

            Map.Add(nu);
        }
    }
}
