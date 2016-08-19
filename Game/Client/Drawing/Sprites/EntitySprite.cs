﻿using Shanism.Common;
using Shanism.Common.Content;
using Shanism.Common.Util;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Interfaces.Entities;
using Shanism.Client;

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
        RectangleF inGameBounds;
        float entityFacing = float.NaN;
        bool isFacingRight;

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

        //user-set
        bool LoopAnimation;
        protected string AnimationName;

        public EntitySprite(AssetList content, IEntity obj)
        {
            this.content = content;
            Entity = obj;

            trySetAnimation(content.DefaultAnimation);
            SetOrientation(entityFacing);
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

            //update current frame if dynamic animation
            if (currentAnimation.IsDynamic
                && frameElapsedCounter.Tick(msElapsed))
            {
                var resetFrames = frameIdCounter.Tick();
                if (!LoopAnimation && resetFrames)
                    SetAnimation(string.Empty, true);
                else
                    SourceRectangle = currentAnimation.GetFrame(frameIdCounter.Value) * textureCellScale;
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

            if (ClientEngine.ShowDebugStats)
                sb.Draw(content.Circles.GetTexture(512),
                    new RectangleF(Entity.Position - Entity.Scale / 2, new Vector(Entity.Scale)).ToRectangle().ToXnaRect(),
                    Microsoft.Xna.Framework.Color.Red.SetAlpha(50));
        }

        protected void SetAnimation(string anim, bool loop)
        {
            LoopAnimation = loop;
            if (anim != AnimationName)
            {
                AnimationName = anim;
                refreshAnimation();
            }
        }
        protected void SetOrientation(float angle)
        {
            if (angle.Equals(entityFacing))
                return;

            entityFacing = angle;

            //only update facing if moving at least a bit along X
            var xDist = Math.Cos(angle);
            if (Math.Abs(xDist) > 1e-3)
                isFacingRight = xDist > 0;

            refreshOrientation();
        }


        bool trySetAnimation(string fullName)
        {
            AnimationDef outAnim;
            if (!content.Animations.TryGetValue(fullName ?? string.Empty, out outAnim))
                return false;

            return trySetAnimation(outAnim);
        }

        bool trySetAnimation(AnimationDef anim)
        {
            if (currentAnimation == anim)
                return true;

            var texName = anim.Texture.ToLowerInvariant();

            TextureDef texDef;
            if (!content.Textures.TryGet(texName, out texDef))
                return false;

            Texture2D tex;
            if (!content.Textures.TryGet(texName, out tex))
                return false;

            //set vars
            Texture = tex;
            currentTexture = texDef;
            currentAnimation = anim;

            //reset frame counters
            frameElapsedCounter.Reset(currentAnimation.Period);
            frameIdCounter.Reset(currentAnimation.FrameCount);

            //update source rect + texture
            textureCellScale = new Vector(Texture.Width, Texture.Height) / texDef.Cells;
            SourceRectangle = currentAnimation.GetFrame(0) * textureCellScale;
            refreshOrientation();

            return true;
        }

        string refreshAnimation()
        {
            //check if same model+anim
            var modelAnim = ShanoPath.Combine(Entity.Model, AnimationName);
            if (ShanoPath.Equals(currentAnimation.Name, modelAnim))
                return modelAnim;

            //try refetch model+anim
            if (trySetAnimation(modelAnim))
                return modelAnim;

            //try just model
            if (trySetAnimation(Entity.Model))
            {
                AnimationName = string.Empty;
                LoopAnimation = true;
                return Entity.Model;
            }

            //set default
            trySetAnimation(content.DefaultAnimation);
            AnimationName = string.Empty;
            return content.DefaultAnimation.Name;
        }


        /// <summary>
        /// Updates the texture orientation using the current animation and unit orientation values.
        /// </summary>
        void refreshOrientation()
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
