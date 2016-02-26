using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer
{
    static class Ext
    {
        public static int[] GetDimensions(this Array arr)
        {
            return Enumerable.Range(0, arr.Rank)
                .Select(i => arr.GetLength(i))
                .ToArray();
        }

        public static IEnumerable<int[]> EnumIndices(this Array arr)
        {
            var rank = arr.Rank;
            var ranks = GetDimensions(arr);
            var cur = new int[rank];
            do
            {
                yield return cur;

                //get the dimension to increase
                var dim = 0;
                while (dim < rank && cur[dim] + 1 == ranks[dim]) { cur[dim] = 0; dim++; }
                if (dim == rank)
                    break;
                cur[dim]++;
            }
            while (true);
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type ty)
        {
            var props = new HashSet<string>();
            var types = new Queue<Type>();
            types.Enqueue(ty);
            do
            {
                var curTy = types.Dequeue();

                foreach (var prop in curTy.GetProperties())
                    if (props.Add(prop.Name))
                        yield return prop;

                foreach (var subTy in curTy.GetInterfaces())
                    types.Enqueue(subTy);
            }
            while (types.Any());
        }
    }
}
