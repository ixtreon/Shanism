using Shanism.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    public static class UnitStat
    {
        public const int MaxLife = 0;
        public const int MaxMana = 1;

        public const int LifeRegen = 2;
        public const int ManaRegen = 3;
        public const int MagicDamage = 4;

        public const int MinDamage = 5;
        public const int MaxDamage = 6;
        public const int Defense = 7;

        public const int MoveSpeed = 8;
        public const int AttacksPerSecond = 9;
        public const int AttackRange = 10;


        public const int Count = 11;
    }

    public static class HeroAttribute
    {
        public const int Strength = 0;
        public const int Agility = 1;
        public const int Vitality = 2;
        public const int Intellect = 3;

        public const int Count = 4;
    }

    public class UnitStats : Stats, IUnitStats
    {
        public UnitStats() : base(UnitStat.Count)
        {
            this[UnitStat.MaxLife] = 10;
            this[UnitStat.MaxMana] = 0;

            this[UnitStat.LifeRegen] = 0.1f;
            this[UnitStat.ManaRegen] = 0;
            this[UnitStat.MagicDamage] = 0;

            this[UnitStat.MinDamage] = 1;
            this[UnitStat.MaxDamage] = 2;
            this[UnitStat.Defense] = 0;

            this[UnitStat.MoveSpeed] = 10;
            this[UnitStat.AttacksPerSecond] = 0.6f;
            this[UnitStat.AttackRange] = 2.5f;
        }
    }

    public class HeroAttributes : Stats, IHeroAttributes
    {
        public HeroAttributes() : base(HeroAttribute.Count) { }

        public HeroAttributes(float val) : base(HeroAttribute.Count, val) { }
    }

    public interface IUnitStats : IStats { }
    public interface IHeroAttributes : IStats { }

    public interface IStats
    {
        float this[int val] { get; }


        void Write(BinaryWriter wr);
        void Read(BinaryReader r);
    }

    public class Stats
    {
        readonly float[] stats;

        internal Stats(int count)
        {
            stats = new float[count];
        }

        internal Stats(int count, float initVal)
        {
            stats = Enumerable.Repeat(initVal, count).ToArray();
        }

        public void Add(float[] other, int offset, int count)
        {
            for (int i = offset; i < count; i++)
                stats[i] += other[i];
        }

        public void Set(float[] other, int offset, int count)
        {
            for (int i = offset; i < count; i++)
                stats[i] = other[i];
        }

        public void Add(Stats other) => Add(other.stats, 0, stats.Length);
        public void Set(Stats other) => Set(other.stats, 0, stats.Length);

        public void Add(int id, float val) => stats[id] += val;
        public void Set(int id, float val) => stats[id] = val;

        public float this[int id]
        {
            get { return stats[id]; }
            set { stats[id] = value; }
        }

        public void Write(BinaryWriter wr)
        {
            for (int i = 0; i < stats.Length; i++)
                wr.Write(stats[i]);
        }

        public void Read(BinaryReader r)
        {
            for (int i = 0; i < stats.Length; i++)
                stats[i] = r.ReadSingle();
        }
    }
}
