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

namespace Shanism.Client.Drawing
{
    /// <summary>
    /// The visual representation of a <see cref="IEntity"/>. 
    /// Tracks the model and animation of a single <see cref="IEntity"/> and keeps the corresponding <see cref="Texture2D"/> for drawing. 
    /// </summary>
    class EntitySprite : SpriteBase
    {
        /// <summary>
        /// Gets or updates the current frame. 
        /// </summary>
        readonly Counter frameIdCounter = new Counter(1);

        /// <summary>
        /// Gets or updates the milliseconds passed since the last frame change. 
        /// </summary>
        readonly Counter frameElapsedCounter = new Counter(1);


        /// <summary>
        /// Gets the game object this sprite is attached to. 
        /// </summary>
        public readonly IEntity Entity;

        readonly AssetList content;


        TextureDef currentTexture;
        AnimationDef currentAnimation;

        Vector textureCellScale;

        float entityFacing;
        bool isFacingRight;


        RectangleF inGameBounds;

        /// <summary>
        /// Gets or sets the draw depth. 0 is back, 1 is front. 
        /// </summary>
        /// <value>
        /// The draw depth.
        /// </value>
        public float DrawDepth { get; protected set; }

        double minDepth => Screen.GameBounds.Bottom;
        double depthRange => Screen.GameBounds.Height;

        public bool RemoveFlag { get; set; }

        public EntitySprite(AssetList content, IEntity obj)
        {
            this.content = content;
            Entity = obj;

            trySetAnimation(AnimationDef.Default);
            setEntityOrientation(entityFacing);
        }

        /// <summary>
        /// Checks if the object's animation has changed and reloads its texture if needed. Also updates dynamic animations' frames. 
        /// <para>
        ///     Tries to get animation with the current <see cref="this.Object.AnimationName"/>. 
        ///     Falls back to <see cref="Object.AnimationName"/> if the current animation cannot be found. 
        /// </para>
        /// </summary>
        public override void Update(int msElapsed)
        {
            //re-fetch animation
            if (!refreshCurrentAnimation())
            {
                //update frames if dynamic animation
                if (currentAnimation.IsDynamic
                    && (Entity.LoopAnimation || frameIdCounter.Value < currentAnimation.FrameCount - 1)
                    && frameElapsedCounter.Tick(msElapsed))
                {
                    frameIdCounter.Tick();
                    SourceRectangle = currentAnimation.GetFrame(frameIdCounter.Value) * textureCellScale;
                }
            }

            //update in-game position
            var animRatio = (float)Texture.Width * currentTexture.Cells.Y / currentTexture.Cells.X / Texture.Height;
            var texSz = new Vector(Entity.Scale);

            if (animRatio > 1f)
                texSz *= new Vector(animRatio, 1);
            else
                texSz *= new Vector(1, 1 / animRatio);
            var texPos = Entity.Position - texSz / 2;

            inGameBounds = new RectangleF(texPos, texSz);
            DrawDepth = (float)((Entity.Position.Y - minDepth) / depthRange);

            //update tint
            Tint = Entity.CurrentTint;
        }

        public void Draw(SpriteBatch sb)
        {
            Draw(sb, inGameBounds, DrawDepth);
            if(ClientEngine.ShowDebugStats)
            sb.ShanoDraw(content.Circles.GetTexture(512),
                new RectangleF(Entity.Position - Entity.Scale / 2, new Vector(Entity.Scale)),
                Color.Red.SetAlpha(50));
        }


        internal void setEntityOrientation(float angle)
        {
            if (angle.Equals(entityFacing))
                return;

            entityFacing = angle;

            //only update facing if moving at least a bit along X
            var xDist = Math.Cos(angle);
            if (Math.Abs(xDist) > 1e-3)
                isFacingRight = xDist > 0;

            updateTextureOrientation();
        }


        /// <summary>
        /// Updates the current animation. Returns whether the animation was changed. 
        /// </summary>
        bool refreshCurrentAnimation()
        {
            return swapAnimation(ShanoPath.Combine(Entity.Model, Entity.Animation)) 
                ?? swapAnimation(ShanoPath.Normalize(Entity.Model)) 
                ?? swapAnimation(AnimationDef.Default.Name)
                ?? false;
        }

        bool? swapAnimation(string name)
        {
            if (currentAnimation.Name == name)
                return false;
            if (trySetAnimation(name))
                return true;
            return null;
        }

        bool trySetAnimation(string animName)
        {
            AnimationDef outAnim;
            if (!content.Animations.TryGetValue(animName, out outAnim))
                return false;

            return trySetAnimation(outAnim);
        }

        bool trySetAnimation(AnimationDef anim)
        {
            var texName = anim.Texture.ToLowerInvariant();

            TextureDef texDef;
            if (!content.Textures.TryGet(texName, out texDef))
                return false;

            Texture2D tex;
            if(!content.Textures.TryGet(texName, out tex))
                return false;

            currentTexture = texDef;
            currentAnimation = anim;

            //reset frame counters
            frameElapsedCounter.Reset(currentAnimation.Period);
            frameIdCounter.Reset(currentAnimation.FrameCount);

            //update source rect + texture
            Texture = tex;
            textureCellScale = new Vector(Texture.Width, Texture.Height) / texDef.Cells;
            SourceRectangle = currentAnimation.GetFrame(0) * textureCellScale;
            updateTextureOrientation();

            return true;
        }

        /// <summary>
        /// Updates the texture orientation using the current animation and unit orientation values.
        /// </summary>
        void updateTextureOrientation()
        {
            switch (currentAnimation.RotationStyle)
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
                    Orientation = entityFacing;
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
