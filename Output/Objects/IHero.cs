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

        IEnumerable<IAbility> Abilities { get; }
    }
}
