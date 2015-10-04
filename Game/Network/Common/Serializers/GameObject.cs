using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using IO;
using IO.Common;
using System.Reflection;
using System.IO;
using IxSerializer;

namespace Network.Objects.Serializers
{
    /// <summary>
    /// Dynamically serializes GameObjects to their corresponding <see cref="IGameObject.Guid"/>. 
    /// 
    /// Tightly coupled with the static <see cref="ObjectFactory"/> which it uses to resolve GUIDs to objects. 
    /// </summary>
    public class GameObjectSerializer : SerializerModule
    {
        static readonly PropertyInfo guidProperty = typeof(IGameObject).GetProperty(nameof(IGameObject.Guid));

        public readonly Func<object, int> GuidGetter;


        public override bool CanSerialize(Type ty)
        {
            return typeof(IGameObject).IsAssignableFrom(ty);
        }

        public override void Serialize(BinaryWriter buf, Type ty, object obj)
        {
            if (!CanSerialize(ty))
                throw new Exception("Cannot serialize type {0}".Format(ty));

            var go = (IGameObject)obj;
            buf.Write(go.ObjectType);
            buf.Write(go.Guid);
        }

        public override object Deserialize(BinaryReader buf, Type ty)
        {
            var goType = buf.ReadByte();
            var guid = buf.ReadInt32();

            return ObjectFactory.GetOrCreate(goType, guid);
        }

        public override bool AreEqual(Type ty, object a, object b)
        {
            return ((IGameObject)a).Guid == ((IGameObject)b).Guid;
        }
    }
}
