using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    class CombatSystem : UnitSystem
    {
        readonly Unit Owner;


        public CombatSystem(Unit target)
        {
            Owner = target;
        }


        /// <summary>
        /// Updates the <see cref="Unit.Mana"/> and <see cref="Unit.Life"/> based on <see cref="Unit.ManaRegen"/> and <see cref="Unit.LifeRegen"/>, respectively. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public override void Update(int msElapsed)
        {
            if (Owner.IsDead)
                return;

            updateStats(Owner);

            regenLife(Owner, msElapsed);

            if (Owner is Hero)
                updateHeroStats((Hero)Owner);
        }


        static void regenLife(Unit target, int msElapsed)
        {
            //life regen affects current life
            var curLife = target.Life;
            var lifeDelta = target.LifeRegen * msElapsed / 1000;
            var newLife = Math.Min(target.MaxLife, curLife + lifeDelta);

            if (lifeDelta < 0 && newLife < 0)
            {
                var killer = rollForKiller(target);
                target.Kill(killer);
            }

            target.Life = newLife;

            var manaDelta = target.ManaRegen * msElapsed / 1000;
            var newMana = Math.Min(target.MaxMana, target.Mana + manaDelta);
            target.Mana = newMana;
        }

        /// <summary>
        /// Updates the stats of the target unit based on the current buffs. 
        /// LINQing this method negatively affects performance under .NET 4.5.2
        /// </summary>
        static void updateStats(Unit target)
        {
            //stats affected by buffs

            //sum
            var maxLife = target.BaseMaxLife;
            var maxMana = target.BaseMaxMana;

            var minDamage = target.BaseMinDamage;
            var maxDamage = target.BaseMaxDamage;
            var defense = target.BaseDefense;
            var magicDamage = target.BaseMagicDamage;

            var lifeRegen = Constants.Units.BaseLifeRegen;
            var manaRegen = Constants.Units.BaseManaRegen;

            //sum + percentage
            var moveSpeed = target.BaseMoveSpeed;
            var bonusMoveSpeed = 1.0;
            var attacksPerSecond = target.BaseAttacksPerSecond;
            var bonusAtkSpeed = 1.0;

            //negative percentage (diminishing stacks)
            var negativeDodgeChance = (100 - target.BaseDodgeChance);
            var negativeCritChance = (100 - target.BaseCritChance);

            //flags
            var states = target.BaseStates;

            //per-buff values
            foreach (var b in target.Buffs)
            {
                //sum
                maxLife += b.MaxLife;
                maxMana += b.MaxMana;

                minDamage += b.MinDamage;
                maxDamage += b.MaxDamage;
                defense += b.Defense;
                //magicDamage += b.MagicDamage;     //TODO

                lifeRegen += b.LifeRegen;
                manaRegen += b.ManaRegen;

                //sum + percentage
                moveSpeed += b.MoveSpeed;
                bonusMoveSpeed += (b.MoveSpeedPercentage / 100.0);
                //attacksPerSecond += ...
                bonusAtkSpeed += (b.AttackSpeedPercentage / 100.0);

                //negative percentage
                negativeDodgeChance = negativeDodgeChance * (100 - b.Dodge) / 100;
                //negativeCritChance = negativeCritChance * (100 - b.Crit) / 100;       //TODO

                //flags
                states |= b.UnitStates;
            }

            target.MaxLife = maxLife;
            target.MaxMana = maxMana;

            target.MinDamage = minDamage;
            target.MaxDamage = maxDamage;
            target.Defense = defense;
            target.MagicDamage = magicDamage;

            target.LifeRegen = lifeRegen;
            target.ManaRegen = manaRegen;


            target.MoveSpeed = Math.Max(0, (double)(moveSpeed * bonusMoveSpeed));
            target.AttacksPerSecond = Math.Max(0, (double)(attacksPerSecond * bonusAtkSpeed));


            target.DodgeChance = 100 - negativeDodgeChance;
            target.CritChance = 100 - negativeCritChance;


            target.States = states;
        }

        static void updateHeroStats(Hero target)
        {

            target.Strength = target.BaseStrength;
            target.Vitality = target.BaseVitality;
            target.Intellect = target.BaseIntellect;
            target.Agility = target.BaseAgility;

            foreach (var b in target.Buffs)
            {
                target.Strength += b.Strength;
                target.Vitality += b.Vitality;
                target.Intellect += b.Intellect;
                target.Agility += b.Agility;
            }

            target.MinDamage += target.Strength * Constants.Heroes.Attributes.DamagePerStrength;
            target.MaxDamage += target.Strength * Constants.Heroes.Attributes.DamagePerStrength;
            target.Defense += target.Strength * Constants.Heroes.Attributes.DefensePerStrength;

            target.Defense += target.Agility * Constants.Heroes.Attributes.AtkSpeedPerAgility;
            target.Defense += target.Agility * Constants.Heroes.Attributes.DodgePerAgility;

            target.MaxLife += target.Vitality * Constants.Heroes.Attributes.LifePerVitality;
            target.MaxMana += target.Vitality * Constants.Heroes.Attributes.ManaPerVitality;

            target.LifeRegen += target.Intellect * Constants.Heroes.Attributes.LifeRegPerInt;
            target.ManaRegen += target.Intellect * Constants.Heroes.Attributes.ManaRegPerInt;

            target.MagicDamage += target.Intellect * Constants.Heroes.Attributes.MagicDamagePerInt;
            target.ManaRegen += target.Intellect * Constants.Heroes.Attributes.ManaRegPerInt;
        }


        static Unit rollForKiller(Unit target)
        {
            //get life degen from each source
            var dmgs = new Dictionary<Unit, double>();
            foreach (var b in target.Buffs)
                dmgs[b.Caster ?? target] -= b.LifeRegen;

            //leave only positive degeneration
            foreach (var kvp in dmgs.Where(kvp => kvp.Value < 0).ToList())
                dmgs.Remove(kvp.Key);

            //get their sum
            var sum = dmgs.Values.Sum();
            var roll = Rnd.NextDouble(0, sum);

            foreach (var kvp in dmgs)
            {
                roll -= kvp.Value;
                if (roll < 0)
                    return kvp.Key;
            }

            return null;
        }
    }
}
