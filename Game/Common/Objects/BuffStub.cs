using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
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


        public double Agility { get; set; }

        public int AttackSpeedPercentage { get; set; }

        public double Defense { get; set; }

        public string Description { get; set; }

        public double Dodge { get; set; }
        public int DurationLeft { get; set; }

        public int FullDuration { get; set; }

        public string Icon { get; set; }

        public double Intellect { get; set; }

        public double MaxLife { get; set; }

        public double MaxMana { get; set; }

        public double LifeRegen { get; set; }

        public double ManaRegen { get; set; }

        public double MaxDamage { get; set; }

        public double MinDamage { get; set; }

        public double MoveSpeed { get; set; }

        public int MoveSpeedPercentage { get; set; }

        public string Name { get; set; }

        public double Strength { get; set; }

        public BuffStackType StackType { get; set; }

        public bool HasIcon { get; set; }

        public double Vitality { get; set; }

        public StateFlags UnitStates { get; set; }
    }
}
