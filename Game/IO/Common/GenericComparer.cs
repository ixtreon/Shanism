using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO.Common
{
    public class GenericComparer<T> : IComparer<T>
    {
        public readonly Func<T, T, int> CompareFunc;

        public GenericComparer(Func<T, T, int> compareFunc)
        {
            this.CompareFunc = compareFunc;
        }

        public int Compare(T x, T y)
        {
            return CompareFunc(x, y);
        }
    }
}
