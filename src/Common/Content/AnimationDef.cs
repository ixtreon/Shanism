using Ix.Math;
using Newtonsoft.Json;
using System;

namespace Shanism.Common.Content
{
    /// <summary>
    /// Represents an animation that can be used by in-game objects and displays one or more frames of a <see cref="TextureDef"/>. 
    /// Animations can be either dynamic or static and are further defined by their source texture and span /frames/. 
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AnimationDef : IEquatable<AnimationDef>, IComparable<AnimationDef>
    {

        static readonly Rectangle UnitRect = new Rectangle(Point.Zero, Point.One);

        RectangleF _pathingRect = UnitRect;

        /// <summary>
        /// Gets or sets the name of this animation. 
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the source texture for this animation. 
        /// </summary>
        [JsonProperty]
        public string Texture { get; set; }


        /// <summary>
        /// Gets the texture cells spanned by this animation. 
        /// </summary>
        [JsonProperty]
        public Rectangle Span { get; set; } = UnitRect;

        /// <summary>
        /// Gets or sets whether this animation is dynamic. 
        /// <para/>
        /// If set to true, cells in this animation's span are treated as animation frames
        /// that change every <see cref="Period"/> milliseconds. 
        /// <para/>
        /// If set to false, all cells in this animation's span are treated 
        /// as part of one frame. 
        /// </summary>
        [JsonProperty]
        public bool IsDynamic { get; set; }

        /// <summary>
        /// Gets or sets the period at which 
        /// the frames in a dynamic texture are changed. 
        /// Undefined if <see cref="IsDynamic"/> is false. 
        /// </summary>
        [JsonProperty]
        public int Period { get; set; }


        [JsonProperty]
        public RectangleF PathingRect
        {
            get => _pathingRect;
            set => _pathingRect = (value != RectangleF.Empty) ? value : UnitRect;
        }

        [JsonProperty]
        public AnimationStyle RotationStyle { get; set; } = AnimationStyle.Fixed;

        /// <summary>
        /// Gets the number of frames in this animation. 
        /// </summary>
        public int FrameCount => IsDynamic ? Span.Area : 1;


        /// <summary>
        /// Creates a new empty animation. 
        /// </summary>
        public AnimationDef() { }

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

        public Point GetFrame(int frame)
        {
            var w = Math.Max(1, Span.Width);
            return Span.Position + new Point(frame % w, frame / w);
        }

        public int CompareTo(AnimationDef other)
            => string.Compare(Name, other.Name, StringComparison.Ordinal);


        public bool Equals(AnimationDef other)
            => Name == other.Name;

        public override int GetHashCode()
            => Name.GetHashCode();

        public override bool Equals(object obj)
            => obj is AnimationDef other
            && Name == other.Name;
    }
}
