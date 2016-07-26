using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// Extension methods over the built-in collection types. 
    /// </summary>
    public static class CollectionExt
    {

        #region Key/Value Collections Extensions
        /// <summary>
        /// Tries to get the value of the given key from the dictionary. 
        /// Returns null if the key is not found. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict">The dictionary to perform the operation on. </param>
        /// <param name="key">The key whose value should be returned. </param>
        public static TVal TryGet<TKey, TVal>(this ConcurrentDictionary<TKey, TVal> dict, TKey key)
            where TVal : class
        {
            TVal result;
            if (key == null || !dict.TryGetValue(key, out result))
                return null;
            return result;
        }
        /// <summary>
        /// Tries to get the value of the given key from the dictionary. 
        /// Returns null if the key is not found. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict">The dictionary to perform the operation on. </param>
        /// <param name="key">The key whose value should be returned. </param>
        public static TVal TryGet<TKey, TVal>(this IReadOnlyDictionary<TKey, TVal> dict, TKey key)
            where TVal : class
        {
            TVal result;
            if (key == null || !dict.TryGetValue(key, out result))
                return null;
            return result;
        }

        /// <summary>
        /// Tries to get the value of the given key from the dictionary. 
        /// Returns null if the key is not found. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict">The dictionary to perform the operation on. </param>
        /// <param name="key">The key whose value should be returned. </param>
        public static TVal? TryGetVal<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key)
            where TVal : struct
        {
            TVal result;
            if (key == null || !dict.TryGetValue(key, out result))
                return null;
            return result;
        }



        /// <summary>
        /// Tries to get the value of the given key from this table. 
        /// Returns default(T) if the key is not found. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict">The dictionary to perform the operation on. </param>
        /// <param name="key">The key whose value should be returned. </param>
        public static TVal TryGet<TKey, TVal>(this ConditionalWeakTable<TKey, TVal> dict, TKey key)
            where TKey : class
            where TVal : class
        {
            TVal result;
            dict.TryGetValue(key, out result);

            return result;
        }
        #endregion


        #region IEnumerable Extensions        
        /// <summary>
        /// Counts the items in the given enumerable. 
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static int Count(this IEnumerable e)
        {
            if (e == null) return 0;

            var i = 0;
            foreach (var o in e)
                i++;
            return i;
        }
        #endregion


        #region IEnumerable<T> Extensions

        /// <summary>
        /// Prepends the specified item to the given enumerable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The enumerable to perform the operation on.</param>
        /// <param name="item">The item to prepend.</param>
        /// <returns></returns>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> e, T item)
        {
            yield return item;
            foreach (var eItem in e)
                yield return eItem;
        }

        /// <summary>
        /// Appends the specified item to the given enumerable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The enumerable to perform the operation on.</param>
        /// <param name="item">The item to append.</param>
        /// <returns></returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> e, T item)
        {
            foreach (var eItem in e)
                yield return eItem;
            yield return item;
        }

        public static int? IndexOf<T>(this IEnumerable<T> e, T item)
        {
            int id = 0;
            foreach (var it in e)
            {
                if (it.Equals(item))
                    return id;
                id++;
            }

            return null;
        }

        /// <summary>
        /// Gets the elements that score the highest according to a given function. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The enumerable whose elements are searched.</param>
        /// <param name="func">The function used to score the elements. </param>
        /// <returns></returns>
        public static IEnumerable<T> ArgMaxList<T>(this IEnumerable<T> e, Func<T, double> func)
        {
            if (!e.Any())
                return Enumerable.Empty<T>();

            return e.GroupBy(elem => func(elem))
                .ArgMax(g => g.Key);
        }

        /// <summary>
        /// Gets the element from the sequence that scores the highest according to the given function. 
        /// </summary>
        /// <typeparam name="TSrc">The type of the source.</typeparam>
        /// <typeparam name="TArg">The type of the argument.</typeparam>
        /// <param name="ie">The enumerable to search in.</param>
        /// <param name="func">The function that calculates each element's score.</param>
        /// <returns>The element that scores the highest.</returns>
        public static TSrc ArgMax<TSrc, TArg>(this IEnumerable<TSrc> ie, Func<TSrc, TArg> func) where TArg : IComparable<TArg>
            => ie.argMax(func, argMaxHelper);

        /// <summary>
        /// Gets the element from the sequence that scores the least according to the given function. 
        /// </summary>
        /// <typeparam name="TSrc">The type of the source.</typeparam>
        /// <typeparam name="TArg">The type of the argument.</typeparam>
        /// <param name="ie">The enumerable to search in.</param>
        /// <param name="func">The function that calculates each element's score.</param>
        /// <returns>The element that scores the least.</returns>
        public static TSrc ArgMin<TSrc, TArg>(this IEnumerable<TSrc> ie, Func<TSrc, TArg> func) where TArg : IComparable<TArg>
            => ie.argMax(func, argMinHelper);

        static TSrc argMax<TSrc, TArg>(this IEnumerable<TSrc> ie, Func<TSrc, TArg> func, Func<TArg, TArg, int> comparer)
        {
            var e = ie.GetEnumerator();
            if (!e.MoveNext())
                throw new InvalidOperationException("Sequence has no elements.");

            TSrc maxElem = e.Current;
            TArg maxVal = func(maxElem);
            if (!e.MoveNext())
                return maxElem;

            TSrc curElemm;
            TArg curVal;
            do
            {
                curElemm = e.Current;
                curVal = func(curElemm);
                if (comparer(curVal, maxVal) > 0)
                {
                    maxElem = curElemm;
                    maxVal = curVal;
                }
            }
            while (e.MoveNext());

            return maxElem;
        }


        static int argMaxHelper<T>(T a, T b) where T : IComparable<T>
            => a.CompareTo(b);

        static int argMinHelper<T>(T a, T b) where T : IComparable<T>
            => b.CompareTo(a);
        #endregion


        #region IList<T> Extensions

        /// <summary>
        /// Returns and removes the last element of the list. 
        /// Has a complexity of O(1)
        /// </summary>
        public static T Pop<T>(this List<T> l)
        {
            var id = l.Count - 1;
            var elem = l[id];
            l.RemoveAt(id);
            return elem;
        }

        #endregion
    }
}
