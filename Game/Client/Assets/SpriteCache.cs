using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IO.Content;
using Client.Textures;
using IO.Common;
using IO.Objects;
using System.Runtime.CompilerServices;
using ScenarioLib;

namespace Client.Assets
{
    /// <summary>
    /// Manages the creation and handling of sprites for game objects. 
    /// Assumes the <see cref="IGameObject.ModelName"/> is not changed. 
    /// </summary>
    class SpriteCache
    {
        public static ModelDef DefaultModel { get; } = new ModelDef(Constants.Content.DefaultValues.ModelName);

        static SpriteCache()
        {
            DefaultModel.Animations.Add(Constants.Content.DefaultValues.Animation,
                new AnimationDef(new TextureDef(Constants.Content.DefaultValues.ModelTexture)));
        }

        ContentList content { get; }

        ConditionalWeakTable<IGameObject, Sprite> sprites { get; } = new ConditionalWeakTable<IGameObject, Sprite>();




        public SpriteCache(ContentList content)
        {
            this.content = content;
        }

        /// <summary>
        /// Gets the sprite for this game object. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The object's existing sprite or a new one otherwise. </returns>
        public Sprite this[IGameObject obj]
        {
            get
            {
                //check if there
                var sprite = sprites.TryGet(obj);
                if(sprite == null)
                {
                    //if not, make a new one
                    sprite = new Sprite(content, obj);
                    sprites.Add(obj, sprite);
                }
                return sprite;
            }
        }
    }
}
