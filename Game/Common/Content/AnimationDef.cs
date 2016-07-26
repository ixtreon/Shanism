using Shanism.Common.Game;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Content
{
    /// <summary>
    /// Represents an animation that can be used by in-game objects and displays one or more frames of a <see cref="TextureDef"/>. 
    /// Animations can be either dynamic or static and are further defined by their source texture and span /frames/. 
    /// </summary>
    public class AnimationDef
    {
        public const int DefaultDuration = 500;

        /// <summary>
        /// A placeholder animation that is present in all games. Uses <see cref="TextureDef.Default"/> as its texture. 
        /// </summary>
        public static readonly AnimationDef Default = new AnimationDef("dummy", TextureDef.Default.Name, new Rectangle(Point.Zero, Point.One));

        /// <summary>
        /// Gets or sets the name of this animation. 
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the source texture for this animation. 
        /// </summary>
        public string Texture { get; set; }


        /// <summary>
        /// Gets the texture cells spanned by this animation. 
        /// </summary>
        public Rectangle Span { get; set; } = new Rectangle(Point.Zero, Point.One);

        /// <summary>
        /// Gets or sets whether this animation is dynamic. 
        /// <para/>
        /// If set to true, cells in this animation's span are treated as animation frames
        /// that change every <see cref="Period"/> milliseconds. 
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

        public AnimationStyle RotationStyle { get; set; } = AnimationStyle.Fixed;

        /// <summary>
        /// Gets the number of frames in this animation. 
        /// </summary>
        public int FrameCount => IsDynamic ? Span.Area : 1;

        public int TotalDuration => IsDynamic ? Period * FrameCount : DefaultDuration;


        /// <summary>
        /// Returns the span of the n'th frame in this animation. 
        /// Values are whole numbers indicating the cells of the animation. 
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
        /// Creates a new static animation spanning all or part of a texture. 
        /// </summary>
        /// <param name="animName">The name of the animation. </param>
        /// <param name="texName">The name of the source texture. </param>
        /// <param name="span">The cell span of this animation within its texture. </param>
        public AnimationDef(string animName, string texName, Rectangle span)
        {
            Name = animName;

            IsDynamic = false;
            Texture = texName;
            Span = span;
        }

        /// <summary>
        /// Creates a new dynamic animation spanning all or part of a texture. 
        /// </summary>
        /// <param name="animName">The name of the animation. </param>
        /// <param name="texName">The source texture for the animation. </param>
        /// <param name="span"></param>
        /// <param name="period"></param>
        public AnimationDef(string animName, string texName, Rectangle span, int period)
        {
            Name = animName;

            IsDynamic = true;
            Texture = texName;
            Span = span;
            Period = period;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationDef"/> class
        /// which is a clone of the supplied animation. 
        /// </summary>
        /// <param name="base">The base animation to clone.</param>
        public AnimationDef(AnimationDef @base)
        {
            IsDynamic = @base.IsDynamic;
            Name = @base.Name;
            Period = @base.Period;
            Span = @base.Span;
            Texture = @base.Texture;
            RotationStyle = @base.RotationStyle;
        }

        /// <summary>
        /// Creates a new empty animation. 
        /// </summary>
        public AnimationDef() { }
    }
}
