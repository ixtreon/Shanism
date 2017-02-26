using Microsoft.Xna.Framework.Graphics;
using Shanism.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Client.Input;
using Shanism.Common;

namespace Shanism.Client.GameScreens
{
    abstract class GameScreen
    {
        public static bool ShowDebugStats { get; set; }

        double timeToRender;    //avg time per frame
        string debugString;

        protected Canvas Canvas { get; }
        protected GraphicsDevice GraphicsDevice { get; }
        protected ContentList Content { get; }


        public GameScreen(GraphicsDevice device, ContentList content)
        {
            Canvas = new Canvas(device);
            GraphicsDevice = device;
            Content = content;
        }

        /// <summary>
        /// Draws debug text, if <see cref="ShowDebugStats"/> is set to true.
        /// </summary>
        public virtual void Draw()
        {
            if (!ShowDebugStats) return;

            //draw debug
            Canvas.Begin();
            Content.Fonts.NormalFont.DrawString(Canvas, debugString, Color.Goldenrod,
                new Vector(24, 18), 0, 0);
            Canvas.End();
        }

        /// <summary>
        /// Updates the debug text, if <see cref="ShowDebugStats"/> is set to true.
        /// </summary>
        /// <param name="msElapsed">The ms elapsed.</param>
        public virtual void Update(int msElapsed)
        {
            Canvas.Bounds = new RectangleF(Vector.Zero, Screen.UiSize);

            //update debug
            updateDebugStats(msElapsed);
        }

        public virtual void Shown() { }

        void updateDebugStats(int msElapsed)
        {
            const double frameConst = 0.5;

            //visibility
            if (KeyboardInfo.JustPressedKeys.Contains(Microsoft.Xna.Framework.Input.Keys.F12))
                ShowDebugStats ^= true;

            if (!ShowDebugStats)
                return;
            
            // FPS
            timeToRender = (1 - frameConst) * timeToRender + frameConst * msElapsed;
            var curFps = 1000 / timeToRender;

            // mouse
            var debugLines = new List<string>
            {
                $"FPS: {curFps:00}",

                $"Window Size: {Screen.Size}",

                $"Mouse: {MouseInfo.ScreenPosition}",
                $"Mouse UI: {MouseInfo.UiPosition:0.00}",
                $"Mouse InGame: {MouseInfo.InGamePosition:0.00}",

                $"UI Hover: {Control.HoverControl?.GetType().Name}",
                $"UI Focus: {Control.FocusControl?.GetType().Name }",
            };

            writeDebugStats(debugLines);

            debugString = string.Join("\n", debugLines);
        }

        protected virtual void writeDebugStats(List<string> lines) { }
    }
}
