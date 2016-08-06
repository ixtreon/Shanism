using System;
using System.Linq;
using Shanism.Engine.Systems;
using Shanism.Engine.Objects;
using Shanism.Engine;
using Shanism.Engine.Common;
using Shanism.Common.Game;
using Shanism.Engine.Entities;
using Shanism.Engine.Events;

namespace Shanism.Engine.Objects.Abilities
{
    /// <summary>
    /// A simple melee attack ability that mirrors its owner's attack type, range and cooldown. 
    /// </summary>
    [AbilityType(AbilityTypeFlags.Spammable)]
    public class Attack : Ability
    {

        public Attack()
        {
            TargetType = AbilityTargetType.PointOrUnitTarget;
            this.Name = "Attack";
            this.Description = "Attacks in the given direction. ";
            ManaCost = 0;
        }

        protected override void OnCast(AbilityCastArgs e)
        {
            e.Success = (Owner.HasRangedAttack ? rangedAttack(e) : meleeAttack(e));
        }

        bool rangedAttack(AbilityCastArgs e)
        {
            var targetLoc = e.TargetLocation;
            var damageRoll = Owner.DamageRoll();

            var proj = new Projectile(Owner)
            {
                Position = Owner.Position,
                Model = "objects/arrowz",
                Scale = 2.5,

                Direction = Owner.Position.AngleTo(targetLoc),
                Speed = 20,
                MaxRange = CastRange,

                DestroyOnCollision = true,
            };
            proj.OnUnitCollision += (p, u) 
                => Owner.DamageUnit(u, DamageType.Physical, damageRoll);
            Map.Add(proj);
            Console.WriteLine(Map.Entities.OfType<Projectile>().Count());
            return true;
        }

        bool meleeAttack(AbilityCastArgs e)
        {
            Unit target;
            if (e.TargetEntity is Unit)
                target = (Unit)e.TargetEntity;
            else
            {
                target = Map.GetUnitsInRange(Owner.Position, CastRange)
                    .Where(u => u.Owner.IsEnemyOf(Owner))
                    .OrderBy(u => u.Position.DistanceTo(e.TargetLocation))
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
            CastRange = Owner.AttackRange;
            Cooldown = Owner.AttackCooldown;
        }
    }
}