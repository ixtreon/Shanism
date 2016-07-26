using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Entities
{
    public class StatsData
    {
        public static readonly int MaxFloats = Enum<FloatStat>.Count;
        public static readonly int MaxInts = Enum<IntStat>.Count;
        public static readonly int MaxBools = Enum<BoolStat>.Count;


        public readonly float[] floats;
        public readonly int[] ints;
        public int bools = 0;

        static int boolBit(BoolStat s) => 1 << (int)s;

        void setBool(BoolStat s, bool val)
        {
            var f = boolBit(s);
            if (val)
                bools |= f;
            else
                bools &= (~f);
        }

        bool getBool(BoolStat s)
        {
            return (bools & boolBit(s)) != 0;
        }


        public StatsData(int bools, float[] floats, int[] ints)
        {
            this.bools = bools;
            this.floats = floats;
            this.ints = ints;
        }

        public StatsData()
        {
            ints = new int[MaxInts];
            floats = new float[MaxFloats];
        }


        public bool this[BoolStat st]
        {
            get { return getBool(st); }
            set { setBool(st, value); }
        }

        public int this[IntStat st]
        {
            get { return ints[(int)st]; }
            set { ints[(int)st] = value; }
        }

        public float this[FloatStat st]
        {
            get { return floats[(int)st]; }
            set { floats[(int)st] = value; }
        }

    }

    public class StatsSerializer
    {
        static readonly int IntBytes = StatsData.MaxInts * sizeof(int);
        static readonly int FloatBytes = StatsData.MaxFloats * sizeof(float);
        static readonly int BoolBytes = sizeof(int);

        static readonly int BoolOffset = 0;
        static readonly int FloatOffset = BoolOffset + BoolBytes;
        static readonly int IntOffset = FloatOffset + FloatBytes;

        static readonly int TotalBytes = IntBytes + FloatBytes + BoolBytes;



        readonly byte[] buffer = new byte[256];

        public void Save(Stream s, StatsData datas)
        {

            writeInt(buffer, BoolOffset, datas.bools);
            Buffer.BlockCopy(datas.ints, 0, buffer, IntOffset, IntBytes);
            Buffer.BlockCopy(datas.floats, 0, buffer, FloatOffset, FloatBytes);

            s.Write(buffer, 0, TotalBytes);
        }

        public StatsData Load(Stream s)
        {
            s.Read(buffer, 0, TotalBytes);
            int[] ints = new int[StatsData.MaxInts];
            float[] floats = new float[StatsData.MaxFloats];
            int bools;

            bools = readInt(buffer, BoolOffset);
            Buffer.BlockCopy(buffer, IntOffset, ints, 0, IntBytes);
            Buffer.BlockCopy(buffer, FloatOffset, floats, 0, FloatBytes);

            return new StatsData(bools, floats, ints);
        }

        public void Update(Stream s, StatsData d)
        {
            s.Read(buffer, 0, TotalBytes);

            d.bools = readInt(buffer, BoolOffset);
            Buffer.BlockCopy(buffer, IntOffset, d.ints, 0, IntBytes);
            Buffer.BlockCopy(buffer, FloatOffset, d.floats, 0, FloatBytes);
        }

        static void writeInt(byte[] bytes, int offset, int intValue)
        {
            bytes[0] = (byte)(intValue >> 24);
            bytes[1] = (byte)(intValue >> 16);
            bytes[2] = (byte)(intValue >> 8);
            bytes[3] = (byte)intValue;
        }

        static int readInt(byte[] bytes, int offset)
        {
            return bytes[offset] << 24 
                | bytes[offset + 1] << 16
                | bytes[offset + 2] << 8 
                | bytes[offset + 3];
        }
    }

    public enum IntStat
    {
        //buffs, units, heroes
        AttackSpeedPerc,
        MoveSpeedPerc,
        StateFlags,
    }

    public enum BoolStat
    {
        //buffs, units, heroes
        CanFly,
        CanSwim,
        CanWalk,


        //units, heroes
        IsDead,
        IsMoving,
    }



    public enum FloatStat
    {
        //buffs, units, heroes
        CurLife,
        MaxLife,
        LifeRegen,

        CurMana,
        MaxMana,
        ManaRegen,

        Crit,
        Dodge,

        MinDamage,
        MaxDamage,
        Defense,

        Strength,
        Agility,
        Vitality,
        Intellect,

        VisionRange,

        //units, heroes
        MoveDirection,

        MoveSpeed,
        AttackCooldown,
    }
}
