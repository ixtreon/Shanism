using Shanism.Common;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Sprites;

namespace Shanism.Client
{
    /// <summary>
    /// A hipster SpriteBatch. 
    /// </summary>
    public class Canvas : SpriteBatch
    {
        readonly Stack<RectangleF> drawStack = new Stack<RectangleF>();

        readonly Screen screen;


        /// <summary>
        /// Gets the bounds of the drawing area. 
        /// </summary>
        public RectangleF Bounds { get; set; }

        /// <summary>
        /// Gets the position of the drawing area. 
        /// </summary>
        public Vector Position => Bounds.Position;

        /// <summary>
        /// Gets the size of the drawing area. 
        /// </summary>
        public Vector Size => Bounds.Size;

        GraphicsDevice device => screen.GraphicsDevice;

        public Canvas(Screen screen)
            : base(screen.GraphicsDevice)
        {
            this.screen = screen;
        }

        public void Begin()
        {
            Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone);
        }

        /// <summary>
        /// Zooms in on the specified window relative to the current window.
        /// </summary>
        public void PushWindow(Vector offset, Vector sz)
        {
            offset = Vector.Max(Vector.Zero, offset);
            sz = Vector.Min(sz, Size - offset);

            drawStack.Push(Bounds);
            Bounds = new RectangleF(Position + offset, sz);
        }

        /// <summary>
        /// Zooms out of the last applied zoom window.
        /// </summary>
        public void PopWindow()
        {
            Bounds = drawStack.Pop();
        }

        public void Draw(Texture2D tex, Vector pos, Vector size, Color? color = null, float depth = 0)
        {
            if (tex == null) throw new ArgumentNullException(nameof(tex));

            clampPositionAndSize(ref pos, ref size);
            uiToScreen(ref pos, ref size);

            Draw(tex,
                position: pos.ToVector2(),
                scale: new Vector(size.X / tex.Width, size.Y / tex.Height).ToVector2(),
                color: (color ?? Color.White).ToXnaColor(),
                layerDepth: depth);
        }


        public void DrawSprite(EntitySprite s, Vector pos, Vector size,
            float depth = 0)
        {
            clampPositionAndSize(ref pos, ref size);
            uiToScreen(ref pos, ref size);

            s.Draw(this, new RectangleF(pos, size), depth);
        }


        public void Draw(StaticSprite s, Vector pos, Vector size, Color? tint = null)
        {
            clampPositionAndSize(ref pos, ref size);
            uiToScreen(ref pos, ref size);

            s.Draw(this, new RectangleF(pos, size), tint ?? Color.White);
        }


        public void DrawString(TextureFont f, string text, Color color,
            Vector txtPos,
            float xAnchor, float yAnchor, double? maxWidth = null)
        {
            var screenPos = screen.UiToScreen(Position + txtPos);
            var screenMaxWidth = screen.UiScale * maxWidth;

            if (string.IsNullOrEmpty(text))
                return;

            if (screenMaxWidth != null)
                text = f.SplitLinesPx(screen, text, screenMaxWidth.Value);

            var sz = f.MeasureStringPx(screen, text);
            var drawPos = screenPos - new Vector(sz.X * xAnchor, sz.Y * yAnchor);

            DrawString(f.Font, text, drawPos.ToVector2(),
                color.ToXnaColor(),
                0, Microsoft.Xna.Framework.Vector2.Zero,
                (float)(f.pixelToUiz * screen.UiScale), SpriteEffects.None, 0);
        }

        void clampPositionAndSize(ref Vector pos, ref Vector size)
        {
            //pos = pos.Clamp(Vector.Zero, Size - size);
            //size = size.Clamp(Vector.Zero, (Size - pos));
        }

        void uiToScreen(ref Vector pos, ref Vector size)
        {
            pos = screen.UiToScreen(Position + pos);
            size = screen.UiToScreen(size);
        }

        public override string ToString() => $"Graphics @ {Position} : {Size}";
    }
}
