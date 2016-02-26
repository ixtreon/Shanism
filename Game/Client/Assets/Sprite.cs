using Client.Textures;
using IO;
using IO.Common;
using IO.Content;
using IO.Objects;
using IO.Util;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Assets
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


        public Texture2D Texture { get; protected set; }

        public Rectangle SourceRectangle { get; protected set; }


        string modelName;
        string animationName;

        AnimationDef animationDef;


        TextureDef textureDef => Content.Listing.Textures.TryGet(animationDef.Texture.ToLowerInvariant());


        public Sprite(ContentList content, IEntity obj)
        {
            ContentList = content;
            Object = obj;
        }

        /// <summary>
        /// Updates the sprite and returns whether it should be destroyed. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(int msElapsed)
        {
            //check if animation changed
            updateAnimation(msElapsed);

        }

        /// <summary>
        /// Checks if the object's animation has changed and reloads its texture if needed. Also updates dynamic animations' frames. 
        /// <para>
        ///     Tries to get <see cref="animationName"/>, then "stand", then any animation from the current model, in this order. 
        ///     Falls back to the default model's animation if the current model has no animations. 
        /// </para>
        /// </summary>
        void updateAnimation(int msElapsed)
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
                frameCounter.Reset(animationDef.Frames);

                //update source rect + texture
                Texture = Content.Textures[textureDef];
                SourceRectangle =
                    animationDef.GetFrame(frameCounter.Value) *
                    (new Vector(Texture.Width, Texture.Height) / textureDef.Splits);
            }
            else if (animationDef.IsDynamic)    //update frames if dynamic
            {
                //but only if looping or not on the last frame
                if (frameCounter.Value == animationDef.Frames - 1 && !animationDef.IsLooping)
                    return;

                //see if frame change is needed
                var nextFrame = elapsedCounter.Tick(msElapsed);
                if (nextFrame)
                {
                    //if so, update source rect
                    frameCounter.Tick();
                    SourceRectangle = animationDef.GetFrame(frameCounter.Value) * (new Vector(Texture.Width, Texture.Height) / textureDef.Splits);
                }
            }
        }
    }
}
