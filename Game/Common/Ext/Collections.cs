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
        /// Gets the element from the sequence that scores the highest according to the given function. 
        /// </summary>
        /// <typeparam name="TSrc">The type of the source.</typeparam>
        /// <typeparam name="TArg">The type of the argument.</typeparam>
        /// <param name="ie">The enumerable to search in.</param>
        /// <param name="func">The function that calculates each element's score.</param>
        /// <returns>The element that scores the highest.</returns>
        public static TSrc ArgMax<TSrc, TArg>(this IEnumerable<TSrc> ie, Func<TSrc, TArg> func) 
            where TArg : IComparable<TArg>
            => ie.argMax(func, argMaxHelper);

        /// <summary>
        /// Gets the element from the sequence that scores the least according to the given function. 
        /// </summary>
        /// <typeparam name="TSrc">The type of the source.</typeparam>
        /// <typeparam name="TArg">The type of the argument.</typeparam>
        /// <param name="ie">The enumerable to search in.</param>
        /// <param name="func">The function that calculates each element's score.</param>
        /// <returns>The element that scores the least.</returns>
        public static TSrc ArgMin<TSrc, TArg>(this IEnumerable<TSrc> ie, Func<TSrc, TArg> func) 
            where TArg : IComparable<TArg>
            => ie.argMax(func, argMinHelper);

        static TSrc argMax<TSrc, TArg>(this IEnumerable<TSrc> ie, Func<TSrc, TArg> func, Func<TArg, TArg, int> comparer)
        {
            //len = 0
            var e = ie.GetEnumerator();
            if (!e.MoveNext())  throw new InvalidOperationException("Sequence has no elements.");

            //len = 1
            TSrc maxElem = e.Current;
            TArg maxVal = func(maxElem);
            if (!e.MoveNext())  return maxElem;

            //len > 1
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


        /// <summary>
        /// Removes and returns the last element of the list. 
        /// </summary>
        public static T Pop<T>(this IList<T> l)
        {
            var id = l.Count - 1;
            var elem = l[id];
            l.RemoveAt(id);
            return elem;
        }
    }
}
