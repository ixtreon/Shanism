using System;
using System.Linq;

using Engine.Systems;
using Engine.Objects;
using Engine;
using Engine.Common;
using IO.Common;
using Engine.Objects.Game;
using Engine.Events;

namespace Engine.Systems.Abilities
{
    /// <summary>
    /// A simple melee attack ability that mirrors its owner's attack range and cooldown. 
    /// </summary>
    public class Attack : Ability
    {

        public Attack()
            : base(AbilityTargetType.PointTarget)
        {
            this.Name = "Attack";
            this.Description = "Attacks in the given direction. ";
            ManaCost = 0;
        }

        public bool IsRanged
        {
            get { return Owner.RangedAttack; }
        }


        public override void OnCast(AbilityCastArgs e)
        {
            var units = Map.GetUnitsInRange(Owner.Position, Owner.AttackRange);

            var potentialTargets = units
                .Where(u => u != Owner)
                .OrderBy(u => u.Position.DistanceTo(e.TargetLocation));

            if (!potentialTargets.Any())
            {
                e.Success = false;
                return;
            }

            var target = potentialTargets.First();

            var dmgAmount = Owner.DamageRoll();

            Owner.DamageUnit(target, DamageType.Physical, dmgAmount);
            Console.WriteLine("BAAM!");
        }

        public override void OnUpdate(int msElapsed)
        {
            this.Cooldown = Owner.AttackCooldown;
            this.CastRange = Owner.AttackRange;
        }
    }
}