using Engine.Entities.Objects;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Events;
using Engine.Entities;
using Engine.Systems.Abilities;

namespace DefaultScenario.Abilities
{
    public class Flameshit : Ability
    {

        public Flameshit()
            : base(AbilityTargetType.PointTarget)
        {
            Name = "Flame Shit Thingy";
            Description = "Fire a flame thing that explodes. ";
            Icon = "spear";

            Cooldown = 3000;
            ManaCost = 5;
        }

        protected override void OnCast(AbilityCastArgs e)
        {
            var targetPos = e.TargetLocation;
            var casterPos = e.CastingUnit.Position;

            var angle = casterPos.AngleTo(targetPos);

            var proj = new Projectile(casterPos, "flame", e.CastingUnit, "Ogin bace");
            proj.Speed = 5;
            proj.Direction = angle;
            proj.MaxRange = 15;
            proj.DestroyOnCollision = true;

            proj.OnUnitCollision += Proj_OnUnitCollision;


            Map.Add(proj);
        }

        Random randomGuy = new Random();

        async void Proj_OnUnitCollision(Projectile p, Unit target)
        {
            if (!Owner.Owner.IsEnemyOf(target.Owner))
                return;

            var dmgAmount = randomGuy.Next(50, 75);
            Owner.DamageUnit(target, DamageType.Magical, dmgAmount);

            target.ApplyState(UnitFlags.Stunned);

            await Task.Delay(5000);

            target.RemoveState(UnitFlags.Stunned);

        }
    }
}
