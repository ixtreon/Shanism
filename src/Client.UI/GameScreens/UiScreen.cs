using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Input;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    
    public abstract class UiScreen : ShanoComponent
    {
        public static Color DefaultBackColor { get; } = new Color(48, 24, 48);

        public static bool ShowDebugStats { get; set; } = true;


        float debugRenderTime;    //avg time per frame

        string debugString;

        UiScreen subScreen;


        protected Canvas Canvas { get; }

        protected RootControl Root { get; }


        public event Action Closed;


        public Color BackColor
        {
            get { return Root.BackColor; }
            set { Root.BackColor = value; }
        }

        RootControl actualRoot => subScreen?.actualRoot ?? Root;

        TextureFont debugFont => Content.Fonts.NormalFont;



        public UiScreen(IShanoComponent game, RootControl root)
            : base(game)
        {
            Canvas = new Canvas(Screen);

            Root = root;
            Root.Add(new Tooltips.SimpleTip());
            Root.KeyPressed += root_KeyActivated;
        }

        public UiScreen(IShanoComponent game)
            : this(game, new RootControl
            {
                BackColor = DefaultBackColor,
                Size = game.Screen.UiSize,
            }) { }


        void root_KeyActivated(Keybind k)
        {
            if (k.Key == Keys.Escape)
            {
                Closed?.Invoke();
            }
        }

        public virtual void Draw()
        {
            Canvas.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.DepthRead,
                RasterizerState.CullNone);

            actualRoot.Draw(Canvas);
            DrawDebugStats();

            Canvas.End();
        }

        public void DrawDebugStats()
        {
            if (ShowDebugStats)
                Canvas.DrawString(debugFont, debugString, Color.Goldenrod, new Vector(0.01), 0, 0);
        }

        public virtual void Update(int msElapsed)
        {
            //fix up canvas
            Canvas.Bounds = new RectangleF(Vector.Zero, Screen.UiSize);

            //fix up root
            actualRoot.Maximize();
            actualRoot.UpdateMain(msElapsed);
            actualRoot.Update(msElapsed);

            updateDebugStats(msElapsed);
        }

        /// <summary>
        /// Sets the child screen.
        /// </summary>
        protected virtual void SetScreen(UiScreen newScreen)
        {
            subScreen = newScreen;
            subScreen.Closed += ResetSubScreen;
        }

        /// <summary>
        /// Resets the child screen.
        /// </summary>
        protected void ResetSubScreen()
            => subScreen = null;

        /// <summary>
        /// Run when the screen is shown.
        /// </summary>
        public virtual void Shown() { }


        void updateDebugStats(int msElapsed)
        {
            const float frameConst = 0.5f;

            //visibility
            if (Keyboard.JustPressedKeys.Contains(Microsoft.Xna.Framework.Input.Keys.F12))
                ShowDebugStats ^= true;

            if (!ShowDebugStats)
                return;

            // FPS
            debugRenderTime = Microsoft.Xna.Framework.MathHelper.
                Lerp(debugRenderTime, msElapsed, frameConst);

            // mouse
            var debugLines = new List<string>
            {
                $"{(1000 / debugRenderTime):000} FPS",

                $"Window Size: {Screen.Size}",

                $"Mouse: {Mouse.ScreenPosition}",
                $"Mouse UI: {Mouse.UiPosition:0.00}",
                $"Mouse InGame: {Mouse.InGamePosition:0.00}",

                $"Hover Control: {RootControl.GlobalHover?.GetType().Name}",
                $"Focus Control: {RootControl.GlobalFocus?.GetType().Name }",
            };

            WriteDebug(debugLines);

            debugString = string.Join("\n", debugLines);
        }

        protected virtual void WriteDebug(List<string> debugStats) { }
    }
}
