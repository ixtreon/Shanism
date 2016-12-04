using System;
using System.Collections;
using System.Collections.Generic;

namespace Shanism.Common.Util
{
    /// <summary>
    /// A generic <see cref="IComparer{T}"/> which uses a lambda function to compare elements. 
    /// </summary>
    /// <typeparam name="T">The type to compare.</typeparam>
    /// <seealso cref="System.Collections.Generic.IComparer{T}" />
    public class GenericComparer<T> : IComparer<T>, IComparer
    {
        readonly Func<T, T, int> compareFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComparer{T}"/> class.
        /// </summary>
        /// <param name="compareFunc">The function to compare elements of the type.</param>
        public GenericComparer(Func<T, T, int> compareFunc)
        {
            this.compareFunc = compareFunc;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        /// <exception cref="System.Exception">Can't compare peanuts to lambdas!</exception>
        public int Compare(object x, object y)
        {
            if (!(x is T) || !(y is T))
                throw new Exception("Can't compare peanuts to lambdas!");
            return compareFunc((T)x, (T)y);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(T x, T y)
            => compareFunc(x, y);
    }
}
