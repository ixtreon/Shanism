using Shanism.Common;
using Shanism.Common.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Sprites
{
    public class DynamicSprite : SpriteBase
    {


        /// <summary>
        /// Gets or updates the current frame. 
        /// </summary>
        readonly Counter frameIdCounter = new Counter(1);

        /// <summary>
        /// Gets or updates the milliseconds passed since the last frame change. 
        /// </summary>
        readonly Counter frameElapsedCounter = new Counter(1);

        protected AnimationDef animation;
        protected Vector frameSize;

        protected ContentList Content { get; }
        public bool LoopAnimation { get; protected set; }


        public DynamicSprite(ContentList content)
        {
            this.Content = content;

        }
        public DynamicSprite(ContentList content, string animName)
            : this(content)
        {
            TrySetAnimation(animName);
        }


        public override void Update(int msElapsed)
        {
            // cycle animation
            if (animation.IsDynamic && frameElapsedCounter.Tick(msElapsed))
            {
                var shouldResetFrames = frameIdCounter.Tick() && !LoopAnimation;

                if (shouldResetFrames)
                    TrySetAnimation(Content.DefaultAnimation);
                else
                    SourceRectangle = animation.GetFrame(frameIdCounter.Value) * frameSize;
            }
        }

        protected bool TrySetAnimation(string fullName)
        {
            AnimationDef outAnim;
            if (!Content.Animations.TryGetValue(fullName ?? string.Empty, out outAnim))
                return false;

            return TrySetAnimation(outAnim);
        }

        protected bool TrySetAnimation(AnimationDef anim)
        {
            if (animation?.Name == anim.Name)
                return true;

            var texName = anim.Texture.ToLowerInvariant();

            TextureDef texDef;
            if (!Content.Textures.TryGetValue(texName, out texDef))
                return false;

            Texture2D tex;
            if (!Content.Textures.TryGetValue(texName, out tex))
                return false;

            //set vars
            Texture = tex;
            animation = anim;

            //reset frame counters
            frameElapsedCounter.Reset(animation.Period);
            frameIdCounter.Reset(animation.FrameCount);

            //update source rect + texture
            frameSize = new Vector(Texture.Width, Texture.Height) / texDef.Cells;
            SourceRectangle = animation.GetFrame(0) * frameSize;

            return true;
        }


        public override string ToString() => animation.Name;
    }
}
