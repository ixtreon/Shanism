using IO.Common;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Textures;
using IO;
using Client.Assets.Fonts;
using Client.Assets.Sprites;

namespace Client
{
    class Graphics
    {
        public SpriteBatch SpriteBatch { get; }
        public Vector Position { get; }
        public Vector Size { get; }

        public Graphics(SpriteBatch sb, Vector pos, Vector sz)
        {
            SpriteBatch = sb;
            Position = pos;
            Size = sz;
        }


        public void Draw(Texture2D tex, Vector texPos, Vector texSize, Color? color = null)
        {
            var screenPos = getClampedScreenPos(texPos);
            var screenSz = getClampedScreenSize(texPos, texSize);

            SpriteBatch.ShanoDraw(tex, screenPos, screenSz, color ?? Color.White);
        }

        public void Draw(Sprite s, Vector sPos, Vector sSize, Color? color = null)
        {
            var screenPos = getClampedScreenPos(sPos);
            var screenSz = getClampedScreenSize(sPos, sSize);

            SpriteBatch.ShanoDraw(s.Texture, s.SourceRectangle, screenPos, screenSz, color ?? Color.White);
        }

        Vector getClampedScreenPos(Vector pos)
        {
            pos = pos.Clamp(Vector.Zero, Size);
            return Screen.UiToScreen(Position + pos);
        }

        Vector getClampedScreenSize(Vector pos, Vector size)
        {
            size = size.Clamp(Vector.Zero, Size - pos);
            return size * Screen.UiScale;
        }

        /// <summary>
        /// Gets the final rectangle in screen coordinates. 
        /// </summary>
        /// <returns></returns>
        Rectangle getDestinationRect(Vector pos, Vector size)
        {
            pos = pos.Clamp(Vector.Zero, Size);
            size = size.Clamp(Vector.Zero, Size - pos);

            var pixlPos = Screen.UiToScreen(Position + pos);
            var pixlSz = (size) * Screen.UiScale;
            return new Rectangle(pixlPos.ToPoint(), pixlSz.ToPoint());
        }

        public int DrawString(TextureFont f, string text,
            Color color, Vector txtPos,
            float xAnchor = 0.0f, float yAnchor = 0.5f,
            double? txtMaxWidth = null)
        {
            txtPos = txtPos.Clamp(Vector.Zero, Size);

            return f.DrawString(SpriteBatch, text, color, Position + txtPos, xAnchor, yAnchor, txtMaxWidth);
        }

        public override string ToString()
        {
            return "Graphics @ {0} : {1}".F(Position, Size);
        }
    }
}
