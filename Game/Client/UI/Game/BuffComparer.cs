using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    class BuffComparer : IComparer<IBuff>
    {
        public int Compare(IBuff x, IBuff y)
        {
            return x.FullDuration.CompareTo(y.FullDuration);
        }
    }
}
