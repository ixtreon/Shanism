using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Common.Entities
{
    /// <summary>
    /// Represents a special type of unit that has secondary attributes affecting 
    /// the main attributes inherited from <see cref="IUnit"/>. 
    /// </summary>
    /// <seealso cref="IUnit" />
    public interface IHero : IUnit
    {
        IHeroAttributes BaseAttributes { get; }

        IHeroAttributes Attributes { get; }

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
