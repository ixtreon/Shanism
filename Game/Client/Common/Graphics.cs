using Shanism.Client.Assets;
using Shanism.Common;
using Shanism.Common.Game;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Shanism.Client
{
    /// <summary>
    /// A graphics object used to draw on top of some or all the MonoGame SpriteBatch. 
    /// </summary>
    class Graphics
    {
        /// <summary>
        /// Gets the SpriteBatch this Graphics works on.
        /// </summary>
        public SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Gets the bounds of the drawing area. 
        /// </summary>
        public RectangleF Bounds { get; }

        /// <summary>
        /// Gets the position of the drawing area. 
        /// </summary>
        public Vector Position { get { return Bounds.Position; } }

        /// <summary>
        /// Gets the size of the drawing area. 
        /// </summary>
        public Vector Size { get { return Bounds.Size; } }

        /// <summary>
        /// Creates a new graphics object that spans the whole sprite batch. 
        /// </summary>
        /// <param name="sb"></param>
        public Graphics(SpriteBatch sb)
        {
            SpriteBatch = sb;
            Bounds = new RectangleF(Vector.Zero, Vector.MaxValue);
        }

        public Graphics(Graphics parent, Vector offset, Vector sz)
        {
            SpriteBatch = parent.SpriteBatch;
            Bounds = new RectangleF(parent.Position + offset, sz);
        }

        public Graphics(SpriteBatch sb, Vector pos, Vector sz)
        {
            SpriteBatch = sb;
            Bounds = new RectangleF(pos, sz);
        }


        public void Draw(Texture2D tex, Vector texPos, Vector texSize, Color? color = null, float depth = 0)
        {
            if (tex == null) throw new ArgumentNullException(nameof(tex), "The texture cannot be null!");

            var screenPos = getClampedScreenPos(texPos);
            var screenSz = getClampedScreenSize(texPos, texSize);

            SpriteBatch.Draw(tex,
                position: screenPos.ToVector2(),
                scale: (screenSz / new Vector(tex.Width, tex.Height)).ToVector2(),
                color: color ?? Color.White,
                layerDepth: depth);
        }

        public void Draw(Sprite s, Vector sPos, Vector sSize, Color? color = null, float depth = 0)
        {
            if (s.Texture == null) throw new ArgumentException("The sprite has a null texture!");

            var screenPos = getClampedScreenPos(sPos);
            var screenSz = getClampedScreenSize(sPos, sSize);

            SpriteBatch.ShanoDraw(s.Texture, s.SourceRectangle, screenPos, screenSz, color ?? Color.White, depth);
        }

        Vector getClampedScreenPos(Vector pos)
        {
            pos = pos.Clamp(Vector.Zero, Size);
            return Screen.UiToScreen(Position + pos);
        }

        Vector getClampedScreenSize(Vector pos, Vector size)
        {
            size = size.Clamp(Vector.Zero, (Size - pos));
            if (size.X < 0 || size.Y < 0)
                return Vector.Zero;
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
            float xAnchor, float yAnchor,
            double? txtMaxWidth = null)
        {
            txtPos = txtPos.Clamp(Vector.Zero, Size);

            return f.DrawStringUi(SpriteBatch, text, color, Position + txtPos, xAnchor, yAnchor, txtMaxWidth);
        }

        public override string ToString()
        {
            return "Graphics @ {0} : {1}".F(Position, Size);
        }
    }
}
