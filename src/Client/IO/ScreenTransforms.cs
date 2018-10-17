using Ix.Math;
using System;
using System.Numerics;

namespace Shanism.Client.IO
{
    public class InGameTransform
    {
        const float DefaultInGameArea = 5_000;

        readonly IScreen screen;

        internal InGameTransform(IScreen screen)
        {
            this.screen = screen;

            Pan(Vector2.Zero);
        }

        public float Area { get; private set; } = DefaultInGameArea;

        /// <summary>
        /// Gets the size of the screen window in in-game units. 
        /// </summary>
        public Vector2 Size { get; private set; }

        public Vector2 Origin { get; private set; }

        /// <summary>
        /// Gets the camera center point in in-game coordinates. 
        /// </summary>
        public Vector2 Center { get; private set; }

        /// <summary>
        /// Gets the Game-to-pixel scaling factor,
        /// i.e. the size of an in-game tile in pixels.
        /// </summary>
        public Vector2 Scale { get; private set; }

        public RectangleF Bounds => new RectangleF(Origin, Size);

        /// <summary>
        /// Gets the render buffer-to-pixel scale.
        /// </summary>
        public float RenderScale { get; private set; }


        #region Editor-Specific

        public void ResetGameZoom()
        {
            Set(null, GetSize(DefaultInGameArea, (float)screen.WindowSize.X / screen.WindowSize.Y));
        }

        #endregion


        /// <summary>
        /// Moves the in-game camera to the specified center point.
        /// </summary>
        /// <param name="centerPoint"></param>
        public void Pan(Vector2 centerPoint)
            => Set(centerPoint, null);

        /// <summary>
        /// Updates the in-game area covered by this screen transform.
        /// </summary>
        /// <param name="newCenter">The new center, if it changed.</param>
        /// <param name="newSize">The new dimensions of the game window, if they changed.</param>
        public void Set(Vector2? newCenter, Vector2? newSize)
        {
            if (newCenter != null)
            {
                Center = newCenter.Value;
            }

            if (newSize != null)
            {
                Size = newSize.Value;
                Scale = screen.WindowSize / newSize.Value * RenderScale;
                Area = Size.X * Size.Y;
            }

            Origin = Center - Size / 2;
        }


        public void SetRenderScale(float renderScale)
        {
            Scale = screen.WindowSize / Size * renderScale;
        }

        public void SetAspectRatio(float aspectRatio)
        {
            Set(null, GetSize(Area, aspectRatio));
        }

        static Vector2 GetSize(float area, float aspectRatio)
        {
            var w = (float)Math.Sqrt(area * aspectRatio);
            return new Vector2(w, area / w);
        }

        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public Vector2 ToScreen(Vector2 position) => (position - Origin) * Scale;

        /// <summary>
        /// Gets a screen rectangle corresponding to the given in-game rectangle.
        /// </summary>
        public RectangleF ToScreen(RectangleF bounds) => (bounds - Origin) * Scale;

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public Vector2 FromScreen(Vector2 position) => (position / Scale) + Origin;

    }
}
