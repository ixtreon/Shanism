using Engine.Entities.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Events;
using Engine.Entities;
using IO.Common;
using Engine.Systems.Abilities;

namespace DefaultScenario.Abilities
{
    class Hook : Ability
    {
        double hookspeed = 10;  //in sq/sec
        double delay = 20; //in msec
        double hookrange = 0.5;

        double HookDamage = 50;

        Unit zahapaniaUnit;

        public Hook()
        {
            TargetType = AbilityTargetType.PointTarget;

            Name = "Hook";
            Description = "Hooks a targer";
            Cooldown = 7000;
            ManaCost = 10;
            CastRange = 5;
        }


        protected override async void OnCast(AbilityCastArgs e)
        {
            var h = this.Owner; //tva e castera na umenieto
            var Hook = new Doodad { Position = h.Position, ModelName = "spark" };
            Map.Add(Hook);

            var nSteps = (CastRange / hookspeed * 1000) / delay;
            var angle = Owner.Position.AngleTo(e.TargetLocation);
            var StepSize = (hookspeed * delay) / 1000;

            int i;
            for (i = 0; i < nSteps; i++)
            {
                Hook.Position = Hook.Position.PolarProjection(angle, StepSize);

                var u = Map.GetUnitsInRange(Hook.Position, hookrange)
                    .Where(uu => uu != Owner)
                    .FirstOrDefault();

                //zahapi, ako ima
                if (u != null)
                {
                    zahapaniaUnit = u;
                    zahapaniaUnit.ApplyState(UnitState.Stunned);

                    //dmg ako e vrag
                    if (u.Owner.IsEnemyOf(Owner.Owner))
                        Owner.DamageUnit(u, IO.Common.DamageType.Physical, HookDamage);

                    break;
                }

                await Task.Delay((int)delay);
            }

            while (Hook.Position.DistanceTo(Owner.Position) > StepSize)
            {
                var ang = Hook.Position.AngleTo(Owner.Position);
                var d = Vector.Zero.PolarProjection(ang, StepSize);

                Hook.Position += d;
                if (zahapaniaUnit != null)
                    zahapaniaUnit.Position += d;

                await Task.Delay((int)delay);
            }

            if (zahapaniaUnit != null)
                zahapaniaUnit.RemoveState(UnitState.Stunned);

            Hook.Destroy();
        }
    }
}
