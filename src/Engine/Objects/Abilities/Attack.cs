using System;
using System.Linq;
using Shanism.Engine.Systems;
using Shanism.Engine.Objects;
using Shanism.Engine;
using Shanism.Common;
using Shanism.Engine.Entities;
using Shanism.Engine.Events;

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
            var damageRoll = Owner.DamageRoll();
            var proj = new Projectile(Owner)
            {
                Position = Owner.Position,
                Model = "objects/arrowz",
                Scale = 2.5f,

                Direction = (float)Owner.Position.AngleTo(targetLoc),
                Speed = 20,
                MaxRange = (float)CastRange,

                DestroyOnCollision = DestroyOnCollision,
            };
            proj.OnUnitCollision += (p, u)
                => Owner.DamageUnit(u, DamageType.Physical, damageRoll);

            Map.Add(proj);
            return true;
        }

        bool meleeAttack(AbilityCastArgs e)
        {
            Unit target;
            if (e.TargetEntity is Unit)
                target = (Unit)e.TargetEntity;
            else
            {
                target = Map.GetUnitsInRange(Owner.Position, CastRange + Constants.Entities.MaxSize)
                    .Where(Owner.Owner.IsEnemyOf)
                    .OrderBy(u => u.Position.DistanceTo(e.TargetLocation))
                    .Where(u => u.Position.DistanceTo(e.TargetLocation) < (CastRange + (u.Scale + Owner.Scale) / 2))
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