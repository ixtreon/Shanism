using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Interfaces.Objects
{
    /// <summary>
    /// The static data of a buff. 
    /// </summary>
    public interface IBuff : IGameObject
    {

        /// <summary>
        /// Gets the total duration of the buff, in miliseconds. 
        /// If this value is nonpositive the buff lasts indefinitely. 
        /// </summary>
        int FullDuration { get; }


        /// <summary>
        /// Gets the type of this buff. 
        /// </summary>
        BuffStackType StackType { get; }

        /// <summary>
        /// Gets whether this buff has an icon 
        /// and is displayed in the default buff bar. 
        /// </summary>
        bool HasIcon { get; }


        /// <summary>
        /// Gets the icon of the buff. 
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// Gets the name of the buff. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the formatted description of this buff. 
        /// </summary>
        string Description { get; }


        /// <summary>
        /// Gets the unit states that are applied to units affected by this buff. 
        /// </summary>
        StateFlags StateFlags { get; }

        /// <summary>
        /// Gets the unit stat modifiers of this buff.
        /// </summary>
        IUnitStats Stats { get; }

        /// <summary>
        /// Gets the hero attribute modifiers of this buff.
        /// </summary>
        IHeroAttributes Attributes { get; }

        /// <summary>
        /// Gets the movement speed percentage modifier of this buff. 
        /// </summary>
        int MoveSpeedPercentage { get; }
        /// <summary>
        /// Gets the attack speed percentage modifier of this buff. 
        /// </summary>
        int AttackSpeedPercentage { get; }
    }
}
