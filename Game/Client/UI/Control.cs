using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
using Shanism.Common.Game;
using Color = Microsoft.Xna.Framework.Color;
using Shanism.Client.Input;

namespace Shanism.Client.UI
{
    [Flags]
    public enum AnchorMode
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 4,
        Bottom = 8,
        All = Left | Right | Bottom | Top,
    }

    /// <summary>
    /// Represents a user interface control. 
    /// </summary>
    class Control
    {
        #region Static/Const members


        /// <summary>
        /// Specifies the default distance between elements in UI scale. 
        /// </summary>
        public const double Padding = 0.01;


        ///// <summary>
        ///// Gets the keyboard state. 
        ///// </summary>
        //protected static KeyboardState
        //    oldKeyboardState = Keyboard.GetState(),
        //    keyboardState = Keyboard.GetState();

        /// <summary>
        /// Gets the mouse state. 
        /// </summary>
        protected static MouseState
            oldMouseState = Mouse.GetState(),
            mouseState = Mouse.GetState();

        /// <summary>
        /// Gets the first control that is under the mouse pointer
        /// and has <see cref="CanHover"/> set to <c>true</c>. 
        /// </summary>
        internal static Control HoverControl { get; private set; }   //todo: make protected

        /// <summary>
        /// Gets the control that currently has keyboard focus. 
        /// A control must have its <see cref="CanFocus"/> property set to <c>true</c> in order to become the <see cref="FocusControl"/>.
        /// </summary>
        internal static Control FocusControl { get; private set; }

        #endregion



        bool _isVisible = true;

        Vector _size;

        /// <summary>
        /// The list of child controls. 
        /// </summary>
        protected readonly List<Control> controls = new List<Control>();


        #region Public Properties

        /// <summary>
        /// Gets the parent of this control. 
        /// </summary>
        public Control Parent { get; private set; }

        /// <summary>
        /// Defines the background color drawn behind the whole control. 
        /// </summary>
        public Color BackColor { get; set; } = Color.Transparent;

        /// <summary>
        /// Gets or sets whether the control is draggable. 
        /// </summary>
        public bool CanDrag { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the control can take input focus. 
        /// </summary>
        public bool CanFocus { get; set; } = false;

        /// <summary>
        /// Gets or sets whether this control responds to mouse events. 
        /// </summary>
        public bool CanHover { get; set; } = true;

        /// <summary>
        /// Gets or sets the tooltip text of this control. 
        /// </summary>
        public object ToolTip { get; set; }

        /// <summary>
        /// Gets the Z-order of the control. 
        /// </summary>
        public double ZOrder { get; set; }

        /// <summary>
        /// Gets the sticky sides in relation to the parent control. 
        /// </summary>
        public AnchorMode ParentAnchor { get; set; } = AnchorMode.Left | AnchorMode.Top;

        /// <summary>
        /// Gets or sets the position of this control in its parent's coordinate space. 
        /// </summary>
        public Vector Location { get; set; }

        #endregion


        #region Events
        /// <summary>
        /// Raised whenever this control receives a drag-drop from another control. 
        /// </summary>
        public event Action<Control> OnDrop;

        /// <summary>
        /// Raised whenever this control is drag-dropped onto another control. 
        /// </summary>
        public event Action<Control> OnDrag;

        /// <summary>
        /// Raised whenever the mouse enters the control's boundary. 
        /// </summary>
        public event Action<MouseArgs> MouseEnter;

        /// <summary>
        /// Raised whenever the mouse leaves the control's boundary. 
        /// </summary>
        public event Action<MouseArgs> MouseLeave;

        /// <summary>
        /// Raised whenever the mouse moves over the control's boundary. 
        /// </summary>
        public event Action<MouseArgs> MouseMove;

        /// <summary>
        /// Raised whenever a mouse button is pressed on this control. 
        /// </summary>
        public event Action<MouseButtonArgs> MouseDown;

        /// <summary>
        /// Raised whenever a mouse button is released while previously pressed on the control.
        /// </summary>
        public event Action<MouseButtonArgs> MouseUp;

        /// <summary>
        /// Raised whenever the control's visibility changes. 
        /// </summary>
        public event Action<Control> VisibleChanged;

        /// <summary>
        /// The event whenever a game action (a key and zero or more modifier keys) 
        /// is activated (pressed or released as per <see cref="Settings.QuickButtonPress"/>)
        /// while this control has focus (see <see cref="FocusControl"/>. 
        /// </summary>
        public event Action<ClientAction> GameActionActivated;

        public event Action<Keybind> KeyPressed;

        #endregion


        /// <summary>
        /// Gets or sets the absolute position of this control in UI coordinates. 
        /// Calculated using the parent's absolute position. 
        /// </summary>
        public Vector AbsolutePosition
        {
            get { return ParentAbsolutePosition + Location; }
            set { Location = value - ParentAbsolutePosition; }
        }

        /// <summary>
        /// Gets or sets whether the control is visible. 
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value != _isVisible)
                {
                    _isVisible = value;
                    VisibleChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the control in UI coordinates. 
        /// </summary>
        public Vector Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    var d = value - _size;
                    _size = value;

                    resizeChildren(AnchorMode.Left, AnchorMode.Right, new Vector(d.X, 0));
                    resizeChildren(AnchorMode.Top, AnchorMode.Bottom, new Vector(0, d.Y));
                }

            }
        }


        /// <summary>
        /// Gets all the children of this control. 
        /// </summary>
        public IEnumerable<Control> Controls => controls;

        /// <summary>
        /// Gets whether this control is the currently focused control.
        /// </summary>
        public bool HasFocus => FocusControl == this;

        /// <summary>
        /// Gets whether this control is the root (top-level) control. 
        /// </summary>
        public bool IsRootControl => Parent == null;



        #region Control Bounds
        /// <summary>
        /// Gets the top (low Y) coordinate of this control relative to its parent. 
        /// </summary>
        public double Top => Location.Y;

        /// <summary>
        /// Gets the left X coordinate of this control relative to its parent. 
        /// </summary>
        public double Left => Location.X;

        /// <summary>
        /// Gets the bottom (high Y) coordinate of this control relative to its parent. 
        /// </summary>
        public double Bottom => Location.Y + Size.Y;


        /// <summary>
        /// Gets the right X coordinate of this control relative to its parent. 
        /// </summary>
        public double Right => Location.X + Size.X;

        /// <summary>
        /// Gets whether the mouse is currently over this control. 
        /// </summary>
        public bool MouseOver => (HoverControl == this);


        /// <summary>
        /// Gets or sets the screen position of this control, in pixels. 
        /// </summary>
        public Vector ScreenLocation
        {
            get { return Screen.UiToScreen(Location); }
            set { AbsolutePosition = Screen.ScreenToUi(value); }
        }

        /// <summary>
        /// Gets the screen size of this control, in pixels. 
        /// </summary>
        public Point ScreenSize
        {
            get { return (Size * Screen.UiScale).ToPoint(); }
        }
        #endregion


        #region Parent Bounds
        /// <summary>
        /// Gets the absolute position of this control's parent, 
        /// or <see cref="Vector.Zero"/> if this control has no parent. 
        /// </summary>
        private Vector ParentAbsolutePosition
        {
            get { return Parent?.AbsolutePosition ?? Vector.Zero; }
        }

        /// <summary>
        /// Gets the size of this control's parent. 
        /// </summary>
        /// <returns></returns>
        private Vector ParentSize
        {
            get { return Parent?.Size ?? Vector.Zero; }
        }
        #endregion



        /// <summary>
        /// Adds the specified control as a child of this control.
        /// </summary>
        /// <param name="c">The control that is to be added. Cannot be null.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Add(Control c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));

            controls.Add(c);

            c.Parent = this;
            c.BringToFront();
        }

        /// <summary>
        /// Removes the given child control. 
        /// </summary>
        /// <param name="c"></param>
        public bool Remove(Control c) => controls.Remove(c);


        public void Show() => IsVisible = true;

        public void Hide() => IsVisible = false;

        public void ToggleVisible() => IsVisible = !IsVisible;


        public void ActivateAction(ClientAction act) => GameActionActivated?.Invoke(act);

        public void PressKey(Keybind k) => KeyPressed?.Invoke(k);


        /// <summary>
        /// Brings thie control to the front of the z-order. 
        /// </summary>
        public void BringToFront()
        {
            if (Parent != null && Parent.controls.Count > 1)
            {
                var maxZ = Parent.controls.Where(c => c != this).Max(c => c.ZOrder);
                if (maxZ >= ZOrder)
                    ZOrder = maxZ + 1;
            }
            else
                ZOrder = 0;
        }

        /// <summary>
        /// Returns whether the given point lies within this control. 
        /// </summary>
        /// <param name="p">The specified point, in UI coordinates. </param>
        /// <returns>True if the point is within this control. False otherwise. </returns>
        public bool Contains(Vector p)
        {
            var localP = p - AbsolutePosition;
            var inside = localP.X >= 0 && localP.Y >= 0 && localP.X < Size.X && localP.Y < Size.Y;
            return inside;
        }

        public void Draw(SpriteBatch sb) => Draw(new Graphics(sb));

        public void Draw(Graphics parentGraphics)
        {
            if (IsVisible)
            {
                var g = new Graphics(parentGraphics, Location, Size);

                OnDraw(g);

                // draw controls
                // sort by z order - lower is first
                foreach (var c in controls.OrderBy(c => c.ZOrder))
                    c.Draw(g);
            }
        }

        /// <summary>
        /// Updates the state of the control and recursively calls update for all child controls. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(int msElapsed)
        {
            //update self
            OnUpdate(msElapsed);

            //update controls
            foreach (var c in controls)
                c.Update(msElapsed);
        }

        /// <summary>
        /// Draws a background over the whole control. 
        /// </summary>
        /// <param name="sb"></param>
        public virtual void OnDraw(Graphics g)
        {
            if (BackColor.A > 0)
                g.Draw(Content.Textures.Blank, Vector.Zero, Size, BackColor);
        }

        /// <summary>
        /// Override in derived classes to implement custom Update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        protected virtual void OnUpdate(int msElapsed) { }


        void resizeChildren(AnchorMode min, AnchorMode max, Vector d)
        {
            foreach (var c in controls)
            {
                var hasMin = c.ParentAnchor.HasFlag(min);
                var hasMax = c.ParentAnchor.HasFlag(max);

                if (hasMax && hasMin)    //anchor both sides -> modify size
                    c.Size += d;
                else if (hasMax && !hasMin)  // anchor at max (right/top) -> modify loc
                    c.Location += d;
                else if (!hasMin && !hasMax) // has no anchor -> float in center
                    c.Location += d / 2;
                //else if hasMin, !hasMax -> don't touch
            }
        }

        Control recalcHoverControl()
        {
            //search child controls first
            //order by reverse z - higher is first
            var childHover = controls
                .Where(c => c.IsVisible)
                .OrderBy(c => -c.ZOrder)
                .Select(c => c.recalcHoverControl())
                .Where(c => c != null)
                .FirstOrDefault();

            if (childHover != null)
                return childHover;

            if (CanHover && Contains(Screen.ScreenToUi(mouseState.Position.ToPoint())))
                return this;

            return null;
        }

        #region Mouse key helpers
        bool holdDownLeft { get { return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed; } }
        bool holdDownRight { get { return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed; } }
        bool justPressedLeft { get { return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released; } }
        bool justPressedRight { get { return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released; } }
        bool justReleasedLeft { get { return mouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed; } }
        bool justReleasedRight { get { return mouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed; } }
        #endregion

        internal void UpdateMain(int msElapsed)
        {
            //update the static keyboard/mouse info

            if (FocusControl == null)
                FocusControl = this;

            raiseMoveEvents();

            raiseKeyboardEvents();
        }

        void raiseMoveEvents()
        {
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            var mPos = Screen.ScreenToUi(mouseState.Position.ToPoint());
            var oldMPos = Screen.ScreenToUi(oldMouseState.Position.ToPoint());

            //check drag-drop start before updating hover
            var oldHover = HoverControl ?? this;
            if (!holdDownLeft)
                HoverControl = recalcHoverControl() ?? this;

            //mouse move
            if (mPos != oldMPos)
                HoverControl.MouseMove?.Invoke(new MouseArgs(HoverControl, mPos));

            //left click
            if (justPressedLeft)
                HoverControl.MouseDown?
                    .Invoke(new MouseButtonArgs(HoverControl, mPos, MouseButton.Left));
            else if (justReleasedLeft)
                HoverControl.MouseUp?
                    .Invoke(new MouseButtonArgs(HoverControl, mPos, MouseButton.Left));

            //right click
            if (justPressedRight)
                HoverControl.MouseDown?
                    .Invoke(new MouseButtonArgs(HoverControl, mPos, MouseButton.Right));
            else if (justReleasedRight)
                HoverControl.MouseUp?
                    .Invoke(new MouseButtonArgs(HoverControl, mPos, MouseButton.Right));

            //focus control
            if (justPressedLeft || justPressedRight)
            {
                var c = HoverControl;
                while (!c.CanFocus && c.Parent != null)
                    c = c.Parent;
                c.SetFocus();
            }

            //hover change
            if (HoverControl != oldHover)
            {
                //leave old control
                oldHover.MouseLeave?.Invoke(new MouseArgs(HoverControl, mPos));

                // if we also released the button this frame and the source 
                // supports drag-drop, raise the drag-drop events
                //
                // hover is kept when mouse is down 
                // so that's what a drag-drop is
                if (justReleasedLeft && oldHover.CanDrag)
                {
                    oldHover.OnDrag?.Invoke(HoverControl);
                    HoverControl.OnDrop?.Invoke(oldHover);
                }

                //enter new control
                HoverControl.MouseEnter?.Invoke(new MouseArgs(HoverControl, mPos));
            }
        }

        void raiseKeyboardEvents()
        {
            var focus = FocusControl;
            if (focus != null)
            {
                //order is important: 
                // keys first so action.chat 
                // doesnt activate chat.enterkey which closes chat bar
                foreach (var k in KeyboardInfo.JustPressedKeys)
                    if (!k.IsModifier())
                        focus.KeyPressed?.Invoke(new Keybind(KeyboardInfo.Modifiers, k));

                foreach (var ga in KeyboardInfo.JustActivatedActions)
                    focus.GameActionActivated?.Invoke(ga);
            }
        }


        /// <summary>
        /// Makes the control span the whole game window. 
        /// </summary>
        public void Maximize()
        {
            //span the whole window
            var min = Screen.ScreenToUi(Point.Zero);
            var max = Screen.ScreenToUi(new Point(Screen.Size.X, Screen.Size.Y));

            AbsolutePosition = min;   //use the lowercase field so we don't move children..
            Size = max - min;
        }

        /// <summary>
        /// Makes this control the currently focused control, only if <see cref="CanFocus"/> is true. 
        /// </summary>
        public void SetFocus()
        {
            if (CanFocus)
                FocusControl = this;
        }

        public void ClearFocus()
        {
            if (FocusControl != this)
                return;

            var c = Parent;
            while (c != null && !c.CanFocus)
                c = c.Parent;
            FocusControl = c;
        }
    }
}
