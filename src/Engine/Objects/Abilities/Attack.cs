using Ix.Math;
using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Events;
using System.Linq;
using System.Numerics;

namespace Shanism.Engine.Objects.Abilities
{
    /// <summary>
    /// A simple attack ability that mirrors its owner's attack type, range and cooldown. 
    /// Ranged heroes shoot projectiles. 
    /// Melee heroes swing at the closest foe and can do it while walking. 
    /// </summary>
    [AbilityType(AbilityTypeFlags.Spammable)]
    public class Attack : Ability
    {

        /// <summary>
        /// Gets or sets a value indicating whether projectiles are destroyed whenever they hit the first unit.
        /// </summary>
        public bool DestroyOnCollision { get; set; } = true;

        /// <summary>
        /// Gets or sets the speed of the projectile in squares per second. 
        /// Makes sense only if this unit's attack is ranged (see <see cref="Unit.HasRangedAttack"/>).
        /// </summary>
        public float ProjectileSpeed { get; set; } = 20;

        public Attack()
        {
            TargetType = AbilityTargetType.PointOrUnitTarget;
            this.Name = "Attack";
            this.Description = "Attacks in the given direction. ";
            ManaCost = 0;
        }

        protected override void OnCast(AbilityCastArgs e)
        {
            if (Owner.HasRangedAttack)
                e.Success = rangedAttack(e);
            else
                e.Success = meleeAttack(e);
        }

        bool rangedAttack(AbilityCastArgs e)
        {
            var targetLoc = e.TargetLocation;
            var damageRoll = Owner.DamageRoll();    // roll the damage now - stats may change!
            var proj = new Projectile(Owner)
            {
                Position = Owner.Position,
                Model = "objects/arrowz",
                Scale = 2.5f,

                Direction = Owner.Position.AngleTo(targetLoc),
                Speed = ProjectileSpeed,
                MaxRange = CastRange,

                DestroyOnCollision = DestroyOnCollision,
            };
            proj.OnUnitCollision += (p, u)
                => Owner.DamageUnit(u, DamageType.Physical, damageRoll);

            Map.Add(proj);
            return true;
        }

        bool meleeAttack(AbilityCastArgs e)
        {
            if (!(e.TargetEntity is Unit target))
            {
                target = Map.GetUnitsInRange(new Ellipse(Owner.Position, CastRange))
                    .Where(Owner.Owner.IsEnemyOf)
                    .OrderBy(u => u.Position.DistanceToSquared(e.TargetLocation))
                    .FirstOrDefault();

                if (target == null)
                    return false;
            }

            var dmgAmount = Owner.DamageRoll();
            Owner.DamageUnit(target, DamageType.Physical, dmgAmount);
            return true;
        }

        protected override void OnUpdate(int msElapsed)
        {
            CastRange = Owner.Scale / 2 + Owner.AttackRange;
            Cooldown = Owner.AttackCooldown;
            CanCastWalk = !Owner.HasRangedAttack;
        }
    }
}