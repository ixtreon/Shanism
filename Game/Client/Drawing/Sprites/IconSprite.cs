using Microsoft.Xna.Framework.Graphics;
using Shanism.Common;
using Shanism.Common.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shanism.Client.Drawing
{
    class IconSprite : SpriteBase
    {
        public IconSprite(AssetList content, string animName, Color? tint = null)
        {
            AnimationDef anim;
            if (!content.Animations.TryGetValue(animName, out anim))
                anim = content.DefaultAnimation;

            TextureDef texDef;
            if (!content.Textures.TryGet(anim.Texture, out texDef))
                texDef = TextureDef.Default;

            Texture2D tex;
            if (!content.Textures.TryGet(texDef.Name, out tex))
                tex = content.Textures.Blank;

            Texture = tex;
            SourceRectangle = anim.Span * new Vector(Texture.Width, Texture.Height) / texDef.Cells;
            Tint = tint ?? Color.White;
        }

        public void Draw(SpriteBatch sb, RectangleF drawRect, Color? customTint = null)
        {
            if (Texture != null)
                sb.ShanoDraw(Texture, SourceRectangle, drawRect, customTint ?? Tint, 0, effects, Orientation);
        }
    }
}
