using Client.Textures;
using IO;
using IO.Common;
using IO.Content;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IO.Constants.Content;

namespace Client.Assets.Sprites
{
    /// <summary>
    /// An instance of a <see cref="ModelDef"/> 
    /// that provides methods for displaying its current animation. 
    /// </summary>
    class Sprite
    {


        /// <summary>
        /// Gets or updates the current frame. 
        /// </summary>
        Ticker currentFrame = new Ticker(1);

        /// <summary>
        /// Gets or updates the milliseconds passed since the last frame change. 
        /// </summary>
        Ticker currentTicks = new Ticker(1);


        public IGameObject Object { get; }

        ContentList ContentList { get; }

        

        public Texture2D Texture { get; protected set; }

        public Rectangle SourceRectangle { get; protected set; }


        string modelName;
        string animationName;
        ModelDef modelDef;
        AnimationDef animationDef;
        TextureDef textureDef;

        public Sprite(ContentList content, IGameObject obj)
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
            //did the model or animation change?
            if (Object.Model.ToLowerInvariant() != modelName)
            {
                //refetch model, reset animation
                modelName = Object.Model.ToLowerInvariant();

                modelDef = ContentList.ModelDict.TryGet(modelName)
                    ?? SpriteCache.DefaultModel;

                animationName = null;
            }

            if(Object.Animation.ToLowerInvariant() != animationName)
            {
                //refetch animation
                animationName = Object.Animation.ToLowerInvariant();

                animationDef = modelDef.Animations.TryGet(animationName)
                    ?? modelDef.Animations.TryGet("stand")
                    ?? modelDef.Animations.First().Value
                    ?? SpriteCache.DefaultModel.Animations.First().Value;
                textureDef = animationDef.Texture;


                //reset counters
                currentTicks.Reset(animationDef.Period);
                currentFrame.Reset(animationDef.Frames);

                //update source rect + texture
                Texture = Content.Textures[textureDef];
                SourceRectangle = 
                    animationDef.GetFrame(currentFrame.Value) * 
                    (new Vector(Texture.Width, Texture.Height) / textureDef.Splits);
                return;
            }

            //update frames if dynamic
            if(animationDef.IsDynamic)
            {
                //but only if looping or not on the last frame
                if (currentFrame.Value == animationDef.Frames - 1 && !animationDef.IsLooping)
                    return;

                //see if frame change is needed
                var nextFrame = currentTicks.Tick(msElapsed);
                if(nextFrame)
                {
                    //if so, update source rect
                    currentFrame.Tick();
                    SourceRectangle = animationDef.GetFrame(currentFrame.Value) * (new Vector(Texture.Width , Texture.Height) / textureDef.Splits);
                }
            }
        }
    }
}
