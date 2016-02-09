using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public static class CollectionExt
    {

        #region Key/Value Collections Extensions
        /// <summary>
        /// Tries to get the value of the given key from the dictionary. 
        /// Returns default(T) if the key is not found. 
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key">The key whose value should be returned. </param>
        public static TVal TryGet<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key)
            where TVal : class
        {
            TVal result;
            if (key == null || !dict.TryGetValue(key, out result))
                return null;
            return result;
        }

        /// <summary>
        /// Tries to get the value of the given key from the dictionary. 
        /// Returns default(T) if the key is not found. 
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
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
        /// <typeparam name="TVal"></typeparam>
        /// <param name="key">The key whose value should be returned. </param>
        public static TVal TryGet<TKey, TVal>(this ConditionalWeakTable<TKey, TVal> dict, TKey val)
            where TKey : class
            where TVal : class
        {
            TVal result;
            dict.TryGetValue(val, out result);

            return result;
        }
        #endregion


        #region IEnumerable Extensions
        public static int Count(this IEnumerable e)
        {
            var i = 0;
            foreach (var o in e)
                i++;
            return i;
        }
        #endregion


        #region IEnumerable<T> Extensions

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> e, T item)
        {
            yield return item;
            foreach (var eItem in e)
                yield return eItem;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> e, T item)
        {
            foreach (var eItem in e)
                yield return eItem;
            yield return item;
        }

        public static T MostCommon<T>(this IEnumerable<T> list)
        {
            return list
                .GroupBy(e => e)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .First();
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

        #endregion

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

    }
}
