using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shanism.Client
{
    /// <summary>
    /// Contains information about the current camera, screen and UI parameters. 
    /// </summary>
    static class Screen
    {
        static readonly Point DefaultSize = new Point(800, 600);
        static readonly int DefaultUiScale = Math.Min(DefaultSize.X, DefaultSize.Y) / 2;

        /// <summary>
        /// Gets the size of the screen window in in-game units. 
        /// </summary>
        public static Vector GameSize { get; private set; } = Constants.Client.WindowSize;

        /// <summary>
        /// Gets the in-game bounds of the screen rectangle. 
        /// </summary>
        public static RectangleF GameBounds { get; private set; }

        /// <summary>
        /// Gets the size of the screen window in pixels. 
        /// </summary>
        public static Point Size { get; private set; } = DefaultSize;

        /// <summary>
        /// Gets half the size of the sreen window in pixels. 
        /// </summary>
        public static Point HalfSize { get; private set; }

        /// <summary>
        /// Gets the font-to-pixel ratio, 
        /// which is defined as the curent Ui scale over the default one. 
        /// </summary>
        public static double FontScale { get; private set; } = 1;

        /// <summary>
        /// Gets the UI-to-pixel scaling factor. 
        /// </summary>
        public static double UiScale { get; private set; } = DefaultUiScale;

        public static float RenderSize { get; private set; } = Settings.Current.RenderSize;

        /// <summary>
        /// Gets the Game-to-pixel scaling factor. 
        /// </summary>
        public static Vector GameScale => Size / GameSize;

        /// <summary>
        /// Gets the size of the screen in UI units. 
        /// </summary>
        public static Vector UiSize => (Vector)Size / UiScale;

        /// <summary>
        /// Gets the camera center point in in-game coordinates. 
        /// </summary>
        public static Vector GameCenter => _pannedLocation;


        static Vector _pannedLocation;

        public static void SetWindowSize(Point windowSz)
        {
            Size = windowSz;
            HalfSize = Size / 2;

            UiScale = Math.Min(Size.X, Size.Y) / 2;
            FontScale = UiScale / DefaultUiScale;
        }

        public static void SetRenderSize(float renderSize)
        {
            RenderSize = renderSize;
            FontScale = UiScale / DefaultUiScale;
        }

        public static void MoveCamera(Vector inGameCenter)
        {
            _pannedLocation = inGameCenter;

            GameBounds = new RectangleF(GameCenter - GameSize / 2, GameSize);
        }

        public static void MoveCamera(Vector inGameCenter, Vector inGameSz)
        {
            GameSize = inGameSz;
            _pannedLocation = inGameCenter;


            GameBounds = new RectangleF(GameCenter - GameSize / 2, GameSize);
        }


        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public static Vector GameToScreen(Vector p) 
            => ((p - GameCenter) * GameScale + HalfSize) * RenderSize;

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public static Vector ScreenToGame(Vector position) 
            => (position / RenderSize - HalfSize) / GameScale + GameCenter;


        // TODO: stop being lazy
        /// <summary>
        /// Converts the given in-game point to Ui co-ordinates. 
        /// </summary>
        public static Vector GameToUi(Vector v)
            => ScreenToUi(GameToScreen(v));

        public static Vector UiToGame(Vector absolutePosition) 
            => ScreenToGame(UiToScreen(absolutePosition));

        /// <summary>
        /// Converts the given Ui point to screen co-ordinates.  
        /// </summary>
        public static Vector UiToScreen(Vector p)
            => p * UiScale;

        /// <summary>
        /// Converts the given screen point to Ui co-ordinates.  
        /// </summary>
        public static Vector ScreenToUi(Vector p) 
            => p / UiScale;

        /// <summary>
        /// Gets the UI size of the given screen size. 
        /// </summary>
        public static double ScreenToUiSize(double sz)
            => sz / UiScale;

        /// <summary>
        /// Gets the UI size of the given screen size. 
        /// </summary>
        public static Vector UiToScreenSize(Vector sz)
            => sz * UiScale;


    }
}
