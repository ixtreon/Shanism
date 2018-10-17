using Ix.Math;
using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.IO;
using Shanism.Common.Content;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using XnaMatrix = Microsoft.Xna.Framework.Matrix;

namespace Shanism.Client
{

    /// <summary>
    /// A wrapper around SpriteBatch. 
    /// </summary>
    public sealed class CanvasStarter
    {

        readonly Stack<RectangleF> drawStack = new Stack<RectangleF>();

        ScreenSystem Screen { get; }

        public ShanoTexture BlankTexture { get; }

        public SpriteBatch SpriteBatch { get; }

        Vector2 DrawScale;
        Vector2 DrawOffset;

        internal CanvasStarter(GraphicsDevice device, ScreenSystem screen)
        {
            SpriteBatch = new SpriteBatch(device);
            Screen = screen;

            var blank = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            BlankTexture = new ShanoTexture(blank, new TextureDef("blank", Point.One));
        }

        #region Begin / End

        /// <summary>
        /// Begins drawing in UI space, where the smaller dimension is always of size 2 for some reason.
        /// </summary>
        public Canvas BeginUI()
            => Begin(Screen.UI.Bounds, SamplerState.AnisotropicClamp, SpriteSortMode.Deferred);

        /// <summary>
        /// Begins drawing in in-game space. Sorts sprites by their ZOrder.
        /// </summary>
        /// <returns></returns>
        public Canvas BeginInGame()
            => Begin(Screen.Game.Bounds, SamplerState.PointClamp, SpriteSortMode.FrontToBack);

        public Canvas BeginShader(Effect e)
            => Begin(Screen.UI.Bounds, SamplerState.LinearClamp, SpriteSortMode.Immediate, e);

        public Canvas BeginScreen(SamplerState samplerState, SpriteSortMode sortMode = SpriteSortMode.Deferred)
            => Begin(new RectangleF(Vector2.Zero, Screen.WindowSize), samplerState, sortMode);


        static readonly BlendState blendState = new BlendState
        {
            AlphaBlendFunction = BlendFunction.Add,
            AlphaDestinationBlend = Blend.InverseSourceAlpha,
            AlphaSourceBlend = Blend.One,

            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            //ColorSourceBlend = Blend.SourceAlpha,
            ColorSourceBlend = Blend.SourceAlpha,
        };

        Canvas Begin(RectangleF sourceRect, SamplerState samplerState, SpriteSortMode sortMode, Effect e = null)
        {

            // begin drawing
            DrawOffset = -sourceRect.Position;
            DrawScale = Screen.WindowSize / sourceRect.Size;
            var mat = XnaMatrix.CreateTranslation(DrawOffset.X, DrawOffset.Y, 0)
                    * XnaMatrix.CreateScale(DrawScale.X, DrawScale.Y, 1);

            SpriteBatch.Begin(sortMode, blendState, samplerState, DepthStencilState.DepthRead, RasterizerState.CullNone, e, mat);

            Debug.Assert(!drawStack.Any());

            // get canvas/drawcall
            return Push(Vector2.Zero, new RectangleF(Vector2.Zero, sourceRect.Size));
        }

        #endregion

        internal Canvas Push(Vector2 drawOffset, RectangleF window)
        {
            drawStack.Push(Rectangle.Empty);

            return new Canvas(this, drawOffset, window);
        }

        internal void Pop()
        {
            drawStack.Pop();

            if (!drawStack.Any())
                SpriteBatch.End();
        }

    }
}
