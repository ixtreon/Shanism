using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Entities;
using Engine.Systems;
using IO.Common;
using Engine.Entities.Objects;
using Engine;

class SpawnMonsters : CustomScript
{
    int CampSpawnRadius = 40;
    int CampCount = 3;

    int UnitsPerCamp = 3;
    int UnitCampRadius = 5;

    public override void OnGameStart()
    {
        spawnMonsterCamps();
        spawnDoodadCircle();
        spawnNRandomMonsters(0);
        spawnNTrees(0);
    }

    void spawnNTrees(int n)
    {
        foreach (var i in Enumerable.Range(0, n))
        {
            var uPos = Rnd.PointInside(Terrain.Bounds);
            Map.Add(new Tree { Position = uPos });
        }
    }

    void spawnNRandomMonsters(int n)
    {
        foreach(var i in Enumerable.Range(0, n))
        {
            var uPos = Rnd.PointInside(Terrain.Bounds);
            Map.Add(new Monster(uPos, 2)
            {
                ModelName = "devilkin",
                Life = 123,
                BaseMoveSpeed = 0.1,
            });
        }
    }

    void spawnDoodadCircle()
    {
        var c = Terrain.Bounds.Center;

        const double DoodadDist = 2;
        const int DoodadCount = 50;
        foreach (var i in Enumerable.Range(0, DoodadCount))
        {   
            var dist = 2 * Math.PI * i / DoodadCount;
            var pos = c.PolarProjection(dist, DoodadDist);
            var m = new Tree { Position = pos };

            Map.Add(m);
        }
    }

    void spawnMonsterCamps()
    {
        //restrain the camp spawn area to the terrain bounds
        var campSpawnRect =
            new Rectangle(-CampSpawnRadius, -CampSpawnRadius, 2 * CampSpawnRadius, 2 * CampSpawnRadius)
            .IntersectWith(Terrain.Bounds);

        foreach (var iCamp in Enumerable.Range(0, CampCount))
        {
            //center it at a random spot
            var campCenter = Rnd.PointInside(campSpawnRect);

            //make some units
            foreach (var iUnit in Enumerable.Range(0, UnitsPerCamp))
            {
                var uPos = Rnd.PointInCircle(campCenter, UnitCampRadius);
                var m = new Monster(uPos, 2)
                {
                    ModelName = "devliklin",
                    Life = 123,
                    BaseMoveSpeed = 0.1,
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
