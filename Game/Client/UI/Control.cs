using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shanism.Common;
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

        public const double LargePadding = 0.02;


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

        internal static ContentList Content { get; private set; }


        public static void SetContent(ContentList content)
        {
            Content = content;
        }

        #endregion


        /// <summary>
        /// The list of child controls in a back-to-front order. 
        /// </summary>
        readonly List<Control> controls = new List<Control>();

        bool _isVisible = true;
        Vector _size;
        Vector _location;


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
        /// Gets or sets the tooltip of this control. 
        /// </summary>
        public object ToolTip { get; set; }

        /// <summary>
        /// Gets or sets the sticky sides in relation to the parent control. 
        /// </summary>
        public AnchorMode ParentAnchor { get; set; } = AnchorMode.Left | AnchorMode.Top;


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
        /// Gets or sets the position of this control in its parent's coordinate space. 
        /// </summary>
        public Vector Location
        {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// Gets or sets the size of the control in UI units. 
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

                    SizeChanged?.Invoke(this);
                }
            }
        }

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
        /// Raised whenever the control's size changes. 
        /// </summary>
        public event Action<Control> SizeChanged;

        /// <summary>
        /// The event whenever a game action (a key plus/minus some modifier keys) 
        /// is activated (pressed or released as per <see cref="Settings.QuickButtonPress"/>)
        /// while this control has focus (see <see cref="FocusControl"/>. 
        /// </summary>
        public event Action<ClientAction> GameActionActivated;

        public event Action<Keybind> KeyPressed;

        public event Action<Keybind> KeyReleased;

        public event Action<Control> ControlAdded;

        public event Action<Control> ControlRemoved;

        #endregion


        #region Property Shortcuts

        /// <summary>
        /// Gets all the children of this control. 
        /// </summary>
        public IReadOnlyList<Control> Controls => controls;

        /// <summary>
        /// Gets the bounds of this control relative to its parent's coordinate space.
        /// </summary>
        public RectangleF Bounds => new RectangleF(_location, Size);

        /// <summary>
        /// Gets or sets the width of this control.
        /// </summary>
        public double Width
        {
            get { return Size.X; }
            set { Size = new Vector(value, Size.Y); }
        }

        /// <summary>
        /// Gets or sets the height of this control.
        /// </summary>
        public double Height
        {
            get { return Size.Y; }
            set { Size = new Vector(Size.X, value); }
        }

        /// <summary>
        /// Gets the top (low Y) coordinate of this control relative to its parent. 
        /// </summary>
        public double Top
        {
            get { return _location.Y; }
            set { _location.Y = value; }
        }

        /// <summary>
        /// Gets the left X coordinate of this control relative to its parent. 
        /// </summary>
        public double Left
        {
            get { return _location.X; }
            set { _location.X = value; }
        }

        /// <summary>
        /// Gets the bottom (high Y) coordinate of this control relative to its parent. 
        /// </summary>
        public double Bottom
        {
            get { return _location.Y + Size.Y; }
            set { _location.Y = value - Size.Y; }
        }

        /// <summary>
        /// Gets the right X coordinate of this control relative to its parent. 
        /// </summary>
        public double Right
        {
            get { return _location.X + Size.X; }
            set { _location.X = value - Size.X; }
        }

        /// <summary>
        /// Gets whether this control is the currently focused control (see <see cref="FocusControl"/>).
        /// </summary>
        public bool HasFocus => FocusControl == this;

        /// <summary>
        /// Gets whether the mouse is currently over this control (see <see cref="HoverControl"/>).
        /// </summary>
        public bool HasHover => (HoverControl == this);

        /// <summary>
        /// Gets whether this control is a root (top-level) control. 
        /// </summary>
        public bool IsRootControl => Parent == null;

        /// <summary>
        /// Calculates the absolute position of this control in UI coordinates. 
        /// </summary>
        public Vector AbsolutePosition
            => (Parent?.AbsolutePosition ?? Vector.Zero) + _location;

        #endregion


        #region Child Controls Methods

        /// <summary>
        /// Adds the specified control as a child of this control.
        /// </summary>
        /// <param name="c">The control that is to be added. Cannot be null.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Add(Control c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (c.Parent != null) throw new ArgumentException("This control is already a child of another control.");

            c.Parent = this;

            controls.Add(c);
            OnControlAdded(c);
            ControlAdded?.Invoke(c);
        }

        public void AddRange(IEnumerable<Control> controls)
        {
            if (controls == null) throw new ArgumentNullException(nameof(controls));

            foreach (var c in controls)
                Add(c);
        }


        /// <summary>
        /// Removes the given child control. 
        /// </summary>
        /// <param name="c"></param>
        public void Remove(Control c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (c.Parent != this) throw new ArgumentException("The specified control is not a child of this control.");

            c.Parent = null;

            controls.Remove(c);
            OnControlRemoved(c);
            ControlRemoved?.Invoke(c);
        }

        /// <summary>
        /// Removes all child controls.
        /// </summary>
        public void RemoveAll() => controls.Clear();

        #endregion


        /// <summary>
        /// Handles the given ClientAction. 
        /// </summary>
        /// <param name="act"></param>
        public void ActivateAction(ClientAction act)
            => GameActionActivated?.Invoke(act);


        public void Draw(Graphics g)
        {
            if (!IsVisible)
                return;

            g.PushWindow(_location, Size);

            OnDraw(g);
            for (int i = 0; i < controls.Count; i++)
                controls[i].Draw(g);

            g.PopWindow();
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
            for (int i = 0; i < controls.Count; i++)
            {
                var c = controls[i];

                c.Update(msElapsed);

                if (controls[i] != c)
                    i--;
            }
        }


        #region Virtual Methods

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

        protected virtual void OnControlAdded(Control c) { }

        protected virtual void OnControlRemoved(Control c) { }

        #endregion



        #region Main Control Methods

        internal void UpdateMain(int msElapsed)
        {
            if (FocusControl == null || !FocusControl.IsChildOf(this))
                FocusControl = this;

            raiseMoveEvents();
            raiseKeyboardEvents();
        }

        MouseArgs baseArgs => new MouseArgs(HoverControl, MouseInfo.UiPosition);
        MouseButtonArgs leftArgs => new MouseButtonArgs(HoverControl, MouseInfo.UiPosition, MouseButton.Left);
        MouseButtonArgs rightArgs => new MouseButtonArgs(HoverControl, MouseInfo.UiPosition, MouseButton.Right);

        void raiseMoveEvents()
        {
            if (HoverControl == null)
                HoverControl = this;

            //button release
            if (MouseInfo.LeftJustReleased)
                HoverControl?.MouseUp?.Invoke(leftArgs);

            if (MouseInfo.RightJustReleased)
                HoverControl?.MouseUp?.Invoke(rightArgs);


            //hover update
            if (!MouseInfo.LeftDown)
            {
                var newHover = getHover(MouseInfo.UiPosition - _location);
                if (newHover != HoverControl)
                {
                    //leave old control
                    HoverControl?.MouseLeave?.Invoke(baseArgs);

                    // if we also released the button this frame and the source 
                    // supports drag-drop, raise the drag-drop events
                    //
                    // hover is kept when mouse is down 
                    // so that's what a drag-drop is
                    if (MouseInfo.LeftJustReleased && HoverControl.CanDrag)
                    {
                        HoverControl.OnDrag?.Invoke(newHover);
                        newHover.OnDrop?.Invoke(HoverControl);
                    }

                    //enter new control
                    HoverControl = newHover;
                    HoverControl.MouseEnter?.Invoke(baseArgs);
                }
            }

            //mouse move
            if (MouseInfo.UiPosition != MouseInfo.OldUiPosition)
                HoverControl?.MouseMove?.Invoke(baseArgs);


            //button press
            if (MouseInfo.LeftJustPressed)
                HoverControl?.MouseDown?.Invoke(leftArgs);

            if (MouseInfo.RightJustPressed)
                HoverControl?.MouseDown?.Invoke(rightArgs);


            //focus control
            if ((MouseInfo.LeftJustPressed || MouseInfo.RightJustPressed) && HoverControl != null)
            {
                var c = HoverControl;
                while (!c.CanFocus && c.Parent != null)
                    c = c.Parent;
                c.SetFocus();
            }
        }

        void raiseKeyboardEvents()
        {
            var focus = FocusControl;
            if (focus != null)
            {
                //release, then press
                //actions before keys

                foreach (var k in KeyboardInfo.JustReleasedKeys)
                    if (!k.IsModifier())
                        focus.KeyReleased?.Invoke(new Keybind(KeyboardInfo.Modifiers, k));


                foreach (var k in KeyboardInfo.JustPressedKeys)
                    if (!k.IsModifier())
                        focus.KeyPressed?.Invoke(new Keybind(KeyboardInfo.Modifiers, k));

                foreach (var ga in KeyboardInfo.JustActivatedActions)
                    focus.GameActionActivated?.Invoke(ga);
            }
        }

        #endregion


        /// <summary>
        /// Makes the control span the whole game window. 
        /// </summary>
        public void Maximize()
        {
            _location = Vector.Zero;
            Size = Parent?.Size ?? Screen.UiSize;
        }

        /// <summary>
        /// Makes this control the currently focused control, only if <see cref="CanFocus"/> is true. 
        /// </summary>
        public void SetFocus()
        {
            if (CanFocus)
                FocusControl = this;
        }

        /// <summary>
        /// Clears focus from the current control, if it is focused (see <see cref="HasFocus"/>).
        /// </summary>
        public void ClearFocus()
        {
            if (FocusControl != this)
                return;

            var c = Parent;
            while (c != null && !c.CanFocus)
                c = c.Parent;
            FocusControl = c;
        }


        /// <summary>
        /// Determines whether this control is contained within the specified control.
        /// Returns true if both controls are the same.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public bool IsChildOf(Control parent)
        {
            var p = this;
            do
            {
                if (p == parent)
                    return true;
            }
            while ((p = p.Parent) != null);

            return false;
        }


        /// <summary>
        /// Brings this control to the front of the Z-order.
        /// </summary>
        public void BringToFront()
        {
            Parent?.controls.MoveToLast(this);
        }

        /// <summary>
        /// Sends this control to the back of the Z-order.
        /// </summary>
        public void SendToBack()
        {
            Parent?.controls.MoveToFirst(this);
        }

        /// <summary>
        /// Places the control in the middle of its <see cref="Parent"/>'s central Y axis.
        /// </summary>
        public void CenterX()
        {
            var sz = Parent?.Width ?? Screen.UiSize.X;
            Left = (sz - Width) / 2;
        }

        /// <summary>
        /// Places the control in the middle of its <see cref="Parent"/>'s central X axis.
        /// </summary>
        public void CenterY()
        {
            var sz = Parent?.Height ?? Screen.UiSize.Y;
            Top = (sz - Height) / 2;
        }

        /// <summary>
        /// Places the control in the middle of its <see cref="Parent"/>
        /// </summary>
        public void CenterBoth()
        {
            var sz = Parent?.Size ?? Screen.UiSize;
            Location = (sz - Size) / 2;
        }


        void resizeChildren(AnchorMode min, AnchorMode max, Vector d)
        {
            foreach (var c in controls)
            {
                var hasMin = c.ParentAnchor.HasFlag(min);
                var hasMax = c.ParentAnchor.HasFlag(max);

                if (hasMax && hasMin)           //anchor both sides -> modify size
                    c.Size += d;
                else if (hasMax && !hasMin)     //anchor at (right/top) -> modify loc
                    c.Location += d;
                else if (!hasMin && !hasMax)    //has no anchor -> float in center
                    c.Location += d / 2;
                //else if                       //anchor at (left/bottom) -> don't touch
            }
        }

        Control getHover(Vector mousePos)
        {
            //search child controls first
            //order by descending zorder
            Control childHover = null;
            for (int i = controls.Count - 1; i >= 0; i--)
            {
                var c = controls[i];
                if (c.IsVisible
                    && c.Bounds.Contains(mousePos)
                    && (childHover = c.getHover(mousePos - c._location)) != null)
                {
                    return childHover;
                }
            }

            return CanHover ? this : null;
        }

        public void ShowMessageBox(string caption, string text)
        {
            foreach (var c in Controls.OfType<MessageBox>())
                Remove(c);
            Add(new MessageBox(caption, text));
        }
    }
}
