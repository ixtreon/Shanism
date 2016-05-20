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

        ContentList ContentList { get; }

        /// <summary>
        /// Gets the game object this sprite is attached to. 
        /// </summary>
        IEntity Object { get; }


        public Texture2D Texture { get; private set; }

        public Rectangle SourceRectangle { get; private set; }

        public double Orientation => Object.Orientation;

        string animationName;

        AnimationDef animationDef;

        TextureDef textureDef;

        Vector textureFrameSize;

        public Sprite(ContentList content, IEntity obj)
        {
            ContentList = content;
            Object = obj;
        }

        /// <summary>
        /// Checks if the object's animation has changed and reloads its texture if needed. Also updates dynamic animations' frames. 
        /// <para>
        ///     Tries to get animation with the current <see cref="animationName"/>. 
        ///     Falls back to the default animation if the current animation cannot be found. 
        /// </para>
        /// </summary>
        public void Update(int msElapsed)
        {
            var _animName = (Object.AnimationName ?? string.Empty).ToLowerInvariant();
            if (_animName != animationName)
            {
                //refetch animation
                animationName = _animName;
                animationDef = ContentList.Animations.TryGet(AnimPath.Normalize(_animName))
                    ?? AnimationDef.Default;

                //reset counters
                elapsedCounter.Reset(animationDef.Period);
                frameCounter.Reset(animationDef.FrameCount);

                //update source rect + texture
                textureDef = Content.Listing.Textures.TryGet(animationDef.Texture.ToLowerInvariant())
                    ?? TextureDef.Default;
                Texture = Content.Textures[textureDef];
                textureFrameSize = new Vector(Texture.Width, Texture.Height) / textureDef.Splits;

                SourceRectangle = animationDef.GetFrame(0) * textureFrameSize;
                return;
            }

            //update frames if dynamic
            if (animationDef.IsDynamic
                && (animationDef.IsLooping || frameCounter.Value < animationDef.FrameCount - 1)
                && elapsedCounter.Tick(msElapsed))
            {
                frameCounter.Tick();
                SourceRectangle = animationDef.GetFrame(frameCounter.Value) * textureFrameSize;
            }
        }
    }
}
