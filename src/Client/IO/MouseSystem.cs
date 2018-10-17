using Microsoft.Xna.Framework.Input;
using Shanism.Common;
using System;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.IO
{
    public class MouseSystem : ISystem
    {
        static int ButtonsCount { get; } = Enum<MouseButton>.Values.Count;

        readonly IActivatable window;
        readonly ScreenSystem screen;

        MouseState mouseState;

        // buttons
        readonly bool[] buttonState = new bool[ButtonsCount];
        readonly bool[] oldButtonState = new bool[ButtonsCount];

        // cursor
        public Vector2 CursorSize { get; set; } = new Vector2(14, 26);

        public Vector2 ScreenPosition { get; private set; }
        public Vector2 InGamePosition { get; private set; }
        public Vector2 UiPosition { get; private set; }

        public Vector2 OldScreenPosition { get; private set; }
        public Vector2 OldInGamePosition { get; private set; }
        public Vector2 OldUiPosition { get; private set; }


        // scroll
        public float ScrollDelta { get; private set; }

        // other
        public bool IsDown(MouseButton button)
            => buttonState[(int)button];

        public bool IsJustPressed(MouseButton button)
            => !oldButtonState[(int)button] && buttonState[(int)button];

        public bool IsJustReleased(MouseButton button)
            => oldButtonState[(int)button] && !buttonState[(int)button];


        public bool LeftDown => IsDown(MouseButton.Left);
        public bool MiddleDown => IsDown(MouseButton.Middle);
        public bool RightDown => IsDown(MouseButton.Right);

        public bool LeftJustPressed => IsJustPressed(MouseButton.Left);
        public bool MiddleJustPressed => IsJustPressed(MouseButton.Middle);
        public bool RightJustPressed => IsJustPressed(MouseButton.Right);

        public bool LeftJustReleased => IsJustReleased(MouseButton.Left);
        public bool MiddleJustReleased => IsJustReleased(MouseButton.Middle);
        public bool RightJustReleased => IsJustReleased(MouseButton.Right);


        public bool MainButtonDown => LeftDown || MiddleDown || RightDown;
        public bool MainButtonJustPressed => LeftJustPressed || MiddleJustPressed || RightJustPressed;
        public bool MainButtonJustReleased => LeftJustReleased || MiddleJustReleased || RightJustReleased;

        public MouseSystem(IActivatable window, ScreenSystem screen)
        {
            this.window = window;
            this.screen = screen;
        }

        public void Update(int msElapsed)
        {
            if(!window.IsActive)
                return;

            var newMouseState = Mouse.GetState();

            updateButtons(newMouseState);
            updatePosition(newMouseState);
            updateScroll(mouseState, newMouseState);

            mouseState = newMouseState;

        }

        void updatePosition(MouseState newMouseState)
        {
            OldScreenPosition = ScreenPosition;
            OldInGamePosition = InGamePosition;
            OldUiPosition = UiPosition;

            ScreenPosition = new Vector2(newMouseState.X, newMouseState.Y);
            InGamePosition = screen.Game.FromScreen(ScreenPosition);
            UiPosition = screen.UI.FromScreen(ScreenPosition);
        }

        void updateButtons(MouseState newMouseState)
        {
            // cur -> old
            Array.Copy(buttonState, oldButtonState, ButtonsCount);

            // new -> cur
            buttonState[(int)MouseButton.Left]   = newMouseState.LeftButton == ButtonState.Pressed;
            buttonState[(int)MouseButton.Middle] = newMouseState.MiddleButton == ButtonState.Pressed;
            buttonState[(int)MouseButton.Right]  = newMouseState.RightButton == ButtonState.Pressed;
            buttonState[(int)MouseButton.Side1]  = newMouseState.XButton1 == ButtonState.Pressed;
            buttonState[(int)MouseButton.Side2]  = newMouseState.XButton2 == ButtonState.Pressed;
        }

        void updateScroll(MouseState oldMouseState, MouseState newMouseState)
        {
            const float Factor = -1f / 120;
            ScrollDelta = (newMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue) * Factor;
        }

    }
}
