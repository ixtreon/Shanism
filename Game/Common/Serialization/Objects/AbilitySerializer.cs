using System.IO;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using Shanism.Common.Game;

namespace Shanism.Common.Serialization
{
    class AbilitySerializer : SerializerBase
    {
        public override ObjectStub Create(uint id) => new AbilityStub(id);

        public override void Write(BinaryWriter w, IGameObject obj)
        {
            var a = (IAbility)obj;

            w.Write(a.Id);

            w.Write(a.Name);
            w.Write(a.Description);
            w.Write(a.Icon);

            w.Write(a.CurrentCooldown);
            w.Write(a.Cooldown);
            w.Write(a.ManaCost);
            w.Write(a.CastTime);

            w.Write((float)a.CastRange);
            w.Write((byte)a.TargetType);
        }

        public override void Read(BinaryReader r, IGameObject obj)
        {
            var a = (AbilityStub)obj;

            a.Id = r.ReadUInt32();

            a.Name = r.ReadString();
            a.Description = r.ReadString();
            a.Icon = r.ReadString();

            a.CurrentCooldown = r.ReadInt32();
            a.Cooldown = r.ReadInt32();
            a.ManaCost = r.ReadInt32();
            a.CastTime = r.ReadInt32();

            a.CastRange = r.ReadSingle();
            a.TargetType = (AbilityTargetType)r.ReadByte();
        }
    }
}