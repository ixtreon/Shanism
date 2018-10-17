using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Entities;
using Shanism.Common.Objects;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace Shanism.Common.ObjectStubs
{
    public class UnitStub : EntityStub, IUnit
    {
        public override ObjectType ObjectType => ObjectType.Unit;


        #region IUnit implementation

        public readonly List<IBuffInstance> Buffs = new List<IBuffInstance>();
        public readonly List<IAbility> Abilities = new List<IAbility>();

        readonly UnitStats currentStats = new UnitStats(0);
        readonly UnitStats baseStats = new UnitStats(0);

        IReadOnlyCollection<IBuffInstance> IUnit.Buffs => Buffs;

        IReadOnlyCollection<IAbility> IUnit.Abilities => Abilities;

        public IUnitStats Stats { get; set; } = new UnitStats(0);
        public IUnitStats BaseStats { get; set; } = new UnitStats(0);

        public uint OwnerId { get; set; }


        public bool IsDead { get; set; }

        public int Level { get; set; }

        public float Life { get; set; }

        public float Mana { get; set; }


        public MovementState MovementState { get; set; } = MovementState.Stand;

        public StateFlags StateFlags { get; set; }


        public float VisionRange { get; set; }

        public uint CastingAbilityId { get; set; }
        public int CastingProgress { get; set; }
        public int TotalCastingTime { get; set; }

        #endregion


        public UnitStub() : base(0) { }

        public UnitStub(uint guid)
            : base(guid)
        {
        }
    }
}
