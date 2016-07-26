using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shanism.Common.Game
{
    /* supports up to 32 (64) slots */

    /// <summary>
    /// The collection of all possible inventory slots.
    /// </summary>
    [Flags]
    public enum EquipSlot : ushort
    {
        /// <summary>
        /// No slots. 
        /// </summary>
        None = 0,

        /// <summary>
        /// The head slot. 
        /// </summary>
        Head = 1 << 0,
        /// <summary>
        /// The neck slot. 
        /// </summary>
        Neck = 1 << 1,
        /// <summary>
        /// The shoulder slot. 
        /// </summary>
        Shoulder = 1 << 2,
        /// <summary>
        /// The torso slot. 
        /// </summary>
        Torso = 1 << 3,
        /// <summary>
        /// The back slot. 
        /// </summary>
        Back = 1 << 4,
        /// <summary>
        /// The legs slot. 
        /// </summary>
        Legs = 1 << 5,
        /// <summary>
        /// The feet slot. 
        /// </summary>
        Feet = 1 << 6,
        /// <summary>
        /// The arms slot. 
        /// </summary>
        Arms = 1 << 7,
        /// <summary>
        /// The main hand slot. 
        /// </summary>
        MainHand = 1 << 8,
        /// <summary>
        /// The off hand slot. 
        /// </summary>
        OffHand = 1 << 9,


        /// <summary>
        /// All slots. 
        /// </summary>
        All = (1 << 16) - 1,
    }
}
