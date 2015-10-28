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
    int CampSpawnRadius = 40;
    int CampCount = 3;

    int UnitsPerCamp = 3;
    int UnitCampRadius = 5;

    public override void GameStart()
    {
        //restrain the camp spawn area to the terrain bounds
        var campSpawnRect = 
            new Rectangle(-CampSpawnRadius, -CampSpawnRadius, 2 * CampSpawnRadius, 2 * CampSpawnRadius)
            .IntersectWith(Terrain.Bounds);

        foreach(var iCamp in Enumerable.Range(0, CampCount))
        {
            //center it at a random spot
            var campCenter = Rnd.PointInside(campSpawnRect);

            //make some units
            foreach(var iUnit in Enumerable.Range(0, UnitsPerCamp))
            {
                var uPos = Rnd.PointInCircle(campCenter, UnitCampRadius);
                var m = new Monster("mobche", uPos, 2)
                {
                    Life = 123,
                };
                Map.Add(m);
            }
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


            //wait 30 sec
            await Task.Delay(30000);

            Map.Add(nu);
        }
    }
}
