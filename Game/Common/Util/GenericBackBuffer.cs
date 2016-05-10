using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Represents a pair of objects one of which is primary (front) and the other secondary (back). 
    /// Allows retrieving and swapping the said buffers.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericBuffer<T>
        where T : new()
    {

        T temp;

        /// <summary>
        /// Gets the primary (front) value. 
        /// </summary>
        public T Front { get; private set; }

        /// <summary>
        /// Gets the secondary (back) value. 
        /// </summary>
        public T Back { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBuffer{T}"/> class.
        /// </summary>
        public GenericBuffer()
            : this(new T(), new T())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBuffer{T}"/> class.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        public GenericBuffer(T a, T b)
        {
            Front = a;
            Back = b;
        }

        /// <summary>
        /// Swaps the positions of the two values.
        /// </summary>
        public virtual void SwapBuffers()
        {
            temp = Front;
            Front = Back;
            Back = temp;
        }
    }
}
