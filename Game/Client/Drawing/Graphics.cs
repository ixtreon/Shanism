using Shanism.Client.Drawing;
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
        readonly Stack<RectangleF> drawStack = new Stack<RectangleF>();

        /// <summary>
        /// Gets the SpriteBatch this Graphics works on.
        /// </summary>
        public SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Gets the bounds of the drawing area. 
        /// </summary>
        public RectangleF Bounds { get; private set; }

        /// <summary>
        /// Gets the position of the drawing area. 
        /// </summary>
        public Vector Position => Bounds.Position;

        /// <summary>
        /// Gets the size of the drawing area. 
        /// </summary>
        public Vector Size => Bounds.Size;

        /// <summary>
        /// Creates a new graphics object that spans the whole sprite batch. 
        /// </summary>
        /// <param name="sb"></param>
        public Graphics(SpriteBatch sb)
        {
            SpriteBatch = sb;
            Bounds = new RectangleF(Vector.Zero, Vector.MaxValue);
        }

        public Graphics(SpriteBatch sb, Vector offset, Vector sz)
        {
            SpriteBatch = sb;
            Bounds = new RectangleF(offset, sz);
        }

        public void PushWindow(Vector offset, Vector sz)
        {
            offset = Vector.Max(Vector.Zero, offset);
            sz = Vector.Min(sz, Size - offset);

            drawStack.Push(Bounds);
            Bounds = new RectangleF(Position + offset, sz);
        }

        public void PopWindow()
        {
            Bounds = drawStack.Pop();
        }


        //public void DrawIcon(string iconName, 


        public void Draw(Texture2D tex, Vector texPos, Vector texSize, Color? color = null, float depth = 0)
        {
            if (tex == null)
                throw new ArgumentNullException(nameof(tex), "The texture cannot be null!");

            var screenPos = getClampedScreenPos(texPos);
            var screenSz = getClampedScreenSize(texPos, texSize);
            SpriteBatch.Draw(tex,
                position: screenPos.ToVector2(),
                scale: (screenSz / new Vector(tex.Width, tex.Height)).ToVector2(),
                color: (color ?? Color.White),
                layerDepth: depth);
        }

        public void Draw(EntitySprite s, Vector sPos, Vector sSize, 
            float depth = 0)
        {
            var screenPos = getClampedScreenPos(sPos);
            var screenSz = getClampedScreenSize(sPos, sSize);
            var screenRect = new RectangleF(screenPos, screenSz);

            s.Draw(SpriteBatch, screenRect, depth);
        }

        public void DrawString(TextureFont f, string text,
            Color color, Vector txtPos,
            float xAnchor, float yAnchor,
            double? txtMaxWidth = null)
        {
            txtPos = txtPos.Clamp(Vector.Zero, Size);

            var screenPos = Screen.UiToScreen(Position + txtPos);
            var screenMaxWidth = Screen.UiScale * txtMaxWidth;
            f.DrawString(SpriteBatch, text, color, screenPos, 
                xAnchor, yAnchor, screenMaxWidth);
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

        public override string ToString() => $"Graphics @ {Position} : {Size}";
    }
}
