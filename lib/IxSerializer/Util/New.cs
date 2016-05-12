using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IxSerializer.Util
{
    public static class New
    {
        static Dictionary<Type, Func<object>> invokes = new Dictionary<Type, Func<object>>();
        public static object CreateInstance(Type ty)
        {
            Func<object> f;
            if (!invokes.TryGetValue(ty, out f))
            {
                var mi = typeof(New<>).MakeGenericType(ty)
                    .GetMethod("CreateInstance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                f = (Func<object>)Delegate.CreateDelegate(typeof(Func<object>), mi);

                invokes[ty] = f;
            }
            return f();
        }

    }

    public static class New<T> where T : new()
    {

        public static object CreateInstance() => new T();
    }
}
