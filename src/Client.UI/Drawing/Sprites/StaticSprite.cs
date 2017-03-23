using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using Shanism.Common.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Assets;

namespace Shanism.Client.Sprites
{
    public class StaticSprite : SpriteBase
    {
        public StaticSprite(ContentList content, string animName)
        {
            AnimationDef anim;
            if (!content.Animations.TryGetValue(animName, out anim))
                anim = content.DefaultAnimation;

            TextureDef texDef;
            if (!content.Textures.TryGetValue(anim.Texture, out texDef))
                texDef = TextureDef.Default;

            Texture2D tex;
            if (!content.Textures.TryGetValue(texDef.Name, out tex))
                tex = content.Textures.Blank;

            Texture = tex;
            SourceRectangle = anim.Span * new Vector(Texture.Width, Texture.Height) / texDef.Cells;
        }
    }
}
