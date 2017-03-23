using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Util;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Client.Sprites
{
    /// <summary>
    /// The visual representation of a <see cref="IEntity"/>. 
    /// Tracks the model and animation of a single <see cref="IEntity"/> and keeps the corresponding <see cref="Texture2D"/> for drawing. 
    /// </summary>
    public class EntitySprite : DynamicSprite
    {

        /// <summary>
        /// Gets the game object this sprite is attached to. 
        /// </summary>
        public readonly IEntity Entity;

        protected IShanoComponent Game { get; }


        TextureDef currentTexture;
        RectangleF inGameBounds;

        float entityFacing = float.NaN;
        bool isFacingRight;

        /// <summary>
        /// Gets or sets the draw depth. 0 is back, 1 is front. 
        /// </summary>
        public float DrawDepth { get; protected set; }

        public Color Tint { get; protected set; }

        double minDepth => Game.Screen.GameBounds.Bottom;
        double depthRange => Game.Screen.GameBounds.Height;

        public bool RemoveFlag { get; set; }

        public bool ShowDebugStats { get; set; } = true;

        //user-set
        protected string AnimationName;

        public EntitySprite(IShanoComponent game, IEntity obj)
            : base(game.Content)
        {
            this.Game = game;

            Entity = obj;

            TrySetAnimation(Content.DefaultAnimation);
            SetOrientation(obj.Orientation);
        }
        string modelName;
        /// <summary>
        /// Checks if the object's animation has changed and reloads its texture if needed. Also updates dynamic animations' frames. 
        /// <para>
        ///     Tries to get animation with the current <see cref="AnimationName"/>. 
        ///     Falls back to <see cref="IEntity.Model"/> if the current animation cannot be found. 
        /// </para>
        /// </summary>
        public override void Update(int msElapsed)
        {
            modelName = refreshAnimation();

            base.Update(msElapsed);

            //update in-game position from unit
            var size = new Vector(Entity.Scale);

            var frameScale = frameSize.X / frameSize.Y;
            if(frameScale > 1f)
                size *= new Vector(frameScale, 1);
            else
                size *= new Vector(1, 1 / frameScale);

            inGameBounds = new RectangleF(Entity.Position - size / 2, size);
            DrawDepth = (float)((Entity.Position.Y - minDepth) / depthRange);

            //update tint
            Tint = Entity.CurrentTint;
        }

        public void Draw(SpriteBatch sb)
        {
            Draw(sb, inGameBounds, DrawDepth);

            if(ShowDebugStats)
            {
                const int texSz = 512;
                sb.ShanoDraw(Content.Circles.GetTexture(texSz),
                    new Rectangle(0, 0, texSz, texSz),
                    new RectangleF(Entity.Position - Entity.Scale / 2, new Vector(Entity.Scale)),
                    Color.Red.SetAlpha(50));
            }
        }

        protected void SetAnimation(string anim, bool loop)
        {
            LoopAnimation = loop;
            if(anim != AnimationName)
            {
                AnimationName = anim;
                refreshAnimation();
            }
        }

        protected void SetOrientation(float angle)
        {
            if(angle.Equals(entityFacing))
                return;

            entityFacing = angle;

            //only update facing if moving at least a bit along X
            var xDist = Math.Cos(angle);
            if(Math.Abs(xDist) > 0.01)
                isFacingRight = xDist > 0;

            refreshOrientation();
        }

        string refreshAnimation()
        {
            //only continue if animation changed
            var modelAnim = ShanoPath.Combine(Entity.Model, AnimationName);
            if(animation.Name == modelAnim)
                return modelAnim;

            // try refetch model+anim
            // e.g. "units/hero/move"
            if(TrySetAnimation(modelAnim))
            {
                refreshOrientation();
                return modelAnim;
            }

            // try just model
            // e.g. "units/hero"
            if(TrySetAnimation(Entity.Model))
            {
                refreshOrientation();
                AnimationName = string.Empty;
                LoopAnimation = true;
                return Entity.Model;
            }

            //set default/dummy
            TrySetAnimation(Content.DefaultAnimation);
            refreshOrientation();
            AnimationName = string.Empty;
            return Content.DefaultAnimation.Name;
        }

        const float PiOverTwo = (float)Math.PI / 2;

        /// <summary>
        /// Updates the texture orientation using the current animation and unit orientation values.
        /// </summary>
        void refreshOrientation()
        {
            switch(animation.RotationStyle)
            {
                case AnimationStyle.FullSizeLeft:
                    FlipHorizontal = isFacingRight;
                    Orientation = 0;
                    break;

                case AnimationStyle.FullSizeRight:
                    FlipHorizontal = !isFacingRight;
                    Orientation = 0;
                    break;

                case AnimationStyle.TopDown:
                    FlipHorizontal = false;
                    Orientation = PiOverTwo + entityFacing;
                    break;

                default:
                    FlipHorizontal = false;
                    Orientation = 0;
                    break;
            }
        }

        public override string ToString() => $"Sprite @ {Entity}";
    }
}
