using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO
{
    public interface IUnit : IGameObject
    {
        bool IsDead { get; }

        double Life { get; }
        double MaxLife { get; }

        double Mana { get; }
        double MaxMana { get; }

        double MoveSpeed { get; }
        double Defense { get; }
        double MinDamage { get; }
        double MaxDamage { get; }
    }
}
