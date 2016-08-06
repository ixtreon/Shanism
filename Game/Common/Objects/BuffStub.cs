using System;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class BuffStub : ObjectStub, IBuff
    {
        public int FullDuration { get; set; }
        public BuffStackType StackType { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }


        public bool HasIcon { get; set; }
        public string Icon { get; set; }

        public IHeroAttributes Attributes { get; set; }
        public IUnitStats Stats { get; set; }
        public StateFlags StateFlags { get; set; }

        public int AttackSpeedPercentage { get; set; }
        public int MoveSpeedPercentage { get; set; }
    }
}