using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IO
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


        /// <summary>
        /// Drops the last value in the sequence. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static IEnumerable<T> DropLast<T>(this IEnumerable<T> e)
        {
            if (e == null || !e.Any()) throw new Exception("Sequence contains no elements!");

            T lastVal = e.First();
            foreach (var newVal in e.Skip(1))
            {
                yield return lastVal;
                lastVal = newVal;
            }
        }

        /// <summary>
        /// Gets the element that scores the highest according to the given function. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e">The enumerable whose elements are searched.</param>
        /// <param name="func">The function used to score the elements. </param>
        /// <returns></returns>
        public static IEnumerable<T> ArgMaxList<T>(this IEnumerable<T> e, Func<T, double> func)
        {
            if (!e.Any())
                return Enumerable.Empty<T>();

            return e
                .Select(i => new { Key = func(i), Val = i })
                .GroupBy(z => z.Key)
                .Aggregate((ga, gb) => (ga.Key > gb.Key) ? ga : gb)
                .Select(z => z.Val);
        }

        #endregion

    }
}
