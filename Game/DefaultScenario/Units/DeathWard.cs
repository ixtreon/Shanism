using Engine.Objects;
using Engine.Objects.Game;
using Engine.Systems;
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
        Ability spell = new Spark()
        {
            Bounces = 3,
            ManaCost = 0,
        };

        public DeathWard(Vector location, int duration = 5000)
            : base("pruchka", location, 1)
        {
            this.Model = "pruchka";
            AddAbility(spell);

            spell.ManaCost = 0;

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
                //get nearby, enemy units
                var us = Map.GetUnitsInRange(Location, 5)
                    .OrderBy(u => u.Location.DistanceTo(this.Location))
                    .Where(u => u != this && !u.IsDead);

                if (us.Any())
                {
                    this.CastAbility(spell, us.First().Location);
                }

                await Task.Delay(spell.CurrentCooldown + 1);
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
