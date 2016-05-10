using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Common.Content;
using Shanism.Client.Textures;
using Shanism.Common.Game;
using Shanism.Common.Objects;
using System.Runtime.CompilerServices;
using Shanism.ScenarioLib;

namespace Shanism.Client.Assets
{
    /// <summary>
    /// Manages the creation and handling of sprites for game objects. 
    /// Assumes the <see cref="IEntity.ModelName"/> is not changed. 
    /// </summary>
    class SpriteCache
    {
        ContentList content { get; }

        ConditionalWeakTable<IEntity, Sprite> sprites { get; } = new ConditionalWeakTable<IEntity, Sprite>();




        public SpriteCache(ContentList content)
        {
            this.content = content;
        }

        /// <summary>
        /// Gets the sprite for this game object. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The object's existing sprite or a new one otherwise. </returns>
        public Sprite this[IEntity obj]
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
