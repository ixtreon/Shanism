using System.IO;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;

namespace Shanism.Common.Serialization
{
    class BuffInstanceSerializer : SerializerBase
    {
        public override ObjectStub Create(uint id) => new BuffInstanceStub(id);

        public override void Write(BinaryWriter w, IGameObject obj)
        {
            var b = (IBuffInstance)obj;

            w.Write(b.DurationLeft);
            writeBuff(w, b.Prototype);
        }

        void writeBuff(BinaryWriter w, IBuff b)
        {
            w.Write(b.FullDuration);
            w.Write((short)b.StateFlags);
            w.Write((byte)b.StackType);

            w.Write(b.HasIcon);
            if (b.HasIcon)
            {
                w.Write(b.Icon);
                w.Write(b.IconTint.Pack());
            }

            w.Write(b.Name);
            w.Write(b.Description);

            b.Stats.Write(w);
            b.Attributes.Write(w);
            w.Write(b.MoveSpeedPercentage);
            w.Write(b.AttackSpeedPercentage);
        }

        public override void Read(BinaryReader r, IGameObject obj)
        {
            var b = (BuffInstanceStub)obj;

            b.DurationLeft = r.ReadInt32();
            readBuff(r, b.Prototype);
        }

        void readBuff(BinaryReader r, BuffStub b)
        {
            b.FullDuration = r.ReadInt32();
            b.StateFlags = (StateFlags)r.ReadInt16();
            b.StackType = (BuffStackType)r.ReadByte();

            b.HasIcon = r.ReadBoolean();
            if (b.HasIcon)
            {
                b.Icon = r.ReadString();
                b.IconTint = new Color(r.ReadInt32());
            }

            b.Name = r.ReadString();
            b.Description = r.ReadString();

            b.Stats.Read(r);
            b.Attributes.Read(r);
            b.MoveSpeedPercentage = r.ReadInt32();
            b.AttackSpeedPercentage = r.ReadInt32();
        }
    }
}