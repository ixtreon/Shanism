using System;
using Shanism.Common;
using Shanism.Common.Interfaces.Objects;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class BuffStub : ObjectStub, IBuff
    {
        public override ObjectType ObjectType => ObjectType.Buff;

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
    }
}