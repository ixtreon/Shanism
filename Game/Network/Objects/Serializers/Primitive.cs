using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Network.Objects.Serializers
{
    class PrimitiveSerializer : ISerializerModule
    {
        //defines how a dynamic primitive type is written
        static Dictionary<Type, Action<NetBuffer, object>> serializeDict = new Dictionary<Type, Action<NetBuffer, object>>
            {
                { typeof(long), (buf, o) => buf.Write((long)o) },
                { typeof(int), (buf, o) => buf.Write((int)o) },
                { typeof(short), (buf, o) => buf.Write((short)o) },
                { typeof(char), (buf, o) => buf.Write(Encoding.UTF8.GetBytes(new[] { (char)o }).First()) },  //char is ushort, also short?
                { typeof(bool), (buf, o) => buf.Write((bool)o) },
                { typeof(double), (buf, o) => buf.Write((double)o) },
                { typeof(float), (buf, o) => buf.Write((float)o) },
                { typeof(byte), (buf, o) => buf.Write((byte)o) },
                { typeof(string), (buf, o) => buf.Write((string)o) },
            };

        //specifies how a dynamic primitive type is read
        static Dictionary<Type, Func<NetBuffer, object>> deserializeDict = new Dictionary<Type, Func<NetBuffer, object>>
            {
                { typeof(long), (buf) => buf.ReadInt64() },
                { typeof(int), (buf) => buf.ReadInt32() },
                { typeof(short), (buf) => buf.ReadInt16() },
                { typeof(char), (buf) => Encoding.UTF8.GetChars(new[] {  buf.ReadByte() }).First() },   //char is short
                { typeof(bool), (buf) => buf.ReadBoolean() },
                { typeof(double), (buf) => buf.ReadDouble() },
                { typeof(float), (buf) => buf.ReadSingle() },
                { typeof(byte), (buf) => buf.ReadByte() },
                { typeof(string), (buf) => buf.ReadString() },
            };

        public bool CanSerialize(Type ty)
        {
            return serializeDict.ContainsKey(ty);
        }

        public void Serialize(NetBuffer buf, Type ty, object o)
        {
            serializeDict[ty](buf, o);
        }

        public object Deserialize(NetBuffer buf, Type ty)
        {
            return deserializeDict[ty](buf);
        }
    }
}
