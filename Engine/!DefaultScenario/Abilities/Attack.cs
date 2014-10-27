using System;
using System.Linq;

using Engine.Systems;
using Engine.Objects;
using Engine;
using Engine.Common;
using IO.Common;

public class Attack : Ability
{

    public Attack()
        : base(AbilityType.PointTarget)
    {
        this.Name = "Attack";
        this.Description = "Attacks in the given direction. ";
    }


    public override void OnCast(CastEventArgs e, Vector pos)
    {
        const float dist = 1f;
        const double angle = Math.PI / 4;

        Console.WriteLine(pos.ToString());

        var units = Map.GetUnitsInRange(Hero.Location, dist);

        var t = units
            .Where(u => u.IsNonPlayable())
            .OrderBy(u => u.Location.DistanceTo(Hero.Location));



        if (!t.Any())
        {
            e.Success = false;
            return;
        }

        var target = t.First();
        var dmgAmount = Rnd.Next((int)Hero.MinDamage, (int)Hero.MaxDamage + 1);

        Hero.DamageUnit(t.First(), DamageType.Physical, dmgAmount);
    }

    public override void OnUpdate(int msElapsed)
    {
        this.Cooldown = Hero.AttackCooldown;
    }
}