using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using IO;

namespace Network.Objects.Serializers
{
    class EnumSerializer : ISerializerModule
    {
        public bool CanSerialize(Type ty)
        {
            return ty.IsEnum;
        }

        public object Deserialize(NetBuffer buf, Type ty)
        {
            var enumTy = ty.GetEnumUnderlyingType();
            object obj = null;
            if(!Serializer.TryRead(buf, enumTy, ref obj))
                throw new Exception("Mnogo shano enumeration brat! Kvo e tva {0}?".Format(enumTy));

            return obj;
        }

        public void Serialize(NetBuffer buf, Type ty, object o)
        {
            var enumTy = ty.GetEnumUnderlyingType();
            if (!Serializer.TryWrite(buf, enumTy, o))
                throw new Exception("Mnogo shano enumeration brat! Kvo e tva {0}?".Format(enumTy));
        }
    }
}
