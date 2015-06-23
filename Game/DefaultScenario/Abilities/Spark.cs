using System;
using System.Linq;
using Engine.Systems;
using Engine.Objects;
using Engine;
using Engine.Common;
using IO.Common;
using System.Collections.Generic;
using Engine.Objects.Game;
using DefaultScenario.Buffs;
using Engine.Events;

/// <summary>
/// A point-target spell that launches a spark to damage and bounce from the units it hits. 
/// </summary>
public class Spark : Ability
{
    const string SparkModel = "lightning_ball";

    public int Bounces = 3;

    // Run whenever a new ability instance is created. 
    // Also specifies the type of the ability, in this case a point-targeted spark. 
    // Sets basic properties such as the name, description, and the mana cost. 
    public Spark()
        : base(AbilityTargetType.PointTarget)
    {
        Icon = "lightning-1";
        Name = "Spark";
        Description = "Conjures a fucking spark travelling in a straight line. ";
        ManaCost = 1;
        Cooldown = 1000;
    }

    // Run whenever the spell is cast. 
    // Note the second parameter is a Vector, in line with the type of the ability. 
    public override void OnCast(AbilityCastArgs e)
    {
        castSpark(Bounces, Owner, Owner, e.TargetLocation);
    }

    /// <summary>
    /// A helper method that casts a spark towards a target location, starting from the given unit. 
    /// The damage roll and owner of the new spark are determined by the caster variable. 
    /// </summary>
    /// <param name="bounces">The remaining bounces for the new spark. </param>
    /// <param name="caster"></param>
    /// <param name="start"></param>
    /// <param name="target"></param>
    private void castSpark(int bounces, Unit caster, Unit start, Vector target)
    {
        // Get the angle to launch the spark at. 
        var angle = start.Location.AngleTo(target);

        // Create the projectile and set its parameters. 
        var p = new Projectile(SparkModel, start.Location, ignoredUnits: new[] { start })
        {
            Size = 0.45,
            Direction = angle,
            Speed = 4,
            DestroyOnCollision = true,
        };

        // Set some custom properties on the projectile:
        // the damage it is to deal, and the spark bounces remaining. 
        p["damage"] = caster.DamageRoll();
        p["bounce"] = bounces;

        // Add it to the current map. 
        Map.Doodads.Add(p);

        // Register the callback to damage the unit. 
        p.OnUnitCollision += onProjectileCollision;
    }

    private void onProjectileCollision(Projectile p, Unit targetUnit)
    {
        var dmgAmount = (double)p["damage"];
        var bounces = (int)p["bounce"];

        Owner.DamageUnit(targetUnit, DamageType.Physical, dmgAmount);

        targetUnit.Buffs.Add(new Stunned(1000));
        

        if(bounces > 0)
        {
            var tar = 
                Map.GetUnitsInRange(targetUnit.Location, 5)
                .OrderBy(uu => uu.Location.DistanceTo(targetUnit.Location))
                .Where(uu => uu != targetUnit && !uu.IsDead)
                .FirstOrDefault();
            if(tar != null)
                castSpark(bounces - 1, Owner, targetUnit, tar.Location);
        }
    }

    public override void OnUpdate(int msElapsed)
    {

    }
}