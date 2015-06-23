using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Collections;
using IO;

namespace Network.Objects.Serializers
{
    /// <summary>
    /// Serializes public properties of type IEnumerable<T> for any supported type T. 
    /// </summary>
    class EnumerableSerializer : ISerializerModule
    {
        public bool CanSerialize(Type ty)
        {
            var elemTy = ty.GetGenericArguments().First();
            return ty.GetGenericTypeDefinition() == typeof(IEnumerable<>) && Serializer.CanWrite(elemTy);
        }

        public object Deserialize(NetBuffer buf, Type ty)
        {
            var elemTy = ty.GetGenericArguments().First();

            var n = buf.ReadInt32();
            var arr = Array.CreateInstance(ty, n);
            object obj = null;
            for(int i = 0; i < n; i++)
            {
                if (Serializer.TryRead(buf, ty, ref obj))
                    arr.SetValue(obj, i);
                else
                    throw new Exception("Unable to read element {0} of type {1}".Format(i, ty.Name));
            }

            return arr;
        }

        public void Serialize(NetBuffer buf, Type ty, object obj)
        {
            var elemTy = ty.GetGenericArguments().First();
            var e = (IEnumerable)obj;

            buf.Write((int)e.Count());

            foreach(var o in (IEnumerable)obj)
                Serializer.TryWrite(buf, elemTy, o);

        }
    }
}
