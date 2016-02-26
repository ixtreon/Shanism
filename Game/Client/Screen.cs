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
        /// Gets the in-game size of the screen. 
        /// </summary>
        public static Vector InGameSize { get; private set; } = Constants.Client.WindowSize;

        /// <summary>
        /// Gets the in-game bounds of the screen rectangle. 
        /// </summary>
        public static RectangleF InGameBounds { get; private set; }

        /// <summary>
        /// Gets the screen size in pixels. 
        /// </summary>
        public static Point ScreenSize { get; private set; } = new Point(800, 600);

        /// <summary>
        /// Gets half the size of the sreen in pixels. 
        /// </summary>
        public static Point ScreenHalfSize { get; private set; }

        /// <summary>
        /// Gets the font scale, which is simply the curent Ui scale over the default one. 
        /// </summary>
        public static double FontScale { get; private set; } = 1;


        /// <summary>
        /// Gets the size of the screen in UI units. 
        /// </summary>
        public static Vector UiSize => (Vector)ScreenSize / UiScale;

        /// <summary>
        /// Gets the camera center point in in-game coordinates. 
        /// </summary>
        public static Vector InGameCenter => _lockedEntity?.Position ?? _inGameCenter;


        static Vector _inGameCenter;
        static IEntity _lockedEntity;



        public static void SetCamera(Point? windowSz, 
            Vector? cameraCenter = null, 
            IEntity lockedEntity = null,
            Vector? inGameSz = null)
        {
            if (windowSz != null)
            {
                ScreenSize = windowSz.Value;
                ScreenHalfSize = ScreenSize / 2;

                UiScale = Math.Min(ScreenSize.X, ScreenSize.Y) / 2;
                FontScale = UiScale / DefaultUiScale;
            }

            if (lockedEntity != null)
                _lockedEntity = lockedEntity;
            else if (cameraCenter != null)
                _inGameCenter = cameraCenter.Value;

            InGameSize = inGameSz ?? InGameSize;
            InGameBounds = new RectangleF(InGameCenter - InGameSize / 2, InGameSize);
        }

        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public static Vector GameToScreen(Vector p)
        {
            return (p - InGameCenter) * ScreenSize / InGameSize + ScreenHalfSize;
        }

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public static Vector ScreenToGame(Vector position)
        {
            return (position - ScreenHalfSize) *  InGameSize / ScreenSize + InGameCenter;
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
