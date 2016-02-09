using Engine.Common;
using Engine.Events;
using IO.Common;
using IO.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    partial class Unit
    {
        /// <summary>
        /// The event fired when the unit gets killed by another unit. 
        /// </summary>
        public event Action<UnitDyingArgs> Dying;

        /// <summary>
        /// The event executed right after a unit receives damage, but before it is eventually killed. 
        /// </summary>
        public event Action<UnitDamagedArgs> DamageReceived;

        /// <summary>
        /// The event executed right before a unit deals damage to a target. 
        /// </summary>
        public event Action<UnitDamagingArgs> DamageDealt;


        /// <summary>
        /// Gets the time it takes for an unit to attack as determined by <see cref="AttacksPerSecond"/>. 
        /// </summary>
        /// <returns>The time for an attack in milliseconds. </returns>
        public int AttackCooldown
        {
            get { return (int)(1000 / AttacksPerSecond); }
            set { AttacksPerSecond = 1000 / value; }
        }



        /// <summary>
        /// Gets the multiplier for damage of the selected type inflicted on this unit. 
        /// </summary>
        /// 
        /// <example>
        /// For example a unit with 0 armor should have 0% physical reduction or a multiplier of 1.0. 
        /// A unit with positive armor will then have a multiplier between 0 and 1,
        /// and one with negative armor will have a multiplier greater than 1. 
        /// </example>
        double getArmorMultiplier(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physical:
                    return 1 / (Constants.Units.DamageReductionPerDefense * Defense + 1);
                case DamageType.Light:
                    return 1;
                case DamageType.Shadow:
                    return 1;
                case DamageType.Dark:
                    return 1;
                default:
                    throw new NotImplementedException();
            }
        }

        double getCritMultiplier(DamageType dmgType, double roll)
        {
            return 1;
        }

        /// <summary>
        /// Gets the experience rewarded when the unit is killed. 
        /// </summary>
        /// <returns></returns>
        public virtual int GetExperienceReward()
        {
            return Constants.Units.Experience.Base + this.Level * Constants.Units.Experience.LevelFactor;
        }

        /// <summary>
        /// Causes this unit to damage the specified unit. 
        /// </summary>
        /// <param name="target">The unit to deal damage to. </param>
        /// <param name="dmgType">The type of damage to deal. </param>
        /// <param name="amount">The amount of damage to deal. </param>
        /// <returns></returns>
        public bool DamageUnit(Unit target, DamageType dmgType, double amount, DamageFlags flags = DamageFlags.None)
        {
            if (target.IsDead || target.Invulnerable)
                return false;

            // Dodge
            var wasDodged = !flags.HasFlag(DamageFlags.NoDodge) && (Rnd.Next(0, 100) < target.DodgeChance);
            var wasCrit = !flags.HasFlag(DamageFlags.NoCrit) && (Rnd.Next(0, 100) < target.CritChance);

            //TODO: if it is magical damage, increase it based on magic dmgg

            //raise the pre-damage event
            var dmgArgs = new UnitDamagingArgs(this, target, dmgType, flags, amount);
            DamageDealt?.Invoke(dmgArgs);

            // calculate and deal the final damage using the EventArgs from the event.
            var finalDmg = target.GetFinalDamage(dmgArgs.BaseDamage, dmgArgs.DamageType);
            target.Life -= finalDmg;

            //raise the post-damage event
            var receiveArgs = new UnitDamagedArgs(this, target, dmgType, flags, amount, finalDmg);
            target.DamageReceived?.Invoke(receiveArgs);

            //send a message yo
            target.SendMessageToVisibles(new DamageEventMessage(target, dmgType, finalDmg, true));

            //check for death
            if (target.LifePercentage <= 0)
            {
                target.Kill(this);

                //fire scenario event
                target.Scenario.RunScripts(s => s.OnUnitDeath(target));
            }

            return true;
        }

        /// <summary>
        /// Calculates the final damage the unit will receive, 
        /// if it was dealt damage of the specified amount and type. 
        /// </summary>
        /// <param name="amount">The amount of damage dealt by the attacker. </param>
        /// <param name="dmgType">The type of damage dealt. </param>
        /// <returns></returns>
        public double GetFinalDamage(double amount, DamageType dmgType)
        {
            return amount * getArmorMultiplier(dmgType);
        }

        /// <summary>
        /// Gets a random value between <see cref="MinDamage"/> and <see cref="MaxDamage"/>
        /// </summary>
        /// <returns></returns>
        public double DamageRoll()
        {
            return Rnd.NextDouble(MinDamage, MaxDamage);
        }
    }
}
