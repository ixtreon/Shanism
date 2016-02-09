using Engine.Entities;
using Engine.Entities.Objects;
using Engine.Systems;
using Engine.Systems.Abilities;
using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine._DefaultScenario.Units
{
    public class DeathWard : Unit
    {
        Ability wardSpell = new Spark
        {
            Bounces = 3,
            ManaCost = 0,
        };


        public DeathWard(Player owner, Vector location, int duration = 5000)
            : base(owner, 1)
        {
            Position = location;

            ModelName = "pruchka";
            Name = "Death Ward";

            //Behaviour = new AggroBehaviour();
            Abilities.Add(wardSpell);

            wardSpell.ManaCost = 0;

            castSpark();
            timedKill(duration);
        }

        /// <summary>
        /// Causes this unit to continuously find targets in its range and cast 
        /// the <see cref="Spark"/> ability towards them. 
        /// </summary>
        private async void castSpark()
        {
            while (!this.IsDead)
            {
                //get nearby enemy units
                var target = Map.GetUnitsInRange(Position, 5, aliveOnly: true)
                    .Where(u => u.Owner.IsEnemyOf(Owner))
                    .Where(u => u != this)
                    .OrderBy(u => u.Position.DistanceTo(this.Position))
                    .FirstOrDefault();

                if (target != null)
                {
                    this.CastAbility(wardSpell, target.Position);
                }

                await Task.Delay(wardSpell.CurrentCooldown + 1);
            }
        }

        /// <summary>
        /// Kills this unit after a specified amount of time. 
        /// </summary>
        /// <param name="msDelay">The time to wait, in miliseconds. </param>
        private async void timedKill(int msDelay)
        {
            await Task.Delay(msDelay);
            this.Kill();
        }
    }
}
