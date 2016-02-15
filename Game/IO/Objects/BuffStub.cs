using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;

namespace IO.Objects
{
    /// <summary>
    /// Represents an empty buff as reconstructed by a network client. 
    /// </summary>
    public class BuffStub : IBuffInstance
    {
        public uint Id { get; set; }

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

        public double MaxDamage { get; private set; }

        public double MinDamage { get; private set; }

        public double MoveSpeed { get; private set; }

        public int MoveSpeedPercentage { get; private set; }

        public string Name { get; private set; }

        public double Strength { get; private set; }

        public BuffType Type { get; private set; }

        public bool HasIcon { get; private set; }

        public double Vitality { get; private set; }
    }
}
