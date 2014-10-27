using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShanoRpgWinGl
{
    /// <summary>
    /// Contains information about the current camera, screen and UI parameters. 
    /// </summary>
    static class Screen
    {
        private const int DefaultUiScale = 240;

        private static Point screenSize;

        /// <summary>
        /// Gets the UI-to-pixel scaling. 
        /// </summary>
        public static double UiScale { get; private set; }

        /// <summary>
        /// Gets or sets the camera center point in game co-ordinates. 
        /// </summary>
        public static Vector2 CenterPoint { get; set; }

        /// <summary>
        /// Gets or sets the screen size in pixels and updates the UI scale. 
        /// </summary>
        public static Point ScreenSize
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
        /// Gets half the size of the sreen. 
        /// </summary>
        /// <returns></returns>
        public static Point ScreenHalfSize
        {
            get { return screenSize.DivideBy(2); }
        }

        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public static Point GameToScreen(Vector2 p)
        {
            return GameToScreen(p.X, p.Y);
        }

        /// <summary>
        /// Converts the given in-game point to Ui co-ordinates. 
        /// </summary>
        public static Vector2 GameToUi(Vector2 v)
        {
            // TODO: stop being lazy
            return ScreenToUi(GameToScreen(v));
        }


        /// <summary>
        /// Converts the given in-game point to screen co-ordinates.  
        /// </summary>
        public static Point GameToScreen(double x, double y)
        {
            return new Point(
                (int)((x - CenterPoint.X) * ScreenSize.X / Constants.Game.ScreenWidth) + ScreenHalfSize.X,
                (int)((y - CenterPoint.Y) * ScreenSize.Y / Constants.Game.ScreenHeight) + ScreenHalfSize.Y);
        }

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public static Vector2 ScreenToGame(Point position)
        {
            return new Vector2(
                ((float)position.X - ScreenHalfSize.X) * Constants.Game.ScreenWidth / ScreenSize.X + CenterPoint.X,
                ((float)position.Y - ScreenHalfSize.Y) * Constants.Game.ScreenHeight / ScreenSize.Y + CenterPoint.Y
                );
        }


        /// <summary>
        /// Converts the given Ui point to screen co-ordinates.  
        /// </summary>
        public static Point UiToScreen(Vector2 p)
        {
            return new Point(
                ScreenHalfSize.X + (int)(p.X * UiScale),
                ScreenHalfSize.Y + (int)(p.Y * UiScale));
        }

        /// <summary>
        /// Gets the screen size of the given Ui size. 
        /// </summary>
        public static int UiToScreen(double sz)
        {
            return (int)(sz * UiScale);
        }

        /// <summary>
        /// Converts the given screen point to Ui co-ordinates.  
        /// </summary>
        public static Vector2 ScreenToUi(Point p)
        {
            return new Vector2(
                (float)((p.X - ScreenHalfSize.X) / UiScale),
                (float)((p.Y - ScreenHalfSize.Y) / UiScale));
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
