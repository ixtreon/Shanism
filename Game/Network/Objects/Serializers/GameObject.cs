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

namespace Network.Objects.Serializers
{
    /// <summary>
    /// Serializes GameObjects to their corresponding <see cref="IGameObject.Guid"/>. 
    /// 
    /// Tightly coupled with the static <see cref="ObjectFactory"/> whici it uses to resolve GUIDs to objects. 
    /// </summary>
    class GameObjectSerializer : ISerializerModule
    {
        static readonly PropertyInfo guidProperty = typeof(IGameObject).GetProperty(nameof(IGameObject.Guid));

        public bool CanSerialize(Type ty)
        {
            return typeof(IGameObject).IsAssignableFrom(ty);
        }

        public void Serialize(NetBuffer buf, Type ty, object obj)
        {
            if (!CanSerialize(ty))
                throw new Exception("Cannot serialize type {0}".Format(ty));

            var go = (IGameObject)obj;
            var goType = go.GetObjectType();

            buf.Write(goType);
            buf.Write(go.Guid);
        }

        public object Deserialize(NetBuffer buf, Type ty)
        {
            var goType = buf.ReadByte();
            var guid = buf.ReadInt32();

            return ObjectFactory.GetOrCreate(goType, guid);
        }
    }
}
