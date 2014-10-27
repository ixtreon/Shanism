using System;
using System.Linq;
using Engine.Systems;
using Engine.Objects;
using Engine;
using Engine.Common;
using IO.Common;
using System.Collections.Generic;

public class Spark : Ability
{
    public Spark()
        : base(AbilityType.PointTarget)
    {
        Icon = "lightning-1";
        Name = "Spark";
        Description = "Conjures a fucking spark travelling in a straight line. ";
        ManaCost = 1;
    }


    public override void OnCast(CastEventArgs e, Vector pos)
    {
        var angle = Hero.Location.AngleTo(pos);

        var p = new Projectile("lightning_ball")
        {
            Size = 0.9,
            Location = Hero.Location,
            Direction = angle,
            Speed = 7,
            DestroyOnCollision = true,
        };
        p["damage"] = Hero.DamageRoll();

        Map.AddDoodad(p);

        p.OnUnitCollision += onProjectileCollision;
    }

    private void onProjectileCollision(Projectile p, Unit u)
    {
        var dmgAmount = (double)p["damage"];

        Hero.DamageUnit(u, DamageType.Physical, dmgAmount);
    }

    public override void OnUpdate(int msElapsed)
    {
        this.Cooldown = Hero.AttackCooldown / 10;
    }
}