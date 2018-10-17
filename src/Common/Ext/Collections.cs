using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            if(key == null || !dict.TryGetValue(key, out result))
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
        [Obsolete]
        public static TVal? TryGetVal<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key)
            where TVal : struct
        {
            TVal result;
            if(key == null || !dict.TryGetValue(key, out result))
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
            foreach(var eItem in e)
                yield return eItem;
        }


        public static int? IndexOf<T>(this IEnumerable<T> e, T item)
        {
            int id = 0;
            foreach(var it in e)
            {
                if(it.Equals(item))
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
            if(!e.MoveNext()) throw new InvalidOperationException("Sequence has no elements.");

            //len = 1
            TSrc maxElem = e.Current;
            TArg maxVal = func(maxElem);
            if(!e.MoveNext()) return maxElem;

            //len > 1
            TSrc curElemm;
            TArg curVal;
            do
            {
                curElemm = e.Current;
                curVal = func(curElemm);
                if(comparer(curVal, maxVal) > 0)
                {
                    maxElem = curElemm;
                    maxVal = curVal;
                }
            }
            while(e.MoveNext());

            return maxElem;
        }


        static int argMaxHelper<T>(T a, T b) where T : IComparable<T>
            => a.CompareTo(b);

        static int argMinHelper<T>(T a, T b) where T : IComparable<T>
            => b.CompareTo(a);


        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> e)
            => new HashSet<T>(e);
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

        /// <summary>
        /// Removes the last element of the list. 
        /// </summary>
        public static void RemoveLast<T>(this IList<T> l) => l.Pop();

        /// <summary>
        /// Moves the last item to the given index 
        /// and then drops the last item of the collection.
        /// <para>
        /// IF ITERATING DO NOT FORGET TO SUBTRACT 1 FROM THE COUNTER.
        /// </para>
        /// </summary>
        public static void RemoveAtFast<T>(this List<T> l, int id)
        {
            var lastId = l.Count - 1;
            if(id < lastId)
                l[id] = l[lastId];
            l.RemoveAt(lastId);
        }

        public static void InsertSorted<T>(this List<T> l, T val)
        {
            var index = l.BinarySearch(val);
            if (index < 0) index = ~index;
            l.Insert(index, val);
        }

        public static void InsertSorted<T>(this List<T> l, T val, IComparer<T> comparer)
        {
            var index = l.BinarySearch(val, comparer);
            if (index < 0) index = ~index;
            l.Insert(index, val);
        }

        public static void Swap<T>(this IList<T> l, int i, int j)
        {
            var temp = l[i];
            l[i] = l[j];
            l[j] = temp;
        }


        /// <summary>
        /// Moves the given item to the end of the list. 
        /// Returns whether the control was found in the list. 
        /// </summary>
        /// <remarks>
        /// Has a complexity of O(n). Walks the list twice. 
        /// </remarks>
        public static bool MoveToLast<T>(this IList<T> l, T item)
        {
            var oldPos = l.IndexOf(item);
            if(oldPos < 0)
                return false;

            var lastId = l.Count - 1;
            for(var i = oldPos; i < lastId; i++)
                l[i] = l[i + 1];
            l[lastId] = item;

            return true;
        }

        /// <summary>
        /// Moves the given item to the start of the list. 
        /// Returns whether the control was found in the list. 
        /// </summary>
        /// <remarks>
        /// Has a complexity of O(n). Walks the list twice. 
        /// </remarks>
        public static bool MoveToFirst<T>(this IList<T> l, T item)
        {
            var oldPos = l.IndexOf(item);
            if(oldPos < 0)
                return false;

            for(var i = oldPos; i > 0; i--)
                l[i] = l[i - 1];
            l[0] = item;

            return true;
        }
    }
}
