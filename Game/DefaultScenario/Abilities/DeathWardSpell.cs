using System;
using System.Linq;
using Engine.Objects;
using IO.Common;
using System.Collections.Generic;
using Engine._DefaultScenario.Units;
using Engine.Objects.Game;

public class DeathWardSpell : Ability
{
    public DeathWardSpell()
        : base(AbilityTargetType.PointTarget)
    {
        CastTime = 3000;
        Icon = "ice-sky-2";
        Name = "Death Ward";
        Description = "dasdasdasd";
        ManaCost = 1;
        Cooldown = 3;
        CastRange = 5;
    }

    public override void OnCast(Engine.Events.AbilityCastArgs e)
    {
        var ward = new DeathWard(e.TargetLocation);
        Map.Units.Add(ward);
    }

    private void onProjectileCollision(Projectile p, Unit u)
    {
        var dmgAmount = (double)p["damage"];

        Owner.DamageUnit(u, DamageType.Physical, dmgAmount);
    }

    public override void OnUpdate(int msElapsed)
    {
        this.Cooldown = Owner.AttackCooldown / 10;
    }
}