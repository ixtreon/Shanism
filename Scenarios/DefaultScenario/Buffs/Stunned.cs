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

        /// <summary>
        /// The <see cref="OnApplied(BuffInstance)"/> method is called every time this buff is applied to a unit. 
        /// </summary>
        /// <param name="buff">The buff instance containing the buff, the target unit and possibly the duration left. </param>
        public override void OnApplied(BuffInstance buff)
        {
            Console.WriteLine("Stunned");
            buff.Target.ApplyState(UnitState.Stunned);
            base.OnApplied(buff);
        }

        /// <summary>
        /// This method is called every frame an instance of this buff remains on a unit. 
        /// </summary>
        /// <param name="buff">The buff instance containing the buff, the target unit and possibly the duration left. </param>
        public override void OnUpdate(BuffInstance buff)
        {
            base.OnUpdate(buff);
        }

        /// <summary>
        /// This method is called every time a buff instance expires from a target unit. 
        /// </summary>
        /// <param name="buff">The buff instance that references the buff, the target unit and possibly the duration left. </param>
        public override void OnExpired(BuffInstance buff)
        {
            Console.WriteLine("Unstunned");
            buff.Target.RemoveState(UnitState.Stunned);
        }
    }
}
