using Shanism.Engine.Events;
using Shanism.Common;
using Shanism.Common.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    partial class Unit
    {

        /// <summary>
        /// Gets whether the unit is dead. 
        /// </summary>
        public bool IsDead { get; private set; }


        /// <summary>
        /// The event fired when the unit gets killed by another unit. 
        /// </summary>
        public event Action<UnitDyingArgs> Death;

        /// <summary>
        /// The event executed right after a unit receives damage, but before it is eventually killed. 
        /// </summary>
        public event Action<UnitDamagedArgs> DamageReceived;

        /// <summary>
        /// The event executed right before the unit is dealt damage by someone else. 
        /// </summary>
        public event Action<UnitDamagingArgs> ReceivingDamage;



        /// <summary>
        /// The event executed right after the unit deals damage to a target. 
        /// </summary>
        public event Action<UnitDamagedArgs> DealtDamage;

        /// <summary>
        /// The event executed right before the unit deals damage to a target. 
        /// </summary>
        public event Action<UnitDamagingArgs> DealingDamage;

        /// <summary>
        /// Occurs when the unit is finally removed from the map, i.e. destroyed.
        /// </summary>
        public event Action<UnitArgs> Destroyed;


        /// <summary>
        /// Gets the time it takes for an unit to attack as determined by <see cref="AttacksPerSecond"/>. 
        /// </summary>
        /// <returns>The time for one attack in milliseconds. </returns>
        public int AttackCooldown => (AttacksPerSecond > 0) ? (int)(1000 / AttacksPerSecond) : int.MaxValue;

        /// <summary>
        /// Gets whether this unit is currently stunned, 
        /// i.e. its <see cref="StateFlags"/> property contains the <see cref="StateFlags.Stunned"/> value.
        /// </summary>
        public bool IsStunned => (StateFlags & StateFlags.Stunned) != 0;

        /// <summary>
        /// Gets the multiplier for damage of the selected type inflicted on this unit. 
        /// </summary>
        /// 
        /// <example>
        /// For example a unit with 0 armor should have 0% physical reduction or a multiplier of 1.0. 
        /// A unit with positive armor will then have a multiplier between 0 and 1,
        /// and one with negative armor will have a multiplier greater than 1. 
        /// </example>
        float getArmorMultiplier(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physical:
                    return 1 / (Constants.Units.DamageReductionPerDefense * Defense + 1);
                case DamageType.Magical:
                    return 1;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the experience rewarded when the unit is killed. 
        /// </summary>
        /// <returns></returns>
        public virtual int GetExperienceReward()
        {
            return Constants.Heroes.Experience.Base + Level * Constants.Heroes.Experience.LevelFactor;
        }


        /// <summary>
        /// Instantly kills the unit. 
        /// </summary>
        /// <param name="killer">The killer of the unit or null if it is a suicide.</param>
        public void Kill(Unit killer = null)
        {
            if (IsDead)
                return;

            // update unit state
            Life = 0;
            IsDead = true;
            Buffs.Clear();


            // give out rewards
            if (killer != null)
            {
                // gold reward!

                if (killer is Hero)
                    ((Hero)killer).Experience += GetExperienceReward();
            }

            // raise the event
            var args = new UnitDyingArgs(this, killer ?? this);
            Death?.Invoke(args);

            //run the scripts
            Scripts.Run(s => s.OnUnitDeath(args));
        }

        /// <summary>
        /// Causes this unit to damage the specified unit.
        /// </summary>
        /// <param name="target">The unit to deal damage to.</param>
        /// <param name="dmgType">The type of damage to deal.</param>
        /// <param name="amount">The amount of damage to deal.</param>
        /// <param name="displayText">Whether to show damage text on the player screen.</param>
        /// <param name="flags">A DamageFlags instance specifying additional rules when dealing damage.</param>
        /// <returns></returns>
        public bool DamageUnit(Unit target, DamageType dmgType, float amount, bool displayText = true, DamageFlags flags = DamageFlags.None)
        {
            //Check target alive
            if (target.IsDead)
                return false;

            // Check target can take damage
            if (dmgType == DamageType.Physical && target.StateFlags.HasFlag(StateFlags.PhysicalImmune))
                return false;

            if (dmgType == DamageType.Magical && target.StateFlags.HasFlag(StateFlags.MagicImmune))
                return false;

            // Check target dodging
            var isDodge = !flags.HasFlag(DamageFlags.NoDodge) && (Rnd.Next(0, 100) < target.DodgeChance);
            if(isDodge)
                return false;

            // TODO: Damage amplifiers (crit, +magic)
            // var isCrit = !flags.HasFlag(DamageFlags.NoCrit) && (Rnd.Next(0, 100) < target.CritChance);


            // raise the pre-damage event
            var dmgArgs = new UnitDamagingArgs(this, target, dmgType, flags, amount);
            target.ReceivingDamage?.Invoke(dmgArgs);
            DealingDamage?.Invoke(dmgArgs);

            // Damage damping (armor, +magic def)
            var finalDmg = target.GetFinalDamage(dmgArgs.BaseDamage, dmgArgs.DamageType);

            //deal damage
            target.Life -= finalDmg;

            // raise the post-damage event
            var receiveArgs = new UnitDamagedArgs(this, target, dmgType, flags, amount, finalDmg);
            target.DamageReceived?.Invoke(receiveArgs);
            DealtDamage?.Invoke(receiveArgs);

            //send a message yo
            if (displayText)
            {
                var eventMessage = new DamageEventMessage(target, dmgType, finalDmg, true);
                target.SendMessageToVisibles(eventMessage);
            }

            //check for death
            if (target.LifePercentage <= 0)
                target.Kill(this);

            return true;
        }

        /// <summary>
        /// Calculates the final damage this unit will receive, 
        /// if it was dealt damage of the specified amount and type. 
        /// </summary>
        /// <param name="amount">The amount of damage dealt by the attacker. </param>
        /// <param name="dmgType">The type of damage dealt. </param>
        /// <returns></returns>
        public float GetFinalDamage(float amount, DamageType dmgType)
        {
            return amount * getArmorMultiplier(dmgType);
        }

        /// <summary>
        /// Gets a random value between <see cref="MinDamage"/> and <see cref="MaxDamage"/>
        /// </summary>
        /// <returns></returns>
        public float DamageRoll()
        {
            return Rnd.NextFloat(MinDamage, MaxDamage);
        }
    }
}
