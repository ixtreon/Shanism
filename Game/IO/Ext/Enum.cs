using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// Provides generic access to the values of an enumeration. 
    /// </summary>
    /// <typeparam name="T">The enumeration to access the values of. </typeparam>
    public class Enum<T>
        where T : struct
    {
        /// <summary>
        /// Gets all the values of the provided enum. 
        /// </summary>
        public static IReadOnlyList<T> Values { get; } = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        /// <summary>
        /// Gets the names of all values in the provided enum. 
        /// </summary>
        public static IReadOnlyList<string> Names { get; } = Values.Select(val => Enum.GetName(typeof(T), val)).ToList();
    }
}
