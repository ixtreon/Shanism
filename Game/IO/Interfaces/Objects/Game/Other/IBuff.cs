using IO.Common;
using IO.Interfaces.Engine;
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
    public interface IBuff : IGameObject
    {

        /// <summary>
        /// Gets the icon of the buff. 
        /// </summary>
        string Icon { get; }


        /// <summary>
        /// Gets whether this buff has an icon 
        /// and is displayed in the default buff bar. 
        /// </summary>
        bool HasIcon { get; }

        /// <summary>
        /// Gets the name of the buff. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the formatted description of this buff. 
        /// </summary>
        string Description { get; }


        /// <summary>
        /// Gets the type of this buff. 
        /// </summary>
        BuffType Type { get; }

        /// <summary>
        /// Gets the life modifier of this buff. 
        /// </summary>
        double Life { get; }

        /// <summary>
        /// Gets the mana modifier of this buff. 
        /// </summary>
        double Mana { get; }

        /// <summary>
        /// Gets the life regen modifier of this buff. 
        /// </summary>
        double LifeRegen { get; }

        /// <summary>
        /// Gets the mana regen modifier of this buff. 
        /// </summary>
        double ManaRegen { get; }

        /// <summary>
        /// Gets the defense provided by this buff. 
        /// </summary>
        double Defense { get; }

        /// <summary>
        /// Gets the dodge (evasion) modifier provided by this buff. 
        /// </summary>
        double Dodge { get; }


        /// <summary>
        /// Gets the movement speed modifier of this buff. 
        /// </summary>
        double MoveSpeed { get; }
        /// <summary>
        /// Gets the movement speed percentage modifier of this buff. 
        /// </summary>
        int MoveSpeedPercentage { get; }
        /// <summary>
        /// Gets the attack speed percentage modifier of this buff. 
        /// </summary>
        int AttackSpeedPercentage { get; }
        /// <summary>
        /// Gets the mnimum damage modifier of this buff. 
        /// </summary>
        double MinDamage { get; }
        /// <summary>
        /// Gets the maximum damage modifier of this buff. 
        /// </summary>
        double MaxDamage { get; }

        /// <summary>
        /// Gets the strength modifier of this buff. 
        /// </summary>
        double Strength { get; }
        /// <summary>
        /// Gets the vitality modifier of this buff. 
        /// </summary>
        double Vitality { get; }
        /// <summary>
        /// Gets the agility modifier of this buff. 
        /// </summary>
        double Agility { get; }
        /// <summary>
        /// Gets the intellect modifier of this buff. 
        /// </summary>
        double Intellect { get; }

        /// <summary>
        /// Gets the total duration of the buff, in miliseconds. 
        /// If this value is nonpositive the buff lasts indefinitely. 
        /// </summary>
        int FullDuration { get; }


        /// <summary>
        /// Gets the unit states that are applied to units affected by this buff. 
        /// </summary>
        UnitFlags UnitStates { get; }
    }
}
