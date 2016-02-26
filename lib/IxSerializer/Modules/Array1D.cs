using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Diagnostics.Contracts;

namespace IxSerializer.Modules
{
    /// <summary>
    /// Serializes one-dimensional arrays. 
    /// </summary>
    class Array1DSerializer : SerializerModule
    {
        public override bool CanSerialize(Type ty)
        {
            return ty.IsArray && ty.GetArrayRank() == 1 && Serializer.CanParse(ty.GetElementType());
        }

        public override object Deserialize(BinaryReader buf, Type ty)
        {
            var elemTy = ty.GetElementType();

            var nItems = buf.ReadInt32();
            var arr = Array.CreateInstance(elemTy, nItems);

            object elem = null;
            for (int i = 0; i < nItems; i++)
            {
                if (Serializer.TryRead(buf, elemTy, out elem))
                    arr.SetValue(elem, i);
                else
                    throw new Exception($"Unable to read element #{i} from array type `{ty.Name}`");
            }

            return arr;
        }

        public override void Serialize(BinaryWriter buf, Type ty, object obj)
        {
            var elemTy = ty.GetElementType();
            var e = (Array)obj;

            buf.Write(e.Length);
            foreach (var o in e)
                Serializer.TryWrite(buf, elemTy, o);
        }

    }
}
