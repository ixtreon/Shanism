using System;
using Shanism.Common;
using Shanism.Common.Objects;

namespace Shanism.Common.ObjectStubs
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

        public IHeroAttributes Attributes { get; } = new HeroAttributes(0);
        public IUnitStats Stats { get; } = new UnitStats(0);
        public StateFlags StateFlags { get; set; }

        public int AttackSpeedPercentage { get; set; }
        public int MoveSpeedPercentage { get; set; }
        public bool HasIcon { get; set; }
    }
}