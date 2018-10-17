using Shanism.Common;
using Shanism.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            if(Owner.IsDead)
                return;

            updateStats(Owner);
            if(Owner is Hero h)
                updateHeroStats(h);

            regenLife(Owner, msElapsed);
        }


        static void regenLife(Unit target, int msElapsed)
        {
            //mana & life regen as % of max
            var lifeDelta = (target.LifeRegen / target.MaxLife) * msElapsed / 1000;
            var newLife = Math.Min(target.LifePercentage + lifeDelta, 1);

            // can die by HP degen
            if(lifeDelta < 0 && newLife < 0)
            {
                target.LifePercentage = 0;

                var killer = rollForKiller(target);
                target.Kill(killer);
                return;
            }

            //mana regen in % of max mana
            if(target.MaxMana > 0)
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
            var baseStats = target.baseStats;
            var curStats = target.stats;

            // MoveSpeed, AttackSpeed - summing percentage bonus
            var bonusMoveSpeed = 1f;
            var bonusAtkSpeed = 1f;
            // Dodge, Critical Strike -> product of negative percentages
            var negativeDodgeChance = (100 - target.BaseDodgeChance);
            var negativeCritChance = (100 - target.BaseCritChance);
            // State Flags -> 'or' together
            var states = target.BaseStates;
            // Others -> sum
            curStats.Set(baseStats);

            //per-buff values
            var buffs = target.Buffs;
            for(int i = 0; i < buffs.Count; i++)
            {
                var b = buffs[i];

                // perc bonus -> sum
                bonusMoveSpeed += (b.Prototype.MoveSpeedPercentage / 100f);
                bonusAtkSpeed += (b.Prototype.AttackSpeedPercentage / 100f);
                // neg perc -> product
                negativeDodgeChance = negativeDodgeChance * (100 - b.Prototype.Dodge) / 100;
                // flags -> OR
                states |= b.Prototype.StateFlags;
                // rest -> sum
                curStats.Add(b.Prototype.unitStats);
            }

            // perc bonus
            curStats[UnitField.MoveSpeed]        = baseStats[UnitField.MoveSpeed] * Math.Max(0, bonusMoveSpeed);
            curStats[UnitField.AttacksPerSecond] = baseStats[UnitField.AttacksPerSecond] * Math.Max(0, bonusAtkSpeed);
            // neg perc
            target.DodgeChance = 100 - negativeDodgeChance;
            target.CritChance = 100 - negativeCritChance;
            // flags
            target.StateFlags = states;
            // rest is set
        }

        static readonly UnitStats statMultipliers = new UnitStats(new[]
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
        });

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

        readonly UnitStats attrStatsTemp = new UnitStats(0);

        void updateHeroStats(Hero target)
        {
            target.updateHeroStats();

            // load the VIT/INT/STR/AGI stats for the hero
            for(var i = 0; i < statModTypes.Length; i++)
                attrStatsTemp.Get((UnitField)i) = target.attributes[statModTypes[i]];

            // multiply by the modifier constants to get HP/MANA/HPREG/...
            attrStatsTemp.Multiply(statMultipliers);

            // add HP/MANA/HPREG/... modifiers to cur stats
            target.stats.Add(attrStatsTemp);
        }


        static Unit rollForKiller(Unit target)
        {
            //get life degen from each source
            var dmgs = new Dictionary<Unit, double>();
            for(int i = 0; i < target.Buffs.Count; i++)
            {
                var b = target.Buffs[i];
                dmgs[b.Caster ?? target] -= b.Prototype.LifeRegen;
            }

            //leave only positive degeneration
            foreach(var kvp in dmgs.Where(kvp => kvp.Value < 0).ToList())
                dmgs.Remove(kvp.Key);

            //get their sum
            var sum = dmgs.Values.Sum();
            var roll = Rnd.NextDouble(0, sum);

            foreach(var kvp in dmgs)
            {
                roll -= kvp.Value;
                if(roll < 0)
                    return kvp.Key;
            }

            return null;
        }
    }
}
