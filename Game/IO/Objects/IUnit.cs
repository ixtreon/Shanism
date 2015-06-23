using IO.Common;
using IO.Objects;
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
        double LifeRegen { get; }
        double ManaRegen { get; }
        double MagicDamage { get; }

        IEnumerable<IBuffInstance> Buffs { get; }

        double BaseDefense { get; }

        int AttackCooldown { get; }
        bool Invulnerable { get; }

        OrderType OrderType { get; }

        UnitState StateFlags { get; }

        int CastingProgress { get; }

        IAbility CastingAbility { get; }

        int Level { get; }
    }
}
