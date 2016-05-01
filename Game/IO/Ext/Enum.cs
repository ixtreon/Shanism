using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// Provides fast, generic access to the values of an enumeration. 
    /// </summary>
    /// <typeparam name="TEnum">The enumeration to access the values of. </typeparam>
    public class Enum<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        /// <summary>
        /// Gets all the values of the provided enum type. 
        /// </summary>
        public static IReadOnlyList<TEnum> Values { get; } = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .OrderBy(e => e.ToString())
            .ToList();

        /// <summary>
        /// Gets the names of all values in the provided enum. 
        /// </summary>
        public static IReadOnlyList<string> Names { get; } = Values
            .Select(val => Enum.GetName(typeof(TEnum), val))
            .OrderBy(s => s)
            .ToList();
    }
}
