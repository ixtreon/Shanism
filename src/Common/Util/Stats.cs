using Shanism.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    public enum UnitStat
    {
        MaxLife,
        MaxMana,

        LifeRegen,
        ManaRegen,
        MagicDamage,

        MinDamage,
        MaxDamage,
        Defense,

        MoveSpeed,
        AttacksPerSecond,
        AttackRange,
    }

    public enum HeroAttribute
    {
        Strength, Agility, Vitality, Intellect,
    }

    public class UnitStats : Stats<UnitStat>, IUnitStats
    {
        public UnitStats() { }

        public UnitStats(float val) : base(val) { }


        public float this[UnitStat val]
        {
            get { return RawStats[(int)val]; }
            set { RawStats[(int)val] = value; }
        }
    }

    public class HeroAttributes : Stats<HeroAttribute>, IHeroAttributes
    {
        public HeroAttributes() { }

        public HeroAttributes(float val) : base(val) { }


        public float this[HeroAttribute val]
        {
            get { return RawStats[(int)val]; }
            set { RawStats[(int)val] = value; }
        }
    }

    public interface IUnitStats : IStats<UnitStat>
    {
        float this[UnitStat val] { get; }
    }

    public interface IHeroAttributes : IStats<HeroAttribute>
    {
        float this[HeroAttribute val] { get; }
    }

    public interface IStats<T>
    {
        float[] RawStats { get; }

        int Count { get; }
    }

    public class Stats<T> : IStats<T>
        where T : struct, IConvertible, IComparable, IFormattable
    {
        public float[] RawStats { get; }

        public int Count { get; }

        internal Stats()
        {
            Count = Enum<T>.Count;
            RawStats = new float[Count];
        }

        internal Stats(float initVal)
        {
            Count = Enum<T>.Count;
            RawStats = Enumerable.Repeat(initVal, Count).ToArray();
        }

        public void Add(float[] other, int offset, int count)
        {
            for (int i = offset; i < count; i++)
                RawStats[i] += other[i];
        }

        public void Set(float[] other, int offset, int count)
        {
            for (int i = offset; i < count; i++)
                RawStats[i] = other[i];
        }

        public void Add(Stats<T> other) => Add(other.RawStats, 0, RawStats.Length);
        public void Set(Stats<T> other) => Set(other.RawStats, 0, RawStats.Length);

        public void Add(int id, float val) => RawStats[id] += val;
        public void Set(int id, float val) => RawStats[id] = val;
    }
}
