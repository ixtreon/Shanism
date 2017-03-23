using Microsoft.Xna.Framework.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.Input
{
    public class MouseInfo
    {
        public static readonly Vector CursorSize = new Vector(14, 26);


        readonly Screen screen;

        MouseState oldMouseState;
        MouseState mouseState;


        public Vector ScreenPosition { get; private set; }
        public Vector InGamePosition { get; private set; }
        public Vector UiPosition { get; private set; }

        public Vector OldScreenPosition { get; private set; }
        public Vector OldInGamePosition { get; private set; }
        public Vector OldUiPosition { get; private set; }


        public bool LeftDown => mouseState.LeftButton == ButtonState.Pressed;
        public bool RightDown => mouseState.RightButton == ButtonState.Pressed;

        public bool LeftHeldDown => mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed;
        public bool RightHeldDown => mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed;
        public bool LeftJustPressed => mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
        public bool RightJustPressed => mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
        public bool LeftJustReleased => mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed;
        public bool RightJustReleased => mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed;


        public MouseInfo(Screen screen)
        {
            this.screen = screen;
        }


        public void Update(bool isActive)
        {
            if (!isActive) return;

            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            OldScreenPosition = ScreenPosition;
            OldInGamePosition = InGamePosition;
            OldUiPosition = UiPosition;

            ScreenPosition = new Vector(mouseState.X, mouseState.Y);
            InGamePosition = screen.ScreenToGame(ScreenPosition);
            UiPosition = screen.ScreenToUi(ScreenPosition);
        }
    }
}
