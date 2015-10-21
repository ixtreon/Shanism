﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IxSerializer.Attributes;
using IO.Common;

namespace IO.Content
{

    /// <summary>
    /// Represents an animation definition as used by model definitions (see <see cref="ModelDef"/>) to display object animations. 
    /// </summary>
    [SerialKiller]
    public class AnimationDefOld
    {
        public static readonly AnimationDefOld Default = new AnimationDefOld(new TextureDef(TextureType.Model, @"default"));

        [SerialMember]
        public readonly TextureDef File;

        /// <summary>
        /// Gets whether this model is animated. 
        /// </summary>
        [SerialMember]
        public readonly bool IsAnimated;

        /// <summary>
        /// Gets the default duration of each frame in the animation. 
        /// <para/>
        /// Defined only if <see cref="IsAnimated"/> is true. 
        /// </summary>
        [SerialMember]
        public readonly int Period;


        [SerialMember]
        public readonly Rectangle SizeAndLocation;


        AnimationDefOld() { }

        /// <summary>
        /// Creates a new animation from the given texture. 
        /// </summary>
        /// <param name="f"></param>
        private AnimationDefOld(TextureDef f) 
        { 
            this.File = f;
            this.SizeAndLocation = new Rectangle(0, 0, 1, 1);
        }

        /// <summary>
        /// Creates a new animation by using all image segments in the provided texture, and looping them at a constant speed. 
        /// </summary>
        /// <param name="f">The texture to be used. </param>
        /// <param name="period">The default period. </param>
        public AnimationDefOld(TextureDef f, int period = 1000)
            : this(f)
        {
            this.Period = period;
            this.IsAnimated = true;
        }


        public AnimationDefOld(TextureDef f, Rectangle span)
            : this(f)
        {
            if (span.Position.X < 0 || span.Position.Y < 0 || span.FarPosition.X > f.Splits.X || span.FarPosition.Y > f.Splits.Y)
                throw new ArgumentOutOfRangeException("Span must be valid for the given file with size {0}".Format(f.Splits));
            this.SizeAndLocation = span;
        }

        public AnimationDefOld(TextureDef f, Point p)
            : this(f, new Rectangle(p.X, p.Y, 1, 1)) { }
    }
}