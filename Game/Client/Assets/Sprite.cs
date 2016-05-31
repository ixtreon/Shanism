using Shanism.Client.Textures;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Content;
using Shanism.Common.Objects;
using Shanism.Common.Util;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Assets
{
    /// <summary>
    /// The visual representation of a <see cref="IEntity"/>. 
    /// Tracks the model and animation of a single <see cref="IEntity"/> and keeps the corresponding <see cref="Texture2D"/> for drawing. 
    /// </summary>
    class Sprite
    {
        /// <summary>
        /// Gets or updates the current frame. 
        /// </summary>
        readonly Counter frameCounter = new Counter(1);

        /// <summary>
        /// Gets or updates the milliseconds passed since the last frame change. 
        /// </summary>
        readonly Counter elapsedCounter = new Counter(1);

        readonly ContentList ContentList;

        /// <summary>
        /// Gets the game object this sprite is attached to. 
        /// </summary>
        readonly IEntity Object;


        public Texture2D Texture { get; private set; }

        public Rectangle SourceRectangle { get; private set; }

        public double Orientation => Object.Orientation;

        AnimationDef animationDef = AnimationDef.Default;

        Vector textureFrameSize;

        public Sprite(ContentList content, IEntity obj)
        {
            ContentList = content;
            Object = obj;
        }

        /// <summary>
        /// Checks if the object's animation has changed and reloads its texture if needed. Also updates dynamic animations' frames. 
        /// <para>
        ///     Tries to get animation with the current <see cref="this.Object.AnimationName"/>. 
        ///     Falls back to the default animation if the current animation cannot be found. 
        /// </para>
        /// </summary>
        public void Update(int msElapsed)
        {
            var baseAnimationName = (Object.AnimationName ?? string.Empty).ToLowerInvariant();
            var fullAnimationName = (AnimPath.Combine(baseAnimationName, Object.AnimationSuffix)).ToLowerInvariant();

            if (tryUpdateAnim(fullAnimationName, msElapsed))
                return;

            if (tryUpdateAnim(baseAnimationName, msElapsed))
                return;

            animationDef = AnimationDef.Default;
            resetAnimationDatas();
        }

        bool tryUpdateAnim(string animName, int msElapsed)
        {
            if (animName == animationDef.Name)
            {
                updateCurrentFrame(msElapsed);
                return true;
            }

            var anims = ContentList.Animations;

            AnimationDef outAnim;
            if (anims.TryGetValue(animName, out outAnim))
            {
                animationDef = outAnim;
                resetAnimationDatas();
                return true;
            }

            return false;
        }

        void updateCurrentFrame(int msElapsed)
        {

            //update frames if dynamic
            if (animationDef.IsDynamic
                && (animationDef.IsLooping || frameCounter.Value < animationDef.FrameCount - 1)
                && elapsedCounter.Tick(msElapsed))
            {
                frameCounter.Tick();
                SourceRectangle = animationDef.GetFrame(frameCounter.Value) * textureFrameSize;
            }
        }

        void resetAnimationDatas()
        {

            //reset counters
            elapsedCounter.Reset(animationDef.Period);
            frameCounter.Reset(animationDef.FrameCount);

            //update source rect + texture
            var textureDef = Content.Listing.Textures.TryGet(animationDef.Texture.ToLowerInvariant())
                    ?? TextureDef.Default;
            Texture = Content.Textures[textureDef];
            textureFrameSize = new Vector(Texture.Width, Texture.Height) / textureDef.Splits;

            SourceRectangle = animationDef.GetFrame(0) * textureFrameSize;
        }
    }
}
