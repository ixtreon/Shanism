using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace IxSerializer.Modules
{
    /// <summary>
    /// Serializes objects of type <see cref="IEnumerable{T}"/>. 
    /// </summary>
    class EnumerableSerializer : SerializerModule
    {
        public override bool CanSerialize(Type ty)
        {
            return ty.IsGenericType 
                && ty.GetGenericTypeDefinition() == typeof(IEnumerable<>)  
                && Serializer.CanParse(ty.GetGenericArguments().First());
        }

        public override object Deserialize(BinaryReader buf, Type ty)
        {
            var elemTy = ty.GetGenericArguments().First();

            var nItems = buf.ReadInt32();
            var arr = Array.CreateInstance(elemTy, nItems);
            object obj = null;
            for(int i = 0; i < nItems; i++)
            {
                if (Serializer.TryRead(buf, elemTy, out obj))
                    arr.SetValue(obj, i);
                else
                    throw new Exception($"Unable to read element #{i} of an enumerable of type `{ty.Name}`. ");
            }

            return arr;
        }

        public override void Serialize(BinaryWriter buf, Type ty, object obj)
        {
            var elemTy = ty.GetGenericArguments().First();
            var vals = (IEnumerable)obj;

            var count = 0;
            foreach (var o in vals) count++;

            buf.Write(count);

            foreach(var o in vals)
                Serializer.TryWrite(buf, elemTy, o);
        }

        public override bool AreEqual(Type ty, object a, object b)
        {
            return a.Equals(b);
        }
    }
}
