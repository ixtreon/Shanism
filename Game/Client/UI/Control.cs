using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IO;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;
using Client.Input;

namespace Client.UI
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
    abstract class Control
    {
        #region Static/Const members


        /// <summary>
        /// Specifies the default distance between elements in UI scale. 
        /// </summary>
        public const double Padding = 0.01;


        internal static Control HoverControl;   //todo: make protected

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
        #endregion



        bool _visible;

        Vector _size;

        /// <summary>
        /// The list of child controls. 
        /// </summary>
        protected List<Control> controls { get; } = new List<Control>();


        #region Public Properties

        /// <summary>
        /// Defines the background color drawn behind the whole control. 
        /// </summary>
        public Color BackColor { get; set; } = Color.Transparent;

        /// <summary>
        /// Gets or sets whether the control is draggable. 
        /// </summary>
        public bool CanDrag { get; set; } = false;

        /// <summary>
        /// Gets or sets whether this control can be moved. NYI
        /// </summary>
        public bool Locked { get; set; } = true;

        /// <summary>
        /// Gets the parent of this control. 
        /// </summary>
        public Control Parent { get; private set; }

        /// <summary>
        /// Gets or sets the tooltip text of this control. 
        /// </summary>
        public object ToolTip { get; set; }

        /// <summary>
        /// Gets the Z-order of the control. 
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// Gets or sets whether this control responds to mouse events. 
        /// </summary>
        public bool CanHover { get; set; } = true;

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
        /// Raised whenever the control receives a drag-drop from another control. 
        /// </summary>
        public event Action<Control> OnDrop;

        /// <summary>
        /// Raised whenever the control is drag-dropped onto another control. 
        /// </summary>
        public event Action<Control> OnDrag;

        /// <summary>
        /// Raised whenever the mouse enters the control's boundary. 
        /// </summary>
        public event Action<MouseEvent> MouseEnter;

        /// <summary>
        /// Raised whenever the mouse leaves the control's boundary. 
        /// </summary>
        public event Action<MouseEvent> MouseLeave;

        /// <summary>
        /// Raised whenever the mouse moves over the control's boundary. 
        /// </summary>
        public event Action<MouseEvent> MouseMove;

        /// <summary>
        /// Raised whenever a mouse button is pressed on this control. 
        /// </summary>
        public event Action<MouseButtonEvent> MouseDown;

        /// <summary>
        /// Raised whenever a mouse button is released while previously pressed on the control.
        /// </summary>
        public event Action<MouseButtonEvent> MouseUp;

        /// <summary>
        /// Raised whenever the control's visibility changes. 
        /// </summary>
        public event Action<Control> VisibleChanged;
        #endregion


        /// <summary>
        /// Gets all the children of this control. 
        /// </summary>
        public IEnumerable<Control> Controls
        {
            get { return controls; }
        }

        /// <summary>
        /// Gets or sets the absolute position of this control in UI coordinates. 
        /// Calculated using the parent's absolute position. 
        /// </summary>
        public Vector AbsolutePosition
        {
            get
            {
                return ParentAbsolutePosition + Location;
            }
            set
            {
                Location = value - ParentAbsolutePosition;
            }
        }

        /// <summary>
        /// Gets or sets whether the control is visible. 
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set
            {
                if (value != _visible)
                {
                    _visible = value;
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

        #region Control Bounds
        /// <summary>
        /// Gets the top (low Y) coordinate of this control relative to its parent. 
        /// </summary>
        public double Top
        {
            get { return Location.Y; }
        }

        /// <summary>
        /// Gets the left X coordinate of this control relative to its parent. 
        /// </summary>
        public double Left
        {
            get { return Location.X; }
        }

        /// <summary>
        /// Gets the bottom (high Y) coordinate of this control relative to its parent. 
        /// </summary>
        public double Bottom
        {
            get { return Location.Y + Size.Y; }
        }

        /// <summary>
        /// Gets the right X coordinate of this control relative to its parent. 
        /// </summary>
        public double Right
        {
            get { return Location.X + Size.X; }
        }

        /// <summary>
        /// Gets whether the mouse is currently over this control. 
        /// </summary>
        public bool MouseOver
        {
            get { return HoverControl == this; }
        }


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
            get
            {
                return (Size * Screen.UiScale).ToPoint();
            }
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



        public Control()
        {
            Visible = true;
            MouseDown += UserControl_MouseDown;
        }


        #region Drag to move

        Vector dragLast = Vector.Zero;
        Vector dragStart = Vector.Zero;
        Keys dragToMoveKey = Keys.Q;

        void UserControl_MouseDown(MouseButtonEvent ev)
        {
            if (KeyboardInfo.IsDown(dragToMoveKey))
            {
                var cc = this;
                while (cc.Locked && cc.Parent != null)
                    cc = cc.Parent;

                if (!cc.Locked)
                {
                    cc.dragLast = ev.RelativePosition;
                    cc.dragStart = ev.RelativePosition;
                }
            }
        }

        void checkDragMove()
        {
            if (dragStart == Vector.Zero)
                return;

            //stop holding keyboard button -> reset control
            if (!KeyboardInfo.IsDown(dragToMoveKey))
            {
                Location += (dragStart - dragLast);
                dragStart = Vector.Zero;
                return;
            }

            //stop holding mouse button -> move control
            if (mouseState.LeftButton == ButtonState.Released)
            {
                dragStart = Vector.Zero;
                return;
            }

            //TODO: controls can't get out of their parents!
            var dragNow = Screen.ScreenToUi(mouseState.Position.ToPoint());
            Location += (dragNow - dragLast);
            dragLast = dragNow;
        }
        #endregion


        /// <summary>
        /// Adds the specified control as a child of this control. 
        /// </summary>
        /// <param name="c"></param>
        public void Add(Control c, bool relativeAnchor = true)
        {
            var pos = c.Location;
            controls.Add(c);

            c.Parent = this;
            c.BringToFront();
        }

        public void Show() { Visible = true; }

        public void Hide() { Visible = false; }


        /// <summary>
        /// Brings thie control to the front of the z-order. 
        /// </summary>
        public void BringToFront()
        {
            if (Parent != null && Parent.controls.Count > 1)
            {
                var maxZ = Parent.controls.Max(c => c.ZOrder);
                this.ZOrder = maxZ + 1;
            }
            else
                this.ZOrder = 0;
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

        public void Draw(SpriteBatch sb)
        {
            Draw(new Graphics(sb));
        }

        public void Draw(Graphics parentGraphics)
        {
            if (Visible)
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
        /// Updates the control's state and fires the appropriate events in response to user input. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public void Update(int msElapsed)
        {
            //update self
            this.OnUpdate(msElapsed);

            //update controls
            foreach (var c in this.controls)
                c.Update(msElapsed);

            checkDragMove();
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
        /// Removes the given child control. 
        /// </summary>
        /// <param name="c"></param>
        public bool Remove(Control c)
        {
            return controls.Remove(c);
        }

        /// <summary>
        /// Sends this control to the back of the z-order. 
        /// </summary>
        public void SendToBack()
        {
            if (Parent != null && Parent.controls.Count > 1)
            {
                var minZ = Parent.controls.Min(c => c.ZOrder);
                this.ZOrder = minZ - 1;
            }
            else
                this.ZOrder = 0;
        }

        /// <summary>
        /// Override in derived classes to implement custom Update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        protected virtual void OnUpdate(int msElapsed) { }


        /// <summary>
        /// Returns the control under the mouse pointer: could be this or any control, even null if there is no such control. 
        /// </summary>
        /// <returns></returns>
        private Control GetHoverControl()
        {
            //search child controls first
            //order by reverse z - higher is first
            var childHover = controls
                .Where(c => c.Visible)
                .OrderBy(c => -c.ZOrder)
                .Select(c => c.GetHoverControl())
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
            oldMouseState = mouseState;
            mouseState = Mouse.GetState();

            var mPos = Screen.ScreenToUi(mouseState.Position.ToPoint());
            var oldMPos = Screen.ScreenToUi(oldMouseState.Position.ToPoint());

            //check drag-drop start before updating hover
            if (holdDownLeft)
                return;

            //update the hover control
            var oldHover = HoverControl ?? this;
            HoverControl = GetHoverControl() ?? this;

            //mouse move
            if (mPos != oldMPos)
                HoverControl.MouseMove?.Invoke(new MouseEvent(HoverControl, mPos));

            if (HoverControl == oldHover)
            {
                //left 
                if (justPressedLeft)
                    HoverControl.MouseDown?
                        .Invoke(new MouseButtonEvent(HoverControl, mPos, MouseButton.Left));
                else if (justReleasedLeft)
                    HoverControl.MouseUp?
                        .Invoke(new MouseButtonEvent(HoverControl, mPos, MouseButton.Left));

                //right 
                if (justPressedRight)
                    HoverControl.MouseDown?
                        .Invoke(new MouseButtonEvent(HoverControl, mPos, MouseButton.Right));
                else if (justReleasedRight)
                    HoverControl.MouseUp?
                        .Invoke(new MouseButtonEvent(HoverControl, mPos, MouseButton.Right));
            }
            else
            {
                //leave old control
                oldHover.MouseLeave?.Invoke(new MouseEvent(HoverControl, mPos));

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
                HoverControl.MouseEnter?.Invoke(new MouseEvent(HoverControl, mPos));
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

            Location = min;   //use the lowercase field so we don't move children..
            Size = max - min;
        }
    }
}
