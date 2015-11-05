using IO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Content
{
    public class AnimationDef
    {
        public TextureDef Texture { get; set; }

        public Rectangle Span { get; set; }

        /// <summary>
        /// Gets or sets whether this animation is dynamic. 
        /// i.e. cells in its span are treated as animation frames
        /// changed every <see cref="Period"/> milliseconds and optionally
        /// whether it loops (see <see cref="IsLooping"/>). 
        /// <para/>
        /// If set to false, all cells in this animation's span are treated 
        /// as part of one frame. 
        /// </summary>
        public bool IsDynamic { get; set; }

        /// <summary>
        /// Gets or sets the period at which 
        /// the frames in a dynamic texture are changed. 
        /// Undefined if <see cref="IsDynamic"/> is false. 
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets whether a dynamic animation is looping. 
        /// Undefined if <see cref="IsDynamic"/> is false. 
        /// </summary>
        public bool IsLooping { get; set; }
    }
}
