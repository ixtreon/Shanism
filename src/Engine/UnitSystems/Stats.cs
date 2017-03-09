using Shanism.Common;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Systems
{
    class StatsSystem : UnitSystem
    {
        readonly Unit Owner;


        public StatsSystem(Unit target)
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
            if (Owner is Hero)
                updateHeroStats((Hero)Owner);

            regenLife(Owner, msElapsed);
        }


        static void regenLife(Unit target, int msElapsed)
        {
            //mana & life regen as % of max
            var lifeDelta = (target.LifeRegen / target.MaxLife) * msElapsed / 1000;
            var newLife = Math.Min(target.LifePercentage + lifeDelta, 1);

            // can die by HP degen
            if (lifeDelta < 0 && newLife < 0)
            {
                target.LifePercentage = 0;

                var killer = rollForKiller(target);
                target.Kill(killer);
                return;
            }

            //mana regen in % of max mana
            if (target.MaxMana > 0)
            {
                var manaDelta = (target.ManaRegen / target.MaxMana) * msElapsed / 1000;
                var newMana = (target.ManaPercentage + manaDelta).Clamp(0, 1);

                target.LifePercentage = newLife;
                target.ManaPercentage = newMana;
            }
        }

        /// <summary>
        /// Updates the stats of the target unit based on the current buffs. 
        /// </summary>
        static void updateStats(Unit target)
        {
            //stats affected by buffs
            target.refreshStats();

            //percentage
            var bonusMoveSpeed = 1f;
            var bonusAtkSpeed = 1f;
            //negative percentage
            var negativeDodgeChance = (100 - target.BaseDodgeChance);
            var negativeCritChance = (100 - target.BaseCritChance);
            //flags
            var states = target.BaseStates;

            //per-buff values
            foreach (var b in target.Buffs)
            {
                //sum of percentages
                bonusMoveSpeed += (b.Prototype.MoveSpeedPercentage / 100f);
                bonusAtkSpeed += (b.Prototype.AttackSpeedPercentage / 100f);
                //product of (negative) percentage
                negativeDodgeChance = negativeDodgeChance * (100 - b.Prototype.Dodge) / 100;
                //OR-ed together
                states |= b.Prototype.StateFlags;
            }

            target.stats[UnitStat.MoveSpeed] *= bonusMoveSpeed;
            target.stats[UnitStat.AttacksPerSecond] *= bonusAtkSpeed;

            target.DodgeChance = 100 - negativeDodgeChance;
            target.CritChance = 100 - negativeCritChance;

            target.StateFlags = states;
        }

        static readonly float[] statModMults =
        {
            Constants.Heroes.Attributes.LifePerVitality,
            Constants.Heroes.Attributes.ManaPerVitality,

            Constants.Heroes.Attributes.LifeRegPerInt,
            Constants.Heroes.Attributes.ManaRegPerInt,
            Constants.Heroes.Attributes.MagicDamagePerInt,

            Constants.Heroes.Attributes.DamagePerStrength,
            Constants.Heroes.Attributes.DamagePerStrength,
            Constants.Heroes.Attributes.DefensePerStrength,
            
            0,
            Constants.Heroes.Attributes.AtkSpeedPerAgility,
            Constants.Heroes.Attributes.DodgePerAgility,
        };

        static readonly HeroAttribute[] statModTypes =
        {
            HeroAttribute.Vitality,
            HeroAttribute.Vitality,

            HeroAttribute.Intellect,
            HeroAttribute.Intellect,
            HeroAttribute.Intellect,

            HeroAttribute.Strength,
            HeroAttribute.Strength,
            HeroAttribute.Strength,

            HeroAttribute.Agility,
            HeroAttribute.Agility,
            HeroAttribute.Agility,
        };

        static void updateHeroStats(Hero target)
        {
            target.updateHeroStats();

            for (int i = 0; i < Enum<UnitStat>.Count; i++)
            {
                var attrMod = target.attributes[statModTypes[i]] * statModMults[i];
                target.stats.Add(i, attrMod);
            }
        }


        static Unit rollForKiller(Unit target)
        {
            //get life degen from each source
            var dmgs = new Dictionary<Unit, double>();
            foreach (var b in target.Buffs)
                dmgs[b.Caster ?? target] -= b.Prototype.LifeRegen;

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
