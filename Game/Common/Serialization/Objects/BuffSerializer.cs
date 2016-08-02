using System.IO;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using Shanism.Common.Game;

namespace Shanism.Common.Serialization
{
    class BuffInstanceSerializer : SerializerBase
    {
        public override ObjectStub Create(uint id) => new BuffInstanceStub(id);

        public override void Write(BinaryWriter w, IGameObject obj)
        {
            var b = (IBuffInstance)obj;

            w.Write(b.DurationLeft);

            w.Write(b.FullDuration);
            w.Write((short)b.UnitStates);
            w.Write((byte)b.StackType);
            w.Write(b.HasIcon);

            w.Write(b.Icon);
            w.Write(b.Name);
            w.Write(b.Description);


            w.Write((float)b.MaxLife);
            w.Write((float)b.MaxMana);
            w.Write((float)b.LifeRegen);
            w.Write((float)b.ManaRegen);
            w.Write((float)b.Defense);
            w.Write((float)b.Dodge);
            w.Write((float)b.MoveSpeed);

            w.Write(b.MoveSpeedPercentage);
            w.Write(b.AttackSpeedPercentage);

            w.Write((float)b.MinDamage);
            w.Write((float)b.MaxDamage);

            w.Write((float)b.Strength);
            w.Write((float)b.Vitality);
            w.Write((float)b.Agility);
            w.Write((float)b.Intellect);

        }

        public override void Read(BinaryReader r, IGameObject obj)
        {
            var b = (BuffInstanceStub)obj;

            b.DurationLeft = r.ReadInt32();

            b.FullDuration = r.ReadInt32();
            b.UnitStates = (StateFlags)r.ReadInt16();
            b.StackType = (BuffStackType)r.ReadByte();

            b.HasIcon = r.ReadBoolean();

            b.Icon = r.ReadString();
            b.Name = r.ReadString();
            b.Description = r.ReadString();

            b.MaxLife = r.ReadSingle();
            b.MaxMana = r.ReadSingle();
            b.LifeRegen = r.ReadSingle();
            b.ManaRegen = r.ReadSingle();
            b.Defense = r.ReadSingle();
            b.Dodge = r.ReadSingle();
            b.MoveSpeed = r.ReadSingle();

            b.MoveSpeedPercentage = r.ReadInt32();
            b.AttackSpeedPercentage = r.ReadInt32();

            b.MinDamage = r.ReadSingle();
            b.MaxDamage = r.ReadSingle();
            b.Strength = r.ReadSingle();
            b.Vitality = r.ReadSingle();
            b.Agility = r.ReadSingle();
            b.Intellect = r.ReadSingle();

        }
    }
}