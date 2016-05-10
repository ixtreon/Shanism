using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Game
{
    class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        readonly Func<T, T, bool> equalsFunc;

        readonly Func<T, int> hashCodeFunc;

        public GenericEqualityComparer(Func<T, T, bool> equalsFunc, Func<T, int> hashCodeFunc)
        {
            this.equalsFunc = equalsFunc;
            this.hashCodeFunc = hashCodeFunc;
        }


        public bool Equals(T x, T y)
        {
            return equalsFunc(x, y);
        }

        public int GetHashCode(T obj)
        {
            return hashCodeFunc(obj);
        }
    }
}
