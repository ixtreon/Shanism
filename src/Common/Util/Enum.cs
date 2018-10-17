using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// Provides fast, generic access to the values of an enumeration. 
    /// </summary>
    /// <typeparam name="TEnum">The enumeration to access the values of. </typeparam>
    public static class Enum<TEnum>
        where TEnum : Enum
    {
        /// <summary>
        /// Gets all declared values of this enum type,
        /// ordered by their numerical representation.
        /// </summary>
        public static IReadOnlyList<TEnum> Values { get; }
            = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .OrderBy(v => v)
                .ToArray();

        public static TEnum MaxValue { get; } = Values.Max();

        public static TEnum MinValue { get; } = Values.Min();

        /// <summary>
        /// Gets a mapping from the name of an enum member to its value.
        /// </summary>
        public static IReadOnlyDictionary<string, TEnum> NameValueLookup { get; }
            = Values.ToDictionary(
                o => Enum.GetName(typeof(TEnum), o),
                o => o);
        /// <summary>
        /// Gets a mapping from the name of an enum member to its value.
        /// </summary>
        public static IReadOnlyDictionary<TEnum, int> ValueIdLookup { get; }
            = Enumerable.Range(0, Values.Count)
                .ToDictionary(i => Values[i], i => i);

        /// <summary>
        /// Gets the number of constants defined in this enum type. 
        /// </summary>
        public static int Count { get; } = Values.Count;

        /// <summary>
        /// Gets the names of all values in this enum type. 
        /// </summary>
        public static IReadOnlyList<string> Names { get; } = Values
            .Select(v => Enum.GetName(typeof(TEnum), v))
            .ToArray();
    }

    public static class EnumExt
    {
        /// <summary>
        /// Gets the next value in the specified enum. 
        /// Order is defined by the numerical representation of the values. 
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="t">The element to get the next value for.</param>
        public static TEnum FindNext<TEnum>(this TEnum t)
            where TEnum : Enum
        {
            var vals = Enum<TEnum>.Values;
            var id = vals.IndexOf(t);

            if (id != null && ++id < vals.Count)
                return vals[id.Value];

            return vals[0];
        }
    }
}
