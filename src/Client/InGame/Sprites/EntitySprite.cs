using Ix.Math;
using Shanism.Client.Assets;
using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Entities;
using System;

namespace Shanism.Client.Sprites
{
    /// <summary>
    /// The visual representation of a <see cref="IEntity"/>. 
    /// Tracks the model and animation of a single <see cref="IEntity"/> 
    /// and keeps the corresponding <see cref="Texture2D"/> for drawing. 
    /// </summary>
    public class EntitySprite : DynamicSprite
    {
        const float PiOverTwo = (float)Math.PI / 2;


        //user-set

        /// <summary>
        /// Gets or sets the name of the animation played on the model, if it exists.
        /// For example "", "walk", "attack"
        /// </summary>
        public string AnimationName { get; set; }
        public float Facing { get; set; } = float.NaN;
        public bool IsFacingRight { get; set; }

        /// <summary>
        /// Gets the game object this sprite is attached to. 
        /// </summary>
        public IEntity Entity { get; }

        public RectangleF InGameBounds { get; set; }

        /// <summary>
        /// Gets or sets the draw depth. 0 is back, 1 is front. 
        /// </summary>
        public float DrawDepth { get; set; }

        public Color Tint { get; set; }

        public int Age { get; set; }


        protected ContentList Content { get; }

        public EntitySprite(ContentList content, IEntity e)
        {
            Content = content;
            Entity = e;
        }

        public void SetAnimation(string anim, bool loop)
        {
            LoopAnimation = loop;
            if (anim == AnimationName)
                return;

            AnimationName = anim;
            if (!Content.Animations.TryGetChangedModel(Animation.Name, Entity.Model, AnimationName, out var shanoAnim))
                return;

            SetAnimation(shanoAnim);
            refreshOrientation();
        }

        public void SetOrientation(float angle)
        {
            if (angle.Equals(Facing))
                return;

            Facing = angle;

            //only update facing if moving at least a bit along X
            var xDist = Math.Cos(angle);
            if (Math.Abs(xDist) > 0.01)
                IsFacingRight = xDist > 0;

            refreshOrientation();
        }

        /// <summary>
        /// Updates the texture orientation using the current animation and unit orientation values.
        /// </summary>
        void refreshOrientation()
        {
            switch (Animation.RotationStyle)
            {
                case AnimationStyle.FullSizeLeft:
                    FlipHorizontal = IsFacingRight;
                    Orientation = 0;
                    break;

                case AnimationStyle.FullSizeRight:
                    FlipHorizontal = !IsFacingRight;
                    Orientation = 0;
                    break;

                case AnimationStyle.TopDown:
                    FlipHorizontal = false;
                    Orientation = PiOverTwo + Facing;
                    break;

                case AnimationStyle.Fixed:
                    FlipHorizontal = false;
                    Orientation = 0;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();

            }
        }


        public override string ToString() => $"Sprite @ {Entity}";
    }
}
