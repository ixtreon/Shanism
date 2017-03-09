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
    abstract class GameScreen : GameComponent
    {
        public static bool ShowDebugStats { get; set; }

        double debugRenderTime;    //avg time per frame
        string debugString;


        protected Canvas Canvas { get; }
        
        public GameScreen(GameComponent game)
            : base(game)
        {
            Canvas = new Canvas(Screen);
        }

        /// <summary>
        /// Draws debug text, if <see cref="ShowDebugStats"/> is set to true.
        /// </summary>
        public virtual void Draw()
        {
            if (!ShowDebugStats) return;

            //draw debug
            Canvas.Begin();
            Canvas.DrawString(Content.Fonts.NormalFont, debugString, 
                Color.Goldenrod,
                new Vector(0.01), 0, 0);
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
            if (Keyboard.JustPressedKeys.Contains(Microsoft.Xna.Framework.Input.Keys.F12))
                ShowDebugStats ^= true;

            if (!ShowDebugStats)
                return;
            
            // FPS
            debugRenderTime = (1 - frameConst) * debugRenderTime + frameConst * msElapsed;
            var curFps = 1000 / debugRenderTime;

            // mouse
            var debugLines = new List<string>
            {
                $"FPS: {curFps:00}",

                $"Window Size: {Screen.Size}",

                $"Mouse: {Mouse.ScreenPosition}",
                $"Mouse UI: {Mouse.UiPosition:0.00}",
                $"Mouse InGame: {Mouse.InGamePosition:0.00}",

                $"UI Hover: {RootControl.GlobalHover?.GetType().Name}",
                $"UI Focus: {RootControl.GlobalFocus?.GetType().Name }",
            };

            writeDebugStats(debugLines);

            debugString = string.Join("\n", debugLines);
        }

        protected virtual void writeDebugStats(List<string> lines) { }
    }
}
