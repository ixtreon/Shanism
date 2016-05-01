﻿using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Content;
using IO.Objects;
using ProtoBuf;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


namespace IO.Objects
{
    public class UnitStub : EntityStub, IUnit
    {

        #region IUnit implementation

        public IEnumerable<IBuffInstance> Buffs { get; set; }
        public IEnumerable<IAbility> Abilities { get; set; }



        public uint OwnerId { get; set; }
        public IEnumerable<uint> BuffIds { get; set; }
        public IEnumerable<uint> AbilityIds { get; set; }
        public uint CastingAbilityId { get; set; }



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

        public UnitFlags States { get; set; }

        public bool IsMoving { get; set; }

        public double MoveDirection { get; set; }

        public double VisionRange { get; set; }

        public int TotalCastingTime { get; set; }

        #endregion


        public UnitStub() : base(0) { }

        public UnitStub(uint guid)
            : base(guid)
        {
        }
    }
}