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

        /// <summary>
        /// Gets a mapping from an enum value to its id (position in the <see cref="Values"/> and <see cref="Names"/> lists).
        /// </summary>
        public static IReadOnlyDictionary<TEnum, int> ValueIds = Values
            .Select((v, i) => new { Val = v, Id = i })
            .ToDictionary(o => o.Val, o => o.Id);
    }

    public static class EnumExt
    {
        /// <summary>
        /// Gets the next value in the specified enum. Order is undefined. 
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="t">The element to get the next value for.</param>
        /// <returns></returns>
        public static TEnum GetNext<TEnum>(this TEnum t)
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            int id;
            if (!Enum<TEnum>.ValueIds.TryGetValue(t, out id) || (++id == Enum<TEnum>.Values.Count))
                return Enum<TEnum>.Values.First();

            return Enum<TEnum>.Values[id];
        }
    }
}
