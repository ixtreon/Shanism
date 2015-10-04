using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO
{
    public interface IHero : IUnit
    {
        double Strength { get; }
        double Agility { get; }
        double Vitality { get; }
        double Intellect { get; }

        double BaseStrength { get; }
        double BaseAgility { get; }
        double BaseVitality { get; }
        double BaseIntellect { get; }

        int Experience { get; }
        int ExperienceNeeded { get; }

    }
}
