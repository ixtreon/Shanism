using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Objects
{
    /// <summary>
    /// Represents an empty hero instance as reconstructed by a network client. 
    /// </summary>
    class HeroStub : UnitStub, IHero
    {
        public double Agility { get; private set; }

        public double BaseAgility { get; private set; }

        public double BaseIntellect { get; private set; }

        public double BaseStrength { get; private set; }

        public double BaseVitality { get; private set; }

        public int Experience { get; private set; }

        public int ExperienceNeeded { get; private set; }

        public double Intellect { get; private set; }

        public double Strength { get; private set; }

        public double Vitality { get; private set; }

        public HeroStub() : base(-1) { }

        public HeroStub(int guid)
            : base(guid)
        {

        }
    }
}
