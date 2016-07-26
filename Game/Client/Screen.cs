﻿using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.StubObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common.Interfaces.Entities;

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
        public static Point Size { get; private set; } = new Point(800, 600);

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
        public static Vector InGameCenter => _lockedEntity?.Position ?? _inGameCenter;


        static Vector _inGameCenter;
        static IEntity _lockedEntity;



        public static void SetCamera(Point? windowSz,
            Vector? inGameSz = null,
            Vector? cameraCenter = null,
            IEntity lockedEntity = null)
        {
            if (windowSz.HasValue)
            {
                Size = windowSz.Value;
                HalfSize = Size / 2;

                UiScale = Math.Min(Size.X, Size.Y) / 2;
                FontScale = UiScale / DefaultUiScale;
            }

            if (lockedEntity != null)
                _lockedEntity = lockedEntity;
            else if (cameraCenter != null)
                _inGameCenter = cameraCenter.Value;

            if (inGameSz != null)
                GameSize = inGameSz.Value;
                
            GameBounds = new RectangleF(InGameCenter - GameSize / 2, GameSize);
        }

        /// <summary>
        /// Gets the screen co-ordinates of the given in-game point. 
        /// </summary>
        public static Vector GameToScreen(Vector p)
        {
            return (p - InGameCenter) * GameScale + HalfSize;
        }

        /// <summary>
        /// Converts the given screen point to in-game co-ordinates.  
        /// </summary>
        public static Vector ScreenToGame(Vector position)
        {
            return (position - HalfSize) / GameScale + InGameCenter;
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
            return HalfSize + p * UiScale;
        }



        /// <summary>
        /// Converts the given screen point to Ui co-ordinates.  
        /// </summary>
        public static Vector ScreenToUi(Vector p)
        {
            return (p - HalfSize) / UiScale;
        }

        /// <summary>
        /// Gets the UI size of the given screen size. 
        /// </summary>
        public static double ScreenToUi(double sz)
        {
            return sz / UiScale;
        }
    }
}
