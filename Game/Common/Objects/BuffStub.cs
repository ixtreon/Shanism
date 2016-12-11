using System;
using Shanism.Common.Game;
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


        public bool HasIcon { get; set; }
        public string Icon { get; set; }
        public Color IconTint { get; set; }

        public IHeroAttributes Attributes { get; } = new HeroAttributes();
        public IUnitStats Stats { get; } = new UnitStats();
        public StateFlags StateFlags { get; set; }

        public int AttackSpeedPercentage { get; set; }
        public int MoveSpeedPercentage { get; set; }

        public override void ReadDiff(FieldReader r)
        {
            FullDuration = r.ReadInt(FullDuration);
        }

        public override void WriteDiff(FieldWriter w, IGameObject newObject)
        {
            var b = (IBuff)newObject;

            w.WriteInt(FullDuration, b.FullDuration);
        }
    }
}