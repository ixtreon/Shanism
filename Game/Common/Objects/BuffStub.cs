using System;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Serialization;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class BuffStub : ObjectStub, IBuff
    {
        public static readonly BuffStub Default = new BuffStub();

        public int FullDuration { get; set; }
        public BuffStackType StackType { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }


        public string Icon { get; set; }
        public Color IconTint { get; set; }

        public IHeroAttributes Attributes { get; } = new HeroAttributes();
        public IUnitStats Stats { get; } = new UnitStats();
        public StateFlags StateFlags { get; set; }

        public int AttackSpeedPercentage { get; set; }
        public int MoveSpeedPercentage { get; set; }
        public bool HasIcon { get; set; }

        public override void ReadDiff(IReader r)
        {
            FullDuration = r.ReadVarInt(FullDuration);

            StackType = (BuffStackType)r.ReadByte((byte)StackType);
            Name = r.ReadString(Name);
            Description = r.ReadString(Description);

            Icon = r.ReadString(Icon);
            IconTint = r.ReadColor(IconTint);

            Attributes.Read(r);
            Stats.Read(r);

            StateFlags = (StateFlags)r.ReadVarInt((int)StateFlags);
            AttackSpeedPercentage = r.ReadVarInt(AttackSpeedPercentage);
            MoveSpeedPercentage = r.ReadVarInt(MoveSpeedPercentage);

            HasIcon = r.ReadBool(HasIcon);
            r.ReadPadBits();
        }

        //public (\w+) (\w+) .*\r\n
        //w.Write$1($2, b.$2);

        public override void WriteDiff(IWriter w, IGameObject newObject)
        {
            var b = (IBuff)newObject;

            w.WriteVarInt(FullDuration, b.FullDuration);
            w.WriteByte((byte)StackType, (byte)b.StackType);
            w.WriteString(Name, b.Name);
            w.WriteString(Description, b.Description);

            w.WriteString(Icon, b.Icon);
            w.WriteColor(IconTint, b.IconTint);

            b.Attributes.Write(w, Attributes);
            b.Stats.Write(w, Stats);

            w.WriteVarInt((int)StateFlags, (int)b.StateFlags);
            w.WriteVarInt(AttackSpeedPercentage, b.AttackSpeedPercentage);
            w.WriteVarInt(MoveSpeedPercentage, b.MoveSpeedPercentage);

            w.WriteBool(HasIcon, b.HasIcon);
            w.WritePadBits();
        }
    }
}