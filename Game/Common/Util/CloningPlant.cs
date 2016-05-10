using Shanism.Common.Objects;
using Shanism.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Util
{
    /// <summary>
    /// Implements a (speeded-up-reflection)-based method of cloning objects. 
    /// </summary>
    public static class CloningPlant
    {
        static class Cache<T>
        {
            public static IReadOnlyDictionary<string, IPropertyCallAdapter<T>> Properties { get; }

            static Cache()

            {
                var ps = typeof(T).GetAllProperties()
                    .Select(p => (IPropertyCallAdapter<T>)PropertyCaller.GetInstance(p))
                    .ToList();
                Properties = ps.ToDictionary(p => p.PropertyName, p => p);
            }
        }

        public static void ShallowCopy<T>(T source, T target)
        {
            foreach (var p in Cache<T>.Properties.Values)
                if (p.CanRead && p.CanWrite)
                    p.InvokeSet(target, p.InvokeGet(source));
        }

        public static void UnsafeCopy<TSrc, TDest>(TSrc src, TDest target)
        {
            var srcProps = Cache<TSrc>.Properties;
            IPropertyCallAdapter<TSrc> srcProp;

            foreach (var destProp in Cache<TDest>.Properties.Values)
                if (destProp.CanWrite
                    && srcProps.TryGetValue(destProp.PropertyName, out srcProp)
                    && srcProp.CanRead)
                {
                    var obj = srcProp.InvokeGet(src);
                    destProp.InvokeSet(target, obj);
                }
        }
    }
}
