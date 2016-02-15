﻿using System;
using System.Linq;
using Engine.Systems;
using Engine.Entities;
using Engine;
using Engine.Common;
using IO.Common;
using Engine.Entities.Objects;
using Engine.Events;

namespace Engine.Systems.Abilities
{
    /// <summary>
    /// A simple melee attack ability that mirrors its owner's attack range and cooldown. 
    /// </summary>
    [AbilityType(AbilityType.Spammable)]
    public class Attack : Ability
    {

        public Attack()
            : base(AbilityTargetType.PointTarget)
        {
            this.Name = "Attack";
            this.Description = "Attacks in the given direction. ";
            ManaCost = 0;
        }

        protected override void OnCast(AbilityCastArgs e)
        {

            var units = Map.GetUnitsInRange(Owner.Position, CastRange);

            var potentialTargets = units
                .Where(u => u.Owner.IsEnemyOf(Owner))
                .OrderBy(u => u.Position.DistanceTo(e.TargetLocation))
                .ToArray();

            if (!potentialTargets.Any())
            {
                e.Success = false;
                return;
            }

            var target = potentialTargets.First();

            var dmgAmount = Owner.DamageRoll();

            Owner.DamageUnit(target, DamageType.Physical, dmgAmount);
        }

        protected override void OnUpdate(int msElapsed)
        {
            Cooldown = Owner.AttackCooldown;
            CastRange = Owner.AttackRange;
        }
    }
}