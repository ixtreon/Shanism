using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    public interface IBuffInstance
    {
        /// <summary>
        /// Gets the icon of the buff. 
        /// </summary>
        string Icon { get; }
        BuffType Type { get; }

        int MoveSpeed { get; }
        int AttackSpeed { get; }
        double Life { get; }
        double Mana { get; }
        double Defense { get; }
        double MinDamage { get; }
        double MaxDamage { get; }
        double Strength { get; }
        double Vitality { get; }
        double Agility { get; }
        double Intellect { get; }

        int DurationLeft { get; }
        int FullDuration { get; }

    }
}
