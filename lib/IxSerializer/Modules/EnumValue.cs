using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IxSerializer.Modules
{
    /// <summary>
    /// Reads and writes enum-type values. 
    /// </summary>
    class EnumValueSerializer : SerializerModule
    {
        static readonly Dictionary<Type, Type> enumTypes = new Dictionary<Type, Type>();

        public override bool AreEqual(Type ty, object a, object b)
        {
            return a.Equals(b);
        }

        public override bool CanSerialize(Type ty)
        {
            return ty.IsEnum;
        }

        public override object Deserialize(BinaryReader buf, Type ty)
        {
            var enumTy = ty.GetEnumUnderlyingType();
            object obj = null;
            if(!Serializer.TryRead(buf, enumTy, out obj))
                throw new Exception($"Unable to parse the enum type `{ty}` as type `{enumTy}`. ");

            return Enum.ToObject(ty, obj);
        }

        public override void Serialize(BinaryWriter buf, Type ty, object o)
        {
            var enumTy = ty.GetEnumUnderlyingType();
            if (!Serializer.TryWrite(buf, enumTy, o))
                throw new Exception($"Could not serialize object `{o}` as a `{ty}` enum. ");
        }
    }
}
