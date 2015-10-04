using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using Engine.Systems.Buffs;

namespace DefaultScenario.Buffs
{
    class Stunned : Buff
    {
        
        public Stunned(int duration) : base(BuffType.StackingNormal, duration)
        {
            this.FullDuration = duration;
            this.Name = "Stunned";
        }

        public override void OnApplied(BuffInstance buff)
        {
            Console.WriteLine("Stunned");
            buff.Target.ApplyState(UnitState.Stunned);
            base.OnApplied(buff);
        }

        public override void OnUpdate(BuffInstance buff)
        {
            base.OnUpdate(buff);
        }

        public override void OnExpired(BuffInstance buff)
        {
            Console.WriteLine("Unstunned");
            buff.Target.RemoveState(UnitState.Stunned);
        }
    }
}
