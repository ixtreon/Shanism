using IO;
using IO.Common;
using IO.Objects;
using Microsoft.Xna.Framework.Graphics;
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


        /// <summary>
        /// Gets the UI-to-pixel scaling. 
        /// </summary>
        public static double UiScale { get; private set; } = DefaultUiScale;

        /// <summary>
        /// Gets the camera center point in in-game coordinates. 
        /// </summary>
        public static Vector CenterPoint { get; private set; }

        /// <summary>
        /// Gets the in-game size of the screen. 
        /// </summary>
        public static Vector InGameSize { get; private set; } = Constants.Client.WindowSize;


        /// <summary>
        /// Gets the screen size in pixels. 
        /// </summary>
        public static Point PixelSize { get; private set; } = new Point(800, 600);

        /// <summary>
        /// Gets the size of the screen in UI units. 
        /// </summary>
        public static Vector UiSize
        {
            get {  return (Vector)PixelSize / UiScale; }
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
        public static Point ScreenHalfSize
        {
            get { return PixelSize / 2; }
        }


        public static void SetCamera(Point? windowSz, Vector? cameraCenter = null, Vector? gameSz = null)
        {
            if (windowSz != null)
            {
                PixelSize = windowSz.Value;
                UiScale = Math.Min(PixelSize.X, PixelSize.Y) / 2;
            }

            CenterPoint = cameraCenter ?? CenterPoint;
            InGameSize = gameSz ?? InGameSize;
        }

        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public static Vector GameToScreen(Vector p)
        {
            return (p - CenterPoint) * PixelSize / InGameSize + ScreenHalfSize;
        }

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public static Vector ScreenToGame(Vector position)
        {
            return (position - ScreenHalfSize) *  InGameSize / PixelSize + CenterPoint;
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
