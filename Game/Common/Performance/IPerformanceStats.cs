using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Performance
{
    public interface IPerformanceStats
    {
        /// <summary>
        /// Gets the measurements recorded in this performance stats window. 
        /// </summary>
        IReadOnlyDictionary<string, long> Measurements { get; }
    }
}
