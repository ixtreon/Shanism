using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Content;
using IO.Objects;

namespace IO.Objects
{
    public class UnitStub : ObjectStub, IUnit
    {
        public IEnumerable<IBuffInstance> Buffs { get; set; }

        public IEnumerable<IAbility> Abilities { get; set; }

        public IAbility CastingAbility { get; set; }


        public int AttackCooldown { get; set; }

        public double BaseDefense { get; set; }

        public int CastingProgress { get; set; }

        public double Defense { get; set; }

        public bool Invulnerable { get; set; }

        public bool IsDead { get; set; }

        public int Level { get; set; }

        public double Life { get; set; }

        public double LifeRegen { get; set; }

        public double MagicDamage { get; set; }

        public double Mana { get; set; }

        public double ManaRegen { get; set; }

        public double MaxDamage { get; set; }

        public double MaxLife { get; set; }

        public double MaxMana { get; set; }

        public double MinDamage { get; set; }

        public double MoveSpeed { get; set; }

        public OrderType OrderType { get; set; }

        public UnitFlags StateFlags { get; set; }

        public IPlayer Owner { get; set; }

        public bool IsMoving { get; set; }

        public double MoveDirection { get; set; }

        public double VisionRange { get; set; }

        public int TotalCastingTime { get; set; }

        public UnitStub() : base(0) {  }

        public UnitStub(uint guid)
            : base(guid)
        {
            Buffs = Enumerable.Empty<IBuffInstance>();
            Abilities = Enumerable.Empty<IAbility>();
        }
    }
}
