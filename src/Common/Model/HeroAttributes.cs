using System.Linq;
using System.Numerics;

namespace Shanism.Common
{

    public enum HeroAttribute
    {
        Strength, Agility, Vitality, Intellect,
    }

    public struct HeroAttributes : IHeroAttributes, IStats
    {
        static readonly int Length = ((Enum<UnitField>.Count - 1) / Vector<float>.Count + 1) * Vector<float>.Count; 

        readonly float[] values;


        public HeroAttributes(float val)
        {
            values = Enumerable.Repeat(val, Length).ToArray();
        }


        public float[] RawStats => values;

        public int Count => values.Length;

        public ref float Get(HeroAttribute val) => ref values[(int)val];

        public float this[HeroAttribute val]
        {
            get => values[(int)val];
            set => values[(int)val] = value;
        }
    }
    public interface IHeroAttributes
    {
        float this[HeroAttribute val] { get; }
        int Count { get; }
        float[] RawStats { get; }
    }
}
