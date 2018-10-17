using Microsoft.Xna.Framework.Input;
using Shanism.Client.Assets;
using Shanism.Client.IO;
using Shanism.Client.Models.Util;
using Shanism.Client.Text;
using Shanism.Common;
using System.Linq;
using System.Numerics;

using static System.Math;

namespace Shanism.Client.Systems
{
    /// <summary>
    /// Simulates a console inside the main game window. 
    /// Not working just yet...
    /// </summary>
    sealed class ConsoleSystem : ISystem
    {
        const float TotalHeight = 0.5f;
        const float SlideSpeed = 3;

        readonly LineWriter consoleOut;
        readonly ScreenSystem screen;
        readonly KeyboardSystem keyboard;
        readonly Font font;

        bool isVisible;
        float currentHeight;
        float targetHeight;

        public Keybind Keybind { get; set; } = new Keybind(Keys.OemTilde);

        public ConsoleSystem(IGameWindow window, FontCache fonts, LineWriter consoleOut)
        {
            this.consoleOut = consoleOut;
            this.screen = window.Screen;
            this.keyboard = window.Keyboard;

            font = fonts.NormalFont;
        }

        public void Update(int msElapsed)
        {
            // toggle visibility
            if (keyboard.JustPressed.Contains(Keybind))
            {
                isVisible = !isVisible;
                targetHeight = TotalHeight - targetHeight;
            }

            //slide up/down
            var dh = targetHeight - currentHeight;
            var dhAbs = Abs(dh);
            if (dhAbs > 1e-3)
                currentHeight += Sign(dh) * Min(dhAbs, SlideSpeed * msElapsed / 1000);

        }

        public void Draw(CanvasStarter painter)
        {
            if (currentHeight <= 0)
                return;

            var maxWidth = screen.UiSize.X;

            var y = currentHeight;
            using (var c = painter.BeginUI())
            {
                // background
                c.FillRectangle(Vector2.Zero, new Vector2(screen.UiSize.X, currentHeight), new Color(32, 32, 32));

                // current line
                drawLine(consoleOut.CurrentLine.ToString());


                var head = consoleOut.Lines.Last;
                while (y > 0 && head != null)
                {
                    drawLine(head.Value);
                    head = head.Previous;
                }


                void drawLine(string s)
                {
                    var sz = font.MeasureString(s, maxWidth);
                    c.DrawString(font, s, Color.White, new Vector2(0, y), maxWidth, AnchorPoint.BottomLeft);
                    y -= (sz.Y + font.Height / 12);
                }
            }

        }
    }
}
