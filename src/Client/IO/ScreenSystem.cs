using Ix.Math;
using System;
using System.Numerics;

using GameWindow = Microsoft.Xna.Framework.GameWindow;

namespace Shanism.Client.IO
{

    interface IScreen
    {
        Point WindowSize { get; }

    }

    /// <summary>
    /// Information about the game screen and window. 
    /// Defines the window, in-game and UI coordinate spaces.
    /// </summary>
    public class ScreenSystem : ISystem, IScreen
    {
        readonly Func<Rectangle> GetWindowBounds;

        public InGameTransform Game { get; }

        public UiTransform UI { get; }


        /// <summary>
        /// Gets the width-to-height ratio of the game window
        /// and the UI coordinate system.
        /// </summary>
        public float AspectRatio { get; private set; }

        public Rectangle WindowBounds { get; private set; }

        /// <summary>
        /// Gets the size of the screen window in pixels. 
        /// </summary>
        public Point WindowSize => WindowBounds.Size;


        /// <summary>
        /// Gets the size of the screen in UI units. 
        /// </summary>
        [Obsolete]
        public Vector2 UiSize => UI.Size;


        public event Action WindowSizeChanged;


        public ScreenSystem(GameWindow window)
        {
            GetWindowBounds = () => window.ClientBounds.ToRect();

            Game = new InGameTransform(this);
            Game.SetRenderScale(1);

            UI = new UiTransform(this);

            UpdateWindowBounds();
        }


        public void Update(int msElapsed)
        {
            UpdateWindowBounds();
        }

        void UpdateWindowBounds() => SetWindowBounds(GetWindowBounds());

        /// <summary>
        /// Updates the size of the game window.
        /// </summary>
        void SetWindowBounds(Rectangle windowBounds)
        {
            if (windowBounds == WindowBounds || windowBounds.Size == Point.Zero)
                return;

            // self
            WindowBounds = windowBounds;
            AspectRatio = (float)windowBounds.Width / windowBounds.Height;

            // transforms
            UI.SetScale(Math.Min(windowBounds.Width, windowBounds.Height) / 2);
            Game.SetAspectRatio(AspectRatio);

            // event
            WindowSizeChanged?.Invoke();
        }

        /// <summary>
        /// Converts the given in-game point to Ui co-ordinates. 
        /// </summary>
        public Vector2 GameToUi(Vector2 p) => UI.FromScreen(Game.ToScreen(p));

        /// <summary>
        /// Converts the given UI point to in-game co-ordinates. 
        /// </summary>
        public Vector2 UiToGame(Vector2 p) => Game.FromScreen(UI.ToScreen(p));


    }
}
