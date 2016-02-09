using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Util
{
    public class ConcurrentSet<T> : IEnumerable<T>
    {
        ConcurrentDictionary<T, bool> dict { get; } = new ConcurrentDictionary<T, bool>();

        public int Count
        {
            get { return dict.Count; }
        }

        public ConcurrentSet() { }

        /// <summary>
        /// Attempts to add the specified value to this <see cref="ConcurrentSet{T}"/>.
        /// </summary>
        /// <param name="val">The value of the element to add. </param>
        /// <returns>true if the value was successfully added to the set; 
        /// false if the item already existed in the set. </returns>
        public bool TryAdd(T val)
        {
            return dict.TryAdd(val, true);
        }

        /// <summary>
        /// Attempts to remove the specified element to this <see cref="ConcurrentSet{T}"/>.
        /// </summary>
        /// <param name="val">The value of the element to remove. </param>
        /// <returns>true if the value was successfully removed from the set; 
        /// false if the item was not found in the set. </returns>
        public bool TryRemove(T val)
        {
            bool ret;
            return dict.TryRemove(val, out ret);
        }

        /// <summary>
        /// Determines whether the specified element is in this <see cref="ConcurrentSet{T}"/>. 
        /// </summary>
        /// <param name="val">The value of the element to check for. </param>
        /// <returns>true if the element was inside the set; false otherwise. </returns>
        public bool Contains(T val)
        {
            return dict.ContainsKey(val);
        }

        /// <summary>
        /// Removes all values from this ConcurrentSet. 
        /// </summary>
        public void Clear()
        {
            dict.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dict.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.Values.GetEnumerator();
        }
    }
}
