﻿using IO.Common;
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
        /// <summary>
        /// A placeholder animation that is present in all games. Uses <see cref="TextureDef.Default"/> as its texture. 
        /// </summary>
        public static readonly AnimationDef Default = new AnimationDef(TextureDef.Default);

        public TextureDef Texture { get; set; }

        /// <summary>
        /// Gets the span of this animation. 
        /// </summary>
        public Rectangle Span { get; set; }

        /// <summary>
        /// Gets or sets whether this animation is dynamic. 
        /// <para/>
        /// If set to true, cells in this animation's span are treated as animation frames
        /// that change every <see cref="Period"/> milliseconds and optionally 
        /// loop forever (see <see cref="IsLooping"/>). 
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

        /// <summary>
        /// Gets the number of frames in this animation. 
        /// </summary>
        public int Frames {  get { return IsDynamic ? Span.Area : 1; } }

        /// <summary>
        /// Returns the span of the n'th frame in this animation. 
        /// </summary>
        /// <param name="frame">The consecutive number of the frame, starting from 0. </param>
        public Rectangle GetFrame(int frame)
        {
            if (!IsDynamic)
                return Span;

            var w = Span.Width > 0 ? Span.Width : 1;
            var h = Span.Height > 0 ? Span.Height  : 1;

            var x = Span.X + (frame % w);
            var y = Span.Y + (frame / w);
            return new Rectangle(x, y, 1, 1);
        }

        /// <summary>
        /// Creates a new animation spanning the whole texture. 
        /// </summary>
        /// <param name="tex"></param>
        public AnimationDef(TextureDef tex)
        {
            IsDynamic = false;
            Texture = tex;
            Span = new Rectangle(Point.Zero, tex.Splits);
        }

        /// <summary>
        /// Creates a new dynamic animation all or part of a texture. 
        /// </summary>
        /// <param name="tex">The source texture for the animation. </param>
        /// <param name="span"></param>
        /// <param name="period"></param>
        /// <param name="isLooping"></param>
        public AnimationDef(TextureDef tex, Rectangle span, int period = 100, bool isLooping = true)
        {
            IsDynamic = true;
            Texture = tex;
            Span = span;
            Period = period;
            IsLooping = isLooping;
        }

        public AnimationDef() { }
    }
}