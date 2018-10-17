using Ix.Math;
using Shanism.Client.Models.UI;
using Shanism.Client.IO;
using Shanism.Client.Systems;
using Shanism.Client.UI;
using Shanism.Client.UI.Containers;
using Shanism.Client.UI.Tooltips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using XnaMouse = Microsoft.Xna.Framework.Input.Mouse;
using Shanism.Client.Assets;

namespace Shanism.Client.Views
{

    /// <summary>
    /// The base class for any screen in the Shanism microcosm.
    /// </summary>
    public partial class View : Control
    {

        readonly Stack<Control> toUpdate = new Stack<Control>();

        protected IClient Client { get; private set; }
        public IViewContainer ViewStack { get; private set; }

        Control OldHoverControl { get; set; }

        public Control ViewHoverControl { get; private set; }
        public Control ViewFocusControl { get; private set; }


        MouseSystem Mouse => Client.Mouse;
        KeyboardSystem Keyboard => Client.Keyboard;
        RenderBuffer DrawBuffer => Client.DrawBuffer;
        protected ContentList DefaultContent => Client.DefaultContent;

        MouseEventDispatcher MouseEvents => new MouseEventDispatcher(Mouse);


        public View()
        {
            ViewFocusControl = ViewHoverControl = this;
        }

        /// <summary>
        /// Initializes the root control.
        /// </summary>
        public void Init(IViewContainer viewStack, IClient game)
        {
            if (ViewStack != null || Client != null)
                throw new InvalidOperationException("Game was already set...");

            ViewStack = viewStack;
            View = this;
            Client = game;
            Size = Screen.UiSize;

            OnInit();
            OnReload();
            OnShown(EventArgs.Empty);
        }

        public void AddTooltip<T>()
            where T : TooltipBase, new()
        {
            Add(new T { Mouse = Mouse });
        }


        public void Draw(CanvasStarter canvas)
        {
            // draw UI on the buffer, basically always
            DrawBuffer?.Apply(Screen.WindowSize, Screen.WindowSize);
            DrawUI(canvas);

            // do custom shit
            OnDraw(canvas);

            DrawBuffer?.DrawToScreen(canvas.SpriteBatch);
        }

        /// <summary>
        /// Custom draw etc.
        /// </summary>
        protected virtual void OnDraw(CanvasStarter canvas) { }

        /// <summary>
        /// Draws the user interface.
        /// </summary>
        protected void DrawUI(CanvasStarter canvas)
        {
            using (var c = canvas.BeginUI())
                drawInner(this, c);

        }

        static void drawInner(Control control, Canvas c)
        {
            control.Draw(c);

            var clientBounds = new RectangleF(Vector2.Zero, control.Size);
            foreach (var child in control.Controls)
                if (child.IsVisible && clientBounds.Intersects(child.Bounds))
                {
                    using (var childC = c.ZoomIn(child.Bounds))
                        drawInner(child, childC);
                }
        }

        protected override void OnKeyPress(KeyboardArgs e)
        {
            if (e.Modifiers.HasControl())
            {
                if (e.Key == Microsoft.Xna.Framework.Input.Keys.OemPlus)
                    Screen.UI.SetScale(Screen.UI.Scale * 1.05f);
                else if (e.Key == Microsoft.Xna.Framework.Input.Keys.OemMinus)
                    Screen.UI.SetScale(Screen.UI.Scale / 1.05f);
            }
            base.OnKeyPress(e);
        }

        bool shouldUpdateHover()
            => !Mouse.MainButtonDown;

        bool shouldUpdateFocus()
            => ViewHoverControl != null
            && Mouse.MainButtonJustPressed;


        public void DoUpdate(int msElapsed)
        {
            toUpdate.Push(this);

            do
            {
                var c = toUpdate.Pop();
                // self
                c.Update(msElapsed);
                // children
                foreach (var cc in c.Controls)
                    toUpdate.Push(cc);
            }
            while (toUpdate.Any());
        }


        /// <summary>
        /// Updates the user interface and all controls.
        /// </summary>
        public override void Update(int msElapsed)
        {
            Bounds = Screen.UI.Bounds;

            // update hover
            OldHoverControl = ViewHoverControl;
            if (shouldUpdateHover())
                ViewHoverControl = findHover();

            // raise mouse events
            MouseEvents.RaiseUiEvents(OldHoverControl, ViewHoverControl);

            // update focus
            if (shouldUpdateFocus())
            {
                var c = ViewHoverControl;
                while (!c.CanFocus && c.Parent != null)
                    c = c.Parent;
                SetFocus(c);
            }

            // update UI cursor
            XnaMouse.SetCursor(ViewHoverControl.Cursor.GetCursorObject());

            // raise UI keyboard events
            {
                var focus = ViewFocusControl;

                foreach (var k in Keyboard.JustReleased)
                    OnKeyRelease(focus, k);

                foreach (var k in Keyboard.JustPressed)
                    OnKeyPress(focus, k);

                if (Keyboard.JustTypedCharacter != null)
                    OnCharacterInput(focus, Keyboard.JustTypedCharacter.Value);

                foreach (var a in Keyboard.JustActivated)
                    OnActionActivated(focus, a);
            }
        }

        internal void SetFocus(Control c)
        {
            ViewFocusControl.InvokeFocusLost();

            ViewFocusControl = c ?? throw new ArgumentNullException(nameof(c));

            if (c is Window)
                c.BringToFront();

            ViewFocusControl.InvokeFocusGained();
        }


        Control findHover()
        {
            return tryFindHover(this, Mouse.UiPosition - Location, out var h) ? h : this;
        }

        static bool tryFindHover(Control c, Vector2 p, out Control hover)
        {
            // search child controls first
            // access by descending zorder
            for (int i = c.Controls.Count - 1; i >= 0; i--)
            {
                var cc = c.Controls[i];
                if (cc.IsVisible && p.IsInside(cc.Bounds))
                    if (tryFindHover(cc, p - cc.Location, out hover))
                        return true;
            }

            hover = c.CanHover ? c : null;
            return c.CanHover;
        }


        public void ShowMessageBox(string caption, string text, bool suppressOtherMessages = false)
        {
            if (suppressOtherMessages)
                RemoveWhere(c => c is MessageBox);

            Add(new MessageBox(caption, text)
            {
                IsVisible = true,
                RemoveOnButtonClick = true
            });
        }

        public async Task<MessageBoxButtons> ShowMessagePrompt(string caption, string text, MessageBoxButtons buttons,
            bool suppressOtherMessages = false)
        {

            if (suppressOtherMessages)
                RemoveWhere(c => c is MessageBox);

            var mBox = new MessageBox(caption, text, buttons)
            {
                IsVisible = true,
                RemoveOnButtonClick = true
            };
            Add(mBox);

            return await mBox.GetResultAsync();
        }

        MessageBox CreateBox(string title, string text, MessageBoxButtons buttons)
        {
            var box = new MessageBox(title, text, buttons)
            {
                IsVisible = true,
                RemoveOnButtonClick = true
            };

            // do this separately, after box has been created & sized, lol
            box.Location = Vector2.Max(Vector2.Zero, Size - box.Size) / 2;

            return box;
        }
    }
}
