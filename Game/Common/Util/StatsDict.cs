using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /* Unused */


    enum StatType
    {
        Life,
        MaxLife,
        LifeRegen,

        Mana,
        MaxMana,
        ManaRegen,

        Dodge, Crit, Defense, MagicDamage,

        MoveSpeed,

        AttacksPerSecond,
        MinDamage, MaxDamage,

    }

    class StatsDict
    {
        readonly double[] _stats = new double[Enum<StatType>.Count];

        public StatsDict()
        {

        }

        public double this[StatType i]
        {
            get { return _stats[(int)i]; }
            set { _stats[(int)i] = value; }
        }
    }

    struct StatsStruct
    {
        readonly StatsDict dict;
        readonly bool isReadOnly;

        public StatsStruct(StatsDict dict, bool readOnly)
        {
            this.dict = dict;
            isReadOnly = readOnly;
        }

        double get(StatType i) => dict[i];

        void set(StatType i, double val)
        {
            if (!isReadOnly)
                dict[i] = val;
        }

        public double Life
        {
            get { return get(StatType.Life); }
            set { set(StatType.Life, value); }
        }
        public double Mana
        {
            get { return get(StatType.Mana); }
            set { set(StatType.Mana, value); }
        }
        public double Dodge
        {
            get { return get(StatType.Dodge); }
            set { set(StatType.Dodge, value); }
        }
        public double Crit
        {
            get { return get(StatType.Crit); }
            set { set(StatType.Crit, value); }
        }
        public double Defense
        {
            get { return get(StatType.Defense); }
            set { set(StatType.Defense, value); }
        }
        public double MagicDamage
        {
            get { return get(StatType.MagicDamage); }
            set { set(StatType.MagicDamage, value); }
        }

        public double MoveSpeed
        {
            get { return get(StatType.MoveSpeed); }
            set { set(StatType.MoveSpeed, value); }
        }

        public double MinDamage
        {
            get { return get(StatType.MinDamage); }
            set { set(StatType.MinDamage, value); }
        }
        public double MaxDamage
        {
            get { return get(StatType.MaxDamage); }
            set { set(StatType.MaxDamage, value); }
        }

        public double AttacksPerSecond
        {
            get { return get(StatType.AttacksPerSecond); }
            set { set(StatType.AttacksPerSecond, value); }
        }
    }
}
