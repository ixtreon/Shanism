using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common
{
    /// <summary>
    /// An enumeration of the different states that affect units, 
    /// both temporary and semi-permanent. 
    /// </summary>
    [Flags]
    public enum StateFlags : short
    {
        /// <summary>
        /// No flags whatsoever...
        /// </summary>
        None = 0,

        /// <summary>
        /// A sleeping unit won't notice anything until it awakens. 
        /// NYI
        /// </summary>
        Sleeping = 1 << 0,

        /// <summary>
        /// A stunned unit that can't do anything. 
        /// </summary>
        Stunned = 1 << 1,

        /// <summary>
        /// A stunned unit that can't do anything. 
        /// </summary>
        Rooted = 1 << 2,

        /// <summary>
        /// A fleeing unit that just runs. 
        /// NYI
        /// </summary>
        Fleeing = 1 << 3,

        /// <summary>
        /// A casting unit that can be interrupted. 
        /// </summary>
        Casting = 1 << 4,

        /// <summary>
        /// Specifies that a unit is currently chanelling a spell. 
        /// Can be interrupted by another unit or by moving. 
        /// </summary>
        Chanelling = 1 << 5,

        /// <summary>
        /// Specifies that the unit is immune to magic damage. 
        /// </summary>
        MagicImmune = 1 << 6,

        /// <summary>
        /// Specifies that the unit is immune to physical damage. 
        /// </summary>
        PhysicalImmune = 1 << 7,

        /// <summary>
        /// Specifies that the unit has no collision with other units. 
        /// </summary>
        NoCollision = 1 << 8,

        /// <summary>
        /// Specifies that the unit has ranged attack. 
        /// </summary>
        RangedAttack = 1 << 9,

        WalkSlowly = 1 << 10,

        /// <summary>
        /// Specifies that the unit is invulnerable to all damage, both magic and physical. 
        /// </summary>
        Invulnerable = MagicImmune | PhysicalImmune,
    }

    public static class UnitFlagsExt
    {
        public static float GetMoveSpeedMultiplier(this StateFlags flags)
        {
            const StateFlags immobilizers = StateFlags.Stunned | StateFlags.Rooted;
            const StateFlags slows = StateFlags.WalkSlowly;


            if ((flags & immobilizers) != 0)
                return 0;

            if ((flags & slows) != 0)
                return 0.5f;

            return 1;
        }
    }
}
