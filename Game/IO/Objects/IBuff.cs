using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    /// <summary>
    /// The static data of a buff. 
    /// </summary>
    public interface IBuff
    {
        /// <summary>
        /// Gets the icon of the buff. 
        /// </summary>
        string Icon { get; }


        bool Visible { get; }
        string Name { get; }
        string Description { get; }


        /// <summary>
        /// Gets the type of this buff. 
        /// </summary>
        BuffType Type { get; }

        double Life { get; }
        double Mana { get; }
        double Defense { get; }
        double Dodge { get; }
        double MoveSpeed { get; }
        int MoveSpeedPercentage { get; }
        int AttackSpeed { get; }
        double MinDamage { get; }
        double MaxDamage { get; }

        double Strength { get; }
        double Vitality { get; }
        double Agility { get; }
        double Intellect { get; }

        int FullDuration { get; }

    }
}
