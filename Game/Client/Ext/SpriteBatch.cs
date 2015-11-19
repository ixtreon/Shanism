using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IO;
using Microsoft.Xna.Framework.Graphics;
using IO.Objects;
using Client.Textures;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

namespace Client
{

    public static class SpriteBatchExt
    {
        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex, 
            Rectangle destRect, Color c)
        {
            sb.Draw(tex, destRect.ToXnaRect(), c);
        }

        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex,
            Rectangle sourceRect, Vector destPos, Vector destSz, Color c)
        {
            sb.Draw(tex,
                sourceRectangle: sourceRect.ToXnaRect(),
                position: destPos.ToXnaVector(),
                scale: (destSz / sourceRect.Size).ToXnaVector(),
                color: c);
        }

        public static void ShanoDraw(this SpriteBatch sb, Texture2D tex,
            Vector destPos, Vector destSz, Color c)
        {
            sb.Draw(tex,
                position: destPos.ToXnaVector(),
                scale: (destSz / new Vector(tex.Width, tex.Height)).ToXnaVector(),
                color: c);
        }
    }
}
