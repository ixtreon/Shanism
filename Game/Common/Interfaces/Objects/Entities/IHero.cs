using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Objects
{
    /// <summary>
    /// Represents a special type of unit that has secondary attributes affecting 
    /// the main attributes inherited from <see cref="IUnit"/>. 
    /// </summary>
    /// <seealso cref="IO.Objects.IUnit" />
    public interface IHero : IUnit
    {
        /// <summary>
        /// Gets the current strength of the unit. 
        /// </summary>
        double Strength { get; }
        /// <summary>
        /// Gets the current agility of the unit. 
        /// </summary>
        double Agility { get; }
        /// <summary>
        /// Gets the current vitality of the unit. 
        /// </summary>
        double Vitality { get; }
        /// <summary>
        /// Gets the current intellect of the unit. 
        /// </summary>
        double Intellect { get; }


        /// <summary>
        /// Gets the base strength of the unit. 
        /// </summary>
        double BaseStrength { get; }
        /// <summary>
        /// Gets the base agility of the unit. 
        /// </summary>
        double BaseAgility { get; }
        /// <summary>
        /// Gets the base vitality of the unit. 
        /// </summary>
        double BaseVitality { get; }
        /// <summary>
        /// Gets the base intellect of the unit. 
        /// </summary>
        double BaseIntellect { get; }

        /// <summary>
        /// Gets the current experience of the hero. 
        /// </summary>
        int Experience { get; }

        /// <summary>
        /// Gets the experience needed to reach the next level. 
        /// </summary>
        int ExperienceNeeded { get; }

    }
}
