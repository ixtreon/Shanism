using System;
using System.Linq;
using System.Numerics;

namespace Shanism.Common
{
    public enum UnitField
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

    public interface IUnitStats
    {
        float this[UnitField val] { get; }
        int Count { get; }
        float[] RawStats { get; }
    }

    public struct UnitStats : IUnitStats, IStats
    {
        static readonly int Length = ((Enum<UnitField>.Count - 1) / Vector<float>.Count + 1) * Vector<float>.Count; 

        readonly float[] values;

        public UnitStats(float val)
        {
            values = Enumerable.Repeat(val, Length).ToArray();
        }

        public UnitStats(float[] values)
        {
            if (values.Length != Length)
                Array.Resize(ref values, Length);

            this.values = values;
        }

        public float[] RawStats => values;
        public int Count => values.Length;

        public ref float Get(UnitField val) => ref values[(int)val];

        public float this[UnitField val]
        {
            get => values[(int)val];
            set => values[(int)val] = value;
        }
    }
}
