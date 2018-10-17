using Ix.Math;
using Shanism.Common.Content;
using Shanism.Common.Util;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Shanism.Client
{
    public class ShanoAnimation
    {
        /// <summary>
        /// Gets the name (also the path) of the animation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the underlying texture of the animation.
        /// </summary>
        public ShanoTexture Texture { get; }

        /// <summary>
        /// Gets the period between animation frames, in milliseconds.
        /// </summary>
        public int Period { get; }

        public AnimationStyle RotationStyle { get; }

        public IReadOnlyList<Rectangle> Frames { get; }

        /// <summary>
        /// Gets the aspect ratio of the texture's frames.
        /// </summary>
        Vector2 InGameRatio { get; }

        /// <summary>
        /// Gets whether this is a dynamic texture.
        /// </summary>
        public bool IsDynamic => Frames.Count > 1;

        readonly AnimationDef def;

        public ShanoAnimation(AnimationDef def, ShanoTexture tex)
        {
            this.def = def;

            Name = ShanoPath.NormalizeAnimation(def.Name);
            Texture = tex ?? throw new ArgumentNullException(nameof(tex));

            Period = def.Period;
            RotationStyle = def.RotationStyle;

            InGameRatio = tex.CellSize / Math.Max(tex.CellSize.X, tex.CellSize.Y);
            Frames = getFrames(def, tex);
        }

        static IReadOnlyList<Rectangle> getFrames(AnimationDef def, ShanoTexture tex)
        {
            if(def.IsDynamic)
            {
                var frames = new Rectangle[def.FrameCount];
                for(int i = 0; i < def.FrameCount; i++)
                    frames[i] = (new RectangleF(def.GetFrame(i), Point.One) * tex.CellSize).ToRectangle();
                return frames;
            }
            else
            {
                return new[] { ((RectangleF)def.Span * tex.CellSize).ToRectangle() };
            }
        }

        public Rectangle GetSourceRect(int frame)
            => Frames[frame];

        public RectangleF GetTextureBounds(Vector2 entityPos, float entityScale)
        {
            var texSz = InGameRatio * entityScale;
            return new RectangleF(entityPos - def.PathingRect.Center * texSz, texSz);
        }
    }
}
