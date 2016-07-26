using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    abstract class ObjectSerializer
    {
        public virtual void Write(BinaryWriter w, IGameObject obj)
        {
            w.Write(obj.Id);
            w.Write((byte)obj.ObjectType);
        }

        public abstract ObjectStub Create(uint id);

        public virtual void Read(BinaryReader r, IGameObject obj) { }

        public static ObjectHeader ReadHeader(BinaryReader r)
        {
            var id = r.ReadUInt32();
            var ty = (ObjectType)r.ReadByte();
            return new ObjectHeader
            {
                Id = id, 
                Type = ty,
            };
        }
    }

    public struct ObjectHeader
    {
        public uint Id;
        public ObjectType Type;
    }
}
