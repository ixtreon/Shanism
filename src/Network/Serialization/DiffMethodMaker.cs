using Lidgren.Network;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Shanism.Network.Serialization
{
    class DiffMethodMaker
    {

        public void Initialize()
        {
            var ans = CreateMap<Color>(
                (m, old, @new) => m.Write(@new.Pack()),
                (m, old) => Color.FromPacked(m.ReadInt32())
            );

            var ans2 = CreateMap<string>(
                (m, old, @new) => m.Write(@new),
                (m, old) => m.ReadString()
            );
        }

        object CreateMap<T>(
            Expression<Action<NetBuffer, T, T>> serialize,
            Expression<Func<NetBuffer,T, T>> deserialize
        )
        {

            {
                var m = serialize.Parameters[0];
                var a = serialize.Parameters[1];
                var b = serialize.Parameters[2];
                var writeBool = GetMI<NetBuffer>(x => x.Write(true));
                var hasChanged = Expression.Parameter(typeof(bool));

                serialize = Expression.Lambda<Action<NetBuffer, T, T>>(
                    Expression.Block(
                        new[] { hasChanged },
                        Expression.Assign(hasChanged, Expression.NotEqual(a, b)),
                        Expression.Call(m, writeBool, hasChanged),
                        Expression.IfThen(hasChanged,
                            serialize.Body
                        )
                    ),
                    new[] { m, a, b }
                );
            }

            {
                var m = deserialize.Parameters[0];
                var a = deserialize.Parameters[1];
                var readBool = GetMI<NetBuffer>(x => x.ReadBoolean());
                var ret = Expression.Label();
                deserialize = Expression.Lambda<Func<NetBuffer, T, T>>(
                    Expression.Block(
                        Expression.IfThen(Expression.Negate(Expression.Call(m, readBool)),
                            Expression.Return(ret, a)
                        ),
                        Expression.Return(ret, deserialize.Body),
                        Expression.Label(ret)
                    ),
                    deserialize.Parameters
                );
            }


            throw new NotImplementedException();
        }

        MethodInfo GetMI<T>(Expression<Action<T>> e) => (e.Body as MethodCallExpression).Method;
    }
}
