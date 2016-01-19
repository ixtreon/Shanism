using IO;
using IO.Common;
using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    /// <summary>
    /// Contains information about the current camera, screen and UI parameters. 
    /// </summary>
    static class Screen
    {
        const int DefaultUiScale = 240;



        static Point screenSize = new Point(800, 600);

        /// <summary>
        /// Gets the UI-to-pixel scaling. 
        /// </summary>
        public static double UiScale { get; private set; } = DefaultUiScale;

        /// <summary>
        /// Gets or sets the camera center point in in-game coordinates. 
        /// </summary>
        public static Vector CenterPoint { get; set; }

        /// <summary>
        /// Gets whether the camera is locked to the center. NYI. 
        /// </summary>
        public static bool IsLocked { get; private set; }

        /// <summary>
        /// Gets or sets the screen size in pixels and updates the UI scale. 
        /// </summary>
        public static Point Size
        {
            get { return screenSize; }
            set
            {
                screenSize = value;
                UiScale = Math.Min(ScreenHalfSize.X, ScreenHalfSize.Y);
            }
        }

        /// <summary>
        /// Gets the font scale, which is simply the curent Ui scale over the default one. 
        /// </summary>
        public static double FontScale
        {
            get { return UiScale / DefaultUiScale; }
        }

        /// <summary>
        /// Gets half the size of the sreen in pixels. 
        /// </summary>
        /// <returns></returns>
        public static Point ScreenHalfSize
        {
            get { return screenSize / 2; }
        }



        public static void Update(Microsoft.Xna.Framework.GraphicsDeviceManager graphics, IHero hero)
        {
            var hasHero = (hero != null);
            CenterPoint = hero?.Position ?? Vector.Zero;

            IsLocked = hasHero;

            Size = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }


        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public static Vector GameToScreen(Vector p)
        {
            return (p - CenterPoint) * Size / Constants.Client.WindowSize + ScreenHalfSize;
        }

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public static Vector ScreenToGame(Vector position)
        {
            return (position - ScreenHalfSize) * Constants.Client.WindowSize / Size + CenterPoint;
        }


        /// <summary>
        /// Converts the given in-game point to Ui co-ordinates. 
        /// </summary>
        public static Vector GameToUi(Vector v)
        {
            // TODO: stop being lazy
            return ScreenToUi(GameToScreen(v));
        }

        public static Vector UiToGame(Vector absolutePosition)
        {
            return ScreenToGame(UiToScreen(absolutePosition));
        }


        /// <summary>
        /// Converts the given Ui point to screen co-ordinates.  
        /// </summary>
        public static Vector UiToScreen(Vector p)
        {
            return ScreenHalfSize + p * UiScale;
        }



        /// <summary>
        /// Converts the given screen point to Ui co-ordinates.  
        /// </summary>
        public static Vector ScreenToUi(Vector p)
        {
            return (p - ScreenHalfSize) / UiScale;
        }

        /// <summary>
        /// Gets the UI size of the given screen size. 
        /// </summary>
        public static double ScreenToUi(int sz)
        {
            return (double)sz / UiScale;
        }
    }
}
