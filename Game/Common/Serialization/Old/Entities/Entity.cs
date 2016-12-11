using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Shanism.Common.StubObjects;

namespace Shanism.Common.Serialization
{
    class EntitySerializer : SerializerBase
    {
        public override void Write(BinaryWriter w, IGameObject obj)
        {
            var e = (IEntity)obj;

            w.Write(e.Name ?? string.Empty);
            w.Write((float)e.Position.X);
            w.Write((float)e.Position.Y);
            w.Write(e.Model ?? string.Empty);
            w.Write(e.Animation ?? string.Empty);
            w.Write(e.Orientation);
            w.Write(e.LoopAnimation);
            w.Write((float)e.Scale);
            w.Write(e.CurrentTint.Pack());
        }

        public override ObjectStub Create(uint id) => new EntityStub(id);

        public override void Read(BinaryReader r, IGameObject obj)
        {
            var e = (EntityStub)obj;

            e.Name = r.ReadString();
            var x = r.ReadSingle();
            var y = r.ReadSingle();
            e.Position = new Vector(x, y);
            e.Model = r.ReadString();
            e.Animation = r.ReadString();
            e.Orientation = r.ReadSingle();
            e.LoopAnimation = r.ReadBoolean();
            e.Scale = r.ReadSingle();
            e.CurrentTint = new Color(r.ReadInt32());
        }
    }
}
