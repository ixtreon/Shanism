using Microsoft.Xna.Framework.Input;
using Shanism.Client.IO;
using Shanism.Client.Text;
using Shanism.Common;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Displays debug information.
    /// </summary>
    class DebugTextSystem : ISystem
    {

        readonly CounterF updateLimiter;
        readonly IHazDebug source;

        readonly KeyboardSystem keyboard;
        readonly MouseSystem mouse;
        readonly ScreenSystem screen;

        readonly Font font;

        bool isVisible;
        string text;

        public Keybind Keybind { get; set; } = new Keybind(Keys.F3);

        public float UpdateInterval
        {
            get => updateLimiter.MaxValue;
            set => updateLimiter.Reset(value);
        }


        public DebugTextSystem(IGameWindow window, FontCache fonts, IHazDebug source, int fps = 4)
        {
            this.source = source;
            this.keyboard = window.Keyboard;
            this.mouse = window.Mouse;
            this.screen = window.Screen;

            font = fonts.NormalFont;
            updateLimiter = new CounterF(1000f / fps);
        }

        public void Update(int msElapsed)
        {
            // visibility
            if (keyboard.JustPressed.Contains(Keybind))
                isVisible ^= true;

            var fps = GetAverageFps(msElapsed);

            if (!isVisible)
                return;

            if (!updateLimiter.Tick(msElapsed))
                return;

            // common stuff
            var lines = new List<string>
            {
                $"{screen.WindowSize.X} x {screen.WindowSize.Y}, {fps:0000} FPS",
                "",

                "Mouse",
                $"  Screen: {mouse.ScreenPosition:0}",
                $"  UI: {mouse.UiPosition:0.00}",
                $"  InGame: {mouse.InGamePosition:0.00}",
                "",
            };

            source.WriteDebug(lines);


            text = string.Join("\n", lines);
        }

        public void Draw(CanvasStarter painter)
        {
            if (!isVisible)
                return;

            using (var c = painter.BeginUI())
            {
                var textPosition = new Vector2(0.01f);
                var textSize = font.MeasureString(text);

                c.FillRectangle(textPosition, textSize, Color.Black.SetAlpha(50));
                c.DrawString(font, text, textPosition, ColorScheme.Current.TextTitle);
            }
        }

        #region FPS Counter

        const float FpsUpdateInterval = 125;

        float fpsCurrent;
        float fpsFrameCount;
        float fpsElapsed;

        float GetAverageFps(float msElapsed)
        {
            fpsFrameCount++;
            fpsElapsed += msElapsed;

            if (fpsElapsed > FpsUpdateInterval)
            {
                fpsCurrent = fpsFrameCount * 1000 / FpsUpdateInterval;

                fpsFrameCount = 0;
                fpsElapsed = 0;
            }

            return fpsCurrent;
        }

        #endregion
    }

    interface IHazDebug
    {
        void WriteDebug(List<string> linesSoFar);
    }
}
