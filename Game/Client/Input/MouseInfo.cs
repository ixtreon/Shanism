using Microsoft.Xna.Framework.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    static class MouseInfo
    {
        public static readonly Vector CursorSize = new Vector(14, 26);

        static MouseState oldMouseState;
        static MouseState mouseState;


        public static Vector ScreenPosition { get; private set; }
        public static Vector InGamePosition { get; private set; }
        public static Vector UiPosition { get; private set; }

        public static Vector OldScreenPosition { get; private set; }
        public static Vector OldInGamePosition { get; private set; }
        public static Vector OldUiPosition { get; private set; }


        public static bool LeftDown => mouseState.LeftButton == ButtonState.Pressed;
        public static bool RightDown => mouseState.RightButton == ButtonState.Pressed;

        public static bool LeftHeldDown => mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightHeldDown => mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed;
        public static bool LeftJustPressed => mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        public static bool RightJustPressed => mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        public static bool LeftJustReleased => mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        public static bool RightJustReleased => mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;


        public static void Update(int msElapsed)
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            OldScreenPosition = ScreenPosition;
            OldInGamePosition = InGamePosition;
            OldUiPosition = UiPosition;

            ScreenPosition = new Vector(mouseState.X, mouseState.Y);
            InGamePosition = Screen.ScreenToGame(ScreenPosition);
            UiPosition = Screen.ScreenToUi(ScreenPosition);
        }
    }
}
