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
using Engine.Systems.Abilities;

/// <summary>
/// A point-target spell that launches a spark to damage and bounce from the units it hits. 
/// </summary>
public class Spark : Ability
{
    const string SparkModel = "spark";

    public int Bounces { get; set; } = 3;

    // Run whenever a new ability instance is created. 
    // Also specifies the type of the ability, in this case a point-targeted spark. 
    // Sets basic properties such as the name, description, and the mana cost. 
    public Spark()
        : base(AbilityTargetType.PointTarget)
    {
        Icon = "lightning-1";
        Name = "Spark";
        Description = "Conjures a spark travelling in a straight line. ";
        ManaCost = 1;
        Cooldown = 100;
    }

    // Run whenever the spell is cast. 
    // Note the second parameter is a Vector, in line with the type of the ability. 
    protected override void OnCast(AbilityCastArgs e)
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
        var angle = start.Position.AngleTo(target);

        // Create the projectile and set its parameters. 
        var p = new Projectile(start.Position, ignoredUnits: new[] { start })
        {
            ModelName = SparkModel,
            Scale = 0.45,
            Speed = 4,
            DestroyOnCollision = false,
            MaxRange = 8,

            Direction = angle,
            Data = new  //add some custom data to the projectile
            {
                Damage = caster.DamageRoll(),
                Bounces = bounces,
            },
        };

        // Add it to the current map. 
        Map.Add(p);

        // Register the callback to damage the unit. 
        p.OnUnitCollision += onProjectileCollision;
    }

    void onProjectileCollision(Projectile p, Unit targetUnit)
    {
        Owner.DamageUnit(targetUnit, DamageType.Physical, p.Data.Damage);

        targetUnit.Buffs.Add(new Stunned(1000));

        if (p.Data.Bounces > 0)
        {
            var tar = Map.GetUnitsInRange(targetUnit.Position, 5, aliveOnly: true)
                .Where(u => u.Owner.IsEnemyOf(Owner))
                .OrderBy(u => u.Position.DistanceTo(targetUnit.Position))
                .FirstOrDefault();

            if (tar != null)
            {
                // Get the angle to redirect the spark at
                var angle = p.Position.AngleTo(tar.Position);
                p.Direction = angle;

                // re-set the spark's data
                p.Data = new
                {
                    Damage = p.Data.Damage,
                    Bounces = p.Data.Bounces - 1,
                };

                return;
            }
        }
        //no bounces or no targets
        p.Destroy();
    }
}