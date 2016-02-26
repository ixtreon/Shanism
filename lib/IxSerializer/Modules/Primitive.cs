using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IxSerializer.Modules
{

    class IntSerializer : SerializerModule
    {
        public override bool CanSerialize(Type ty) => ty == typeof(int);

        public override object Deserialize(BinaryReader buf, Type ty) => buf.ReadInt32();

        public override void Serialize(BinaryWriter buf, Type ty, object o) => buf.Write((int)o);
    }
    /// <summary>
    /// Reads and writes primitive types and byte/char arrays. 
    /// </summary>
    class PrimitiveSerializer : SerializerModule
    {
        //defines how primitive types are written
        static readonly Dictionary<Type, Action<BinaryWriter, object>> serializeDict = new Dictionary<Type, Action<BinaryWriter, object>>
            {
                { typeof(bool), (buf, o) => buf.Write((bool)o) },
                { typeof(byte), (buf, o) => buf.Write((byte)o) },
                { typeof(char), (buf, o) => buf.Write((char)o) },
                { typeof(short), (buf, o) => buf.Write((short)o) },
                { typeof(int), (buf, o) => buf.Write((int)o) },
                { typeof(uint), (buf, o) => buf.Write((uint)o) },
                { typeof(long), (buf, o) => buf.Write((long)o) },
                { typeof(float), (buf, o) => buf.Write((float)o) },
                { typeof(double), (buf, o) => buf.Write((double)o) },
                { typeof(string), (buf, o) => buf.Write((string)o ?? string.Empty) },
                { typeof(byte[]), (buf, o) =>
                {
                    var arr = (byte[])o;
                    buf.Write(arr.Length);
                    buf.Write(arr);
                }},
                { typeof(char[]), (buf, o) =>
                {
                    var arr = (char[])o;
                    buf.Write(arr.Length);
                    buf.Write(arr);
                }},
            };

        //specifies how a dynamic primitive types are read
        static readonly Dictionary<Type, Func<BinaryReader, object>> deserializeDict = new Dictionary<Type, Func<BinaryReader, object>>
            {
                { typeof(long), (buf) => buf.ReadInt64() },
                { typeof(int), (buf) => buf.ReadInt32() },
                { typeof(uint), (buf) => buf.ReadUInt32() },
                { typeof(short), (buf) => buf.ReadInt16() },
                { typeof(char), (buf) => buf.ReadChar() },
                { typeof(bool), (buf) => buf.ReadBoolean() },
                { typeof(double), (buf) => buf.ReadDouble() },
                { typeof(float), (buf) => buf.ReadSingle() },
                { typeof(byte), (buf) => buf.ReadByte() },
                { typeof(string), (buf) => buf.ReadString() },
                { typeof(byte[]), (buf) =>
                {
                    var n = buf.ReadInt32();
                    return buf.ReadBytes(n);
                }},
                { typeof(char[]), (buf) =>
                {
                    var n = buf.ReadInt32();
                    return buf.ReadChars(n);
                }},
            };

        public override bool CanSerialize(Type ty)
        {
            return serializeDict.ContainsKey(ty);
        }

        public override void Serialize(BinaryWriter buf, Type ty, object o)
        {
            serializeDict[ty](buf, o);
        }

        public override object Deserialize(BinaryReader buf, Type ty)
        {
            return deserializeDict[ty](buf);
        }

        public override bool AreEqual(Type ty, object a, object b)
        {
            return a.Equals(b);
        }
    }
}
