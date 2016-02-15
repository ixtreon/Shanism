using IO;
using IO.Objects;
using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IO.Serialization
{
    /// <summary>
    /// Provides methods for mapping types to underlying interfaces.  
    /// Also provides serialization of objects of mapped types to a stream corresponding to the mapped interface. 
    /// The stream can be then decoded into any other mapped type that inherits the same interface. 
    /// <para>Used to convert convert GameObjects (and the descending types) into streams, and then into Stub objects. </para>
    /// </summary>
    public class ProtoConverter
    {
        public static readonly ProtoConverter Default = new ProtoConverter();

        //maps interface properties to propertyinfos
        readonly Dictionary<Type, List<PropertyInfo>> _interfaceProperties = new Dictionary<Type, List<PropertyInfo>>();

        //maps concrete types to interfaces
        readonly Dictionary<Type, Type> _classMappings = new Dictionary<Type, Type>();



        public void AddMappingFromTo<TInt, TConcrete>()
            where TConcrete : TInt
        {
            if (!_interfaceProperties.ContainsKey(typeof(TInt)))
            {
                var props = GetPublicProperties(typeof(TInt))
                    .OrderBy(p => p.Name)
                    .Select(p => typeof(TConcrete).GetProperty(p.Name))
                    .ToList();

                _interfaceProperties.Add(typeof(TInt), props);
            }

            _classMappings.Add(typeof(TConcrete), typeof(TInt));
        }

        Type getMappedInterface(Type objTy)
        {
            Type mappedInterface;
            if (_classMappings.TryGetValue(objTy, out mappedInterface))
                return mappedInterface;

            var curTy = objTy;

            do
            {
                curTy = curTy.BaseType;
            }
            while (curTy != null && (mappedInterface = _classMappings.TryGet(curTy)) == null);

            if(mappedInterface != null)
                _classMappings[objTy] = mappedInterface;

            return mappedInterface;
        }

        public void Serialize(MemoryStream ms, object obj)
        {
            //get mapped interface
            // walk the inheritance chain if needed to find the mapping
            var objTy = obj.GetType();
            var ty = obj.GetType();
            Type mappedInterface;
            while ((mappedInterface = _classMappings.TryGet(ty)) == null && ty.BaseType != null)
                ty = ty.BaseType;

            if (mappedInterface == null)
                throw new Exception("Unable to serialize the object of type `{0}` as the type is not mapped!".F(ty.FullName));

            var mappedProperties = _interfaceProperties.TryGet(mappedInterface);
            if (mappedProperties == null)
                throw new Exception("No properties found for interface `{1}` to which the provided type `{1}` maps. ".F(mappedInterface.FullName, ty.FullName));


            foreach (var prop in mappedProperties)
            {
                var propAdapter = PropertyCaller.GetInstance(ty, prop.Name);
                var propData = propAdapter.InvokeGet(obj);

                RuntimeTypeModel.Default.SerializeWithLengthPrefix(ms, propData, prop.PropertyType, PrefixStyle.Base128, 0);
            }
        }

        public void Deserialize(MemoryStream ms, object obj)
        {
            var ty = obj.GetType();

            var mappedInterface = _classMappings.TryGet(ty);
            if (mappedInterface == null)
                throw new Exception("Unable to serialize the object of type `{0}` as the type is not mapped!".F(ty.FullName));

            var mappedProperties = _interfaceProperties.TryGet(mappedInterface);
            if (mappedProperties == null)
                throw new Exception("No properties found for interface `{1}` to which the provided type `{1}` maps. ".F(mappedInterface.FullName, ty.FullName));

            foreach (var prop in mappedProperties)
            {
                var propAdapter = PropertyCaller.GetInstance(ty, prop.Name);
                var val = RuntimeTypeModel.Default.DeserializeWithLengthPrefix(ms, null, prop.PropertyType, PrefixStyle.Base128, 0);

                propAdapter.InvokeSet(obj, val);
            }
        }
        static IEnumerable<PropertyInfo> GetPublicProperties(Type type)
        {
            if (!type.IsInterface)
                return type.GetProperties();

            return (new Type[] { type })
                   .Concat(type.GetInterfaces())
                   .SelectMany(i => i.GetProperties());
        }

    }
}
