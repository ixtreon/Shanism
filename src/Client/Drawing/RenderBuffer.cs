using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Shanism.Client
{
    public class RenderBuffer
    {
        readonly GraphicsDevice graphics;

        public RenderTarget2D buffer;

        Point bufferSize;
        Point windowSize;

        public RenderBuffer(GraphicsDevice graphics)
        {
            this.graphics = graphics;

            var w = graphics.Adapter.CurrentDisplayMode.Width;
            var h = graphics.Adapter.CurrentDisplayMode.Height;
            buffer = new RenderTarget2D(graphics, w, h,
                false, SurfaceFormat.Color,
                DepthFormat.Depth24, 8, RenderTargetUsage.DiscardContents);
        }


        public void Apply(Point bufferSize, Point windowSize)
        {
            this.bufferSize = bufferSize;
            this.windowSize = windowSize;

            // see if buffer needs upscaling
            var needsRefresh = windowSize.X > buffer.Width || windowSize.Y > buffer.Height;
            if (needsRefresh)
            {
                buffer?.Dispose();
                buffer = new RenderTarget2D(graphics, windowSize.X, windowSize.Y, 
                    false, SurfaceFormat.Color, 
                    DepthFormat.Depth24, 8, RenderTargetUsage.DiscardContents);
            }

            // apply buffer
            graphics.SetRenderTarget(buffer);

            // clear, unless just upscaled
            if (!needsRefresh)
            {
                graphics.Clear(Color.Transparent);
            }
        }

        public void DrawToScreen(SpriteBatch sb)
        {
            graphics.SetRenderTarget(null);

            sb.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend, SamplerState.AnisotropicWrap,
                DepthStencilState.None, RasterizerState.CullNone);

            var srcRect = new Rectangle(0, 0, bufferSize.X, bufferSize.Y);
            var destRect = new Rectangle(0, 0, windowSize.X, windowSize.Y);
            sb.Draw(buffer, destRect, srcRect, Color.White);

            sb.End();

        }
    }
}
