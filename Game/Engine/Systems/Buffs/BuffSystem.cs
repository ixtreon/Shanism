using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Buffs;
using Shanism.Engine.Objects.Entities;
using Shanism.Engine.Systems;
using Shanism.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems.Buffs
{
    /// <summary>
    /// Keeps hold of all buffs currently applied on a target unit
    /// and provides methods for applying and dispelling buffs. 
    /// </summary>
    public class BuffSystem : UnitSystem, IUnitBuffs
    {
        ConcurrentSet<BuffInstance> buffs { get; } = new ConcurrentSet<BuffInstance>();

        Unit Target { get; }

        public BuffSystem(Unit target)
        {
            Target = target;
        }

        internal override void Update(int msElapsed)
        {
            var toRemove = new List<BuffInstance>();

            foreach (var b in buffs)
            {
                b.Update(msElapsed);
                if (b.ShouldDestroy)
                    toRemove.Add(b);
            }

            foreach (var b in toRemove)
                buffs.TryRemove(b);

            updateStats(Target);

            if (Target is Hero)
                updateHeroStats((Hero)Target);
        }

        /// <summary>
        /// Updates the stats of the target unit based on the current buffs. 
        /// </summary>
        static void updateStats(Unit target)
        {

            var maxLife = target.BaseLife;
            var maxMana = target.BaseMana;

            var minDamage = target.BaseMinDamage;
            var maxDamage = target.BaseMaxDamage;
            var defense = target.BaseDefense;
            var magicDamage = target.BaseMagicDamage;

            var moveSpeed = target.BaseMoveSpeed;
            var bonusMoveSpeed = 1.0;
            var attacksPerSecond = target.BaseAttacksPerSecond;
            var bonusAtkSpeed = 1.0;

            var negativeDodgeChance = (100 - target.BaseDodgeChance);
            var negativeCritChance = (100 - target.BaseCritChance);

            var lifeRegen = Constants.Units.BaseLifeRegen;
            var manaRegen = Constants.Units.BaseManaRegen;

            var states = target.BaseStates;

            foreach (var b in target.Buffs)
            {
                maxLife += b.Life;
                maxMana += b.Mana;

                minDamage += b.MinDamage;
                maxDamage += b.MaxDamage;
                defense += b.Defense;
                //magicDamage += b.MagicDamage;     //TODO

                moveSpeed += b.MoveSpeed;
                bonusMoveSpeed += (b.MoveSpeedPercentage / 100.0);
                bonusAtkSpeed += (b.AttackSpeedPercentage / 100.0);

                negativeDodgeChance = negativeDodgeChance * (100 - b.Dodge) / 100;
                //negativeCritChance = negativeCritChance * (100 - b.Crit) / 100;       //TODO

                lifeRegen += b.LifeRegen;
                manaRegen += b.ManaRegen;

                states |= b.UnitStates;
            }

            target.MaxLife = maxLife;
            target.MaxMana = maxMana;

            target.MinDamage = minDamage;
            target.MaxDamage = maxDamage;
            target.Defense = defense;
            target.MagicDamage = magicDamage;

            target.MoveSpeed = Math.Max(0, moveSpeed * bonusMoveSpeed);
            target.AttacksPerSecond = Math.Max(0, attacksPerSecond * bonusAtkSpeed);

            target.DodgeChance = 100 - negativeDodgeChance;
            target.CritChance = 100 - negativeCritChance;

            target.LifeRegen = lifeRegen;
            target.ManaRegen = manaRegen;

            target.States = states;
        }

        static void updateHeroStats(Hero target)
        {

            target.Strength = target.BaseStrength;
            target.Vitality = target.BaseVitality;
            target.Intellect = target.BaseIntellect;
            target.Agility = target.BaseAgility;

            foreach(var b in target.Buffs)
            {
                target.Strength += b.Strength;
                target.Vitality += b.Vitality;
                target.Intellect += b.Intellect;
                target.Agility += b.Agility;
            }

            target.MinDamage += target.Strength * Constants.Units.Attributes.DamagePerStrength;
            target.MaxDamage += target.Strength * Constants.Units.Attributes.DamagePerStrength;
            target.Defense += target.Strength * Constants.Units.Attributes.DefensePerStrength;

            target.Defense += target.Agility * Constants.Units.Attributes.AtkSpeedPerAgility;
            target.Defense += target.Agility * Constants.Units.Attributes.DodgePerAgility;

            target.MaxLife += target.Vitality * Constants.Units.Attributes.LifePerVitality;
            target.MaxMana += target.Vitality * Constants.Units.Attributes.ManaPerVitality;

            target.LifeRegen += target.Intellect * Constants.Units.Attributes.LifeRegPerInt;
            target.ManaRegen += target.Intellect * Constants.Units.Attributes.ManaRegPerInt;

            target.MagicDamage += target.Intellect * Constants.Units.Attributes.MagicDamagePerInt;
            target.ManaRegen += target.Intellect * Constants.Units.Attributes.ManaRegPerInt;
        }

        /// <summary>
        /// Applies the given buff to the unit. 
        /// </summary>
        /// <param name="buff">The buff to apply. </param>
        /// <param name="caster">The caster of the buff. </param>
        public BuffInstance TryApply(Unit caster, Buff buff)
        {
            if (Target.IsDead)
                return null;

            var newBuff = new BuffInstance(buff, caster, Target);
            buffs.TryAdd(newBuff);

            var existingBuffs = buffs
                .Where(oldBuff => oldBuff.Equals(newBuff))
                .OrderBy(b => b.DurationLeft)
                .ToList();

            //remove a stack if need be
            if (buff.MaxStacks > 0 && existingBuffs.Count >= buff.MaxStacks)
                buffs.TryRemove(existingBuffs.First());

            return newBuff;
        }

        /// <summary>
        /// Removes all instances of the given buff. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        public void Remove(Buff buffType)
        {
            var toRemove = buffs
                .Where(b => b.Prototype.Equals(buffType))
                .ToList();

            foreach (var b in toRemove)
                buffs.TryRemove(b);
        }

        /// <summary>
        /// Removes a specified number of instances of the given buff from this unit's buffs. 
        /// </summary>
        /// <param name="buffType">The buff prototype to remove instances of. </param>
        /// <param name="nStacks">The maximum number of stacks of this buff to remove. </param>
        public void Remove(Buff buffType, int nStacks)
        {
            var toRemove = buffs
                .Where(b => b.Prototype.Equals(buffType))
                .Take(nStacks)
                .ToList();

            foreach (var b in toRemove)
                buffs.TryRemove(b);
        }

        /// <summary>
        /// Removes the given buff instance from this unit. 
        /// </summary>
        /// <param name="buff"></param>
        public void Remove(BuffInstance buff)
        {
            if(buff != null)
                buffs.TryRemove(buff);
        }

        /// <summary>
        /// Purges all buffs from this unit. 
        /// </summary>
        public void Clear()
        {
            buffs.Clear();
        }


        #region IEnumerable<Buff> Implementation
        public IEnumerator<BuffInstance> GetEnumerator()
        {
            return buffs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffs.GetEnumerator();
        }
        #endregion
    }
}
