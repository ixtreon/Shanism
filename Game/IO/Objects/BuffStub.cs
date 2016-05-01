using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace IO.Objects
{
    /// <summary>
    /// Represents an empty buff as reconstructed by a network client. 
    /// </summary>
    public class BuffInstanceStub : ObjectStub, IBuffInstance
    {

        public BuffInstanceStub() { }

        public BuffInstanceStub(uint id)
            : base(id)
        {

        }


        public double Agility { get; private set; }

        public int AttackSpeedPercentage { get; private set; }

        public double Defense { get; private set; }

        public string Description { get; private set; }

        public double Dodge { get; private set; }
        public int DurationLeft { get; private set; }

        public int FullDuration { get; private set; }

        public string Icon { get; private set; }

        public double Intellect { get; private set; }

        public double Life { get; private set; }

        public double Mana { get; private set; }

        public double LifeRegen { get; private set; }

        public double ManaRegen { get; private set; }

        public double MaxDamage { get; private set; }

        public double MinDamage { get; private set; }

        public double MoveSpeed { get; private set; }

        public int MoveSpeedPercentage { get; private set; }

        public string Name { get; private set; }

        public double Strength { get; private set; }

        public BuffType Type { get; private set; }

        public bool HasIcon { get; private set; }

        public double Vitality { get; private set; }

        public UnitFlags UnitStates { get; private set; }
    }
}
