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
            const float range = 1f;
            const double angle = Math.PI / 4;   //todo: check angle

            var units = Map.GetUnitsInRange(Owner.Position, range);

            var t = units
                .Where(u => u != Owner)
                .OrderBy(u => u.Position.DistanceTo(Owner.Position));

            if (!t.Any())
            {
                e.Success = false;
                return;
            }

            var target = t.First();

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