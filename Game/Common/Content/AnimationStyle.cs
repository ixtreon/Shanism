using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Content
{
    public enum AnimationStyle
    {
        /// <summary>
        /// Indicates that an animation should not rotate at all. 
        /// </summary>
        Fixed,

        /// <summary>
        /// An animation that faces either left or right,
        /// depending on the entity's orientation. 
        /// Indicates that the original texture is looking left. 
        /// </summary>
        FullSizeLeft,

        /// <summary>
        /// An animation that faces either left or right,
        /// depending on the entity's orientation. 
        /// Indicates that the original texture is looking right. 
        /// </summary>
        FullSizeRight,

        /// <summary>
        /// An animation that rotates at 360deg depending on the 
        /// entity's orientation. 
        /// </summary>
        TopDown,
    }
}
