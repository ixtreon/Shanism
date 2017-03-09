using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.Util
{
    class RangeAttribute : Attribute
    {
        internal static readonly RangeAttribute Default = new RangeAttribute(int.MinValue, int.MaxValue);

        public readonly int Maximum;
        public readonly int Minimum;
        public readonly int Step;

        public RangeAttribute(int min, int max, int step = 1)
        {
            Minimum = min;
            Maximum = max;
            Step = step;
        }
    }
}
