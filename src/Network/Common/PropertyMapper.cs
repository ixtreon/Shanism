using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Objects;
using Shanism.Common.ObjectStubs;
using Shanism.Network.Serialization;

namespace Shanism.Network.Common
{
    public class PropertyMapper<TStub, TInt>
        where TStub : ObjectStub
        where TInt : IGameObject
    {
        struct PropertyTuple
        {
            public PropertyInfo StubProperty;
            public PropertyInfo InterfaceProperty;
            public Type UnderlyingType;
        }

        static readonly DiffMethodCache miCache = new DiffMethodCache();

        static IEnumerable<PropertyTuple> GetProperties()
        {
            var skippedProperties = new List<PropertyInfo>();

            var types = new List<Type> { typeof(TInt) };
            if (typeof(TInt).GetTypeInfo().IsInterface)
                types.AddRange(typeof(TInt).GetInterfaces());

            foreach (var ty in types)
                foreach (var pInt in ty.GetProperties())
                {
                    var pStub = typeof(TStub).GetProperty(pInt.Name);

                    if (!canUseProperty(pStub, out Type propType))
                    {
                        skippedProperties.Add(pInt);
                        continue;
                    }

                    yield return new PropertyTuple
                    {
                        StubProperty = pStub,
                        InterfaceProperty = pInt,
                        UnderlyingType = propType,
                    };
                }

            if (skippedProperties.Any())
                Debug.WriteLine($"Excluded properties: {string.Join(", ", skippedProperties.Select(p => p.Name))}");

        }

        static bool canUseProperty(PropertyInfo prop, out Type readAsType)
        {
            if (prop?.SetMethod == null)
            {
                readAsType = null;
                return false;
            }

            var propType = prop.PropertyType;
            var propTypeInfo = propType.GetTypeInfo();
            readAsType = propTypeInfo.IsEnum
                    ? propTypeInfo.GetEnumUnderlyingType()
                    : propType;

            return miCache.Reads.ContainsKey(readAsType)
                && miCache.Writes.ContainsKey(readAsType);
        }


        static Action<ObjectStub, FieldReader> CreateReader()
        {
            Debug.WriteLine($"Creating reader for {typeof(TStub)}");

            var obj = Expression.Variable(typeof(ObjectStub));
            var typedObj = Expression.Convert(obj, typeof(TStub));
            var rdr = Expression.Variable(typeof(FieldReader));

            var methodBody = new List<Expression>();
            foreach (var pair in GetProperties())
            {
                var propType = pair.StubProperty.PropertyType;
                var propTypeInfo = propType.GetTypeInfo();
                var readMethod = miCache.Reads[pair.UnderlyingType];

                // obj.Prop
                var accessObjProp = Expression.MakeMemberAccess(typedObj, pair.StubProperty);

                // (T)obj.Prop
                var accessReadableObjProp = propTypeInfo.IsEnum
                    ? (Expression)Expression.Convert(accessObjProp, pair.UnderlyingType)
                    : accessObjProp;

                //(T)rdr.Read((T)obj.Prop)
                Expression callMethod = Expression.Call(rdr, readMethod, accessReadableObjProp);
                if (propTypeInfo.IsEnum)
                    callMethod = Expression.Convert(callMethod, propType);

                //obj.Prop = (T)rdr.Read((T)obj.Prop)
                var assignUpdatedVal = Expression.Assign(accessObjProp, callMethod);


                methodBody.Add(assignUpdatedVal);
            }

            if (!methodBody.Any())
                return (_, __) => { };

            var ff = Expression.Lambda<Action<ObjectStub, FieldReader>>(
                Expression.Block(methodBody), obj, rdr);

            return ff.Compile();
        }

        static Action<ObjectStub, IGameObject, FieldWriter> CreateWriter()
        {
            Debug.WriteLine($"Creating writer for {typeof(TStub)} + {typeof(TInt)}");

            var fromObj = Expression.Variable(typeof(ObjectStub), "from");
            var toObj = Expression.Variable(typeof(IGameObject), "to");
            var from = Expression.Convert(fromObj, typeof(TStub));
            var to = Expression.Convert(toObj, typeof(TInt));
            var writer = Expression.Variable(typeof(FieldWriter), "wr");

            var methodBody = new List<Expression>();
            foreach (var pair in GetProperties())
            {
                var propType = pair.StubProperty.PropertyType;
                var writeMethod = miCache.Writes[pair.UnderlyingType];

                Expression accessPropFrom = Expression.MakeMemberAccess(from, pair.StubProperty);
                Expression accessPropTo = Expression.MakeMemberAccess(to, pair.InterfaceProperty);

                // apply cast from enum to underlying type
                if (propType.GetTypeInfo().IsEnum)
                {
                    accessPropFrom = Expression.Convert(accessPropFrom, pair.UnderlyingType);
                    accessPropTo = Expression.Convert(accessPropTo, pair.UnderlyingType);
                }

                var writeCall = Expression.Call(writer, writeMethod,
                    accessPropFrom,
                    accessPropTo);
                methodBody.Add(writeCall);

            }

            if (!methodBody.Any())
                return (_, __, ___) => { };

            var ff = Expression.Lambda<Action<ObjectStub, IGameObject, FieldWriter>>(
                Expression.Block(methodBody),
                fromObj, toObj, writer);

            return ff.Compile();
        }


        public static Action<ObjectStub, FieldReader> Reader { get; } = CreateReader();
        public static Action<ObjectStub, IGameObject, FieldWriter> Writer { get; } = CreateWriter();
    }

}
