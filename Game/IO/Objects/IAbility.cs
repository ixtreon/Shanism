using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IO
{
    public interface IAbility
    {
        /// <summary>
        /// Gets the name of the ability. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the ability. 
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the icon of the ability. 
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// Gets the current cooldown of the ability. 
        /// </summary>
        int CurrentCooldown { get; }

        /// <summary>
        /// Gets the total cooldown of the ability. 
        /// </summary>
        int Cooldown { get; }

        /// <summary>
        /// Gets the mana cost of the ability. 
        /// </summary>
        int ManaCost { get; }

        int CastTime { get; }

        double CastRange { get; }
    }
}
