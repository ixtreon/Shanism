using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Shanism.Client
{
    /// <summary>
    /// Contains all the necessary information to display things on the screen. 
    /// Includes the window dimensions, as well as mappings to UI and in-game coordinates.
    /// </summary>
    public class Screen
    {
        static readonly Point DefaultSize = new Point(800, 600);
        public static int DefaultUiScale { get; } = Math.Min(DefaultSize.X, DefaultSize.Y) / 2;

        /// <summary>
        /// Gets the size of the screen window in in-game units. 
        /// </summary>
        public Vector GameSize { get; private set; } = Constants.Client.WindowSize;

        /// <summary>
        /// Gets the in-game bounds of the screen rectangle. 
        /// </summary>
        public RectangleF GameBounds { get; private set; }

        /// <summary>
        /// Gets the size of the screen window in pixels. 
        /// </summary>
        public Point Size { get; private set; } = DefaultSize;

        /// <summary>
        /// Gets half the size of the sreen window in pixels. 
        /// </summary>
        public Point HalfSize { get; private set; }

        /// <summary>
        /// Gets the font-to-pixel ratio, 
        /// which is defined as the curent Ui scale over the default one. 
        /// </summary>
        public double FontScale { get; private set; } = 1;

        /// <summary>
        /// Gets the UI-to-pixel scaling factor. 
        /// </summary>
        public double UiScale { get; private set; } = DefaultUiScale;

        public float RenderSize { get; private set; } = Settings.Current.RenderSize;

        /// <summary>
        /// Gets the Game-to-pixel scaling factor. 
        /// </summary>
        public Vector GameScale => Size / GameSize;

        /// <summary>
        /// Gets the size of the screen in UI units. 
        /// </summary>
        public Vector UiSize => (Vector)Size / UiScale;

        /// <summary>
        /// Gets the camera center point in in-game coordinates. 
        /// </summary>
        public Vector GameCenter => _pannedLocation;


        Vector _pannedLocation;

        public GraphicsDevice GraphicsDevice { get; }

        public Screen(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }


        public void Update()
        {
            //window size
            //var winSize = Window.ClientBounds.Size;
            //if(winSize.X != Size.X || winSize.Y != Size.Y)
            //    SetWindowSize(winSize.ToPoint());

            //render scale
            RenderSize = Settings.Current.RenderSize;
        }


        public void SetWindowSize(Point windowSz)
        {
            Size = windowSz;
            HalfSize = Size / 2;

            UiScale = Math.Min(Size.X, Size.Y) / 2;
            FontScale = UiScale / DefaultUiScale;
        }

        public void SetRenderSize(float renderSize)
        {
            RenderSize = renderSize;
        }


        public void MoveCamera(Vector inGameCenter)
        {
            _pannedLocation = inGameCenter;

            GameBounds = new RectangleF(GameCenter - GameSize / 2, GameSize);
        }

        public void MoveCamera(Vector inGameCenter, Vector inGameSz)
        {
            GameSize = inGameSz;
            _pannedLocation = inGameCenter;


            GameBounds = new RectangleF(GameCenter - GameSize / 2, GameSize);
        }

        #region Unit Conversions

        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public Vector GameToScreen(Vector p) 
            => ((p - GameCenter) * GameScale + HalfSize) * RenderSize;

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public Vector ScreenToGame(Vector position) 
            => (position / RenderSize - HalfSize) / GameScale + GameCenter;


        // TODO: stop being lazy
        /// <summary>
        /// Converts the given in-game point to Ui co-ordinates. 
        /// </summary>
        public Vector GameToUi(Vector v)
            => ScreenToUi(GameToScreen(v));

        public Vector UiToGame(Vector absolutePosition) 
            => ScreenToGame(UiToScreen(absolutePosition));

        /// <summary>
        /// Converts the given Ui point to screen co-ordinates.  
        /// </summary>
        public Vector UiToScreen(Vector p)
            => p * UiScale;

        /// <summary>
        /// Converts the given screen point to Ui co-ordinates.  
        /// </summary>
        public Vector ScreenToUi(Vector p) 
            => p / UiScale;

        /// <summary>
        /// Gets the UI size of the given screen size. 
        /// </summary>
        public double ScreenToUi(double p)
            => p / UiScale;

        /// <summary>
        /// Gets the UI size of the given screen size. 
        /// </summary>
        public double UiToScreen(double p)
            => p * UiScale;

        #endregion

    }
}
