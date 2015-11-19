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


        protected static Control HoverControl;

        /// <summary>
        /// Gets the keyboard state. 
        /// </summary>
        protected static KeyboardState
            oldKeyboardState = Keyboard.GetState(),
            keyboardState = Keyboard.GetState();

        /// <summary>
        /// Gets the mouse state. 
        /// </summary>
        protected static MouseState
            oldMouseState = Mouse.GetState(),
            mouseState = Mouse.GetState();
        #endregion


        bool _visible;
        //Vector _absolutePosition;
        //Vector _size = new Vector(0.1f, 0.1f);

        protected List<Control> controls { get; } = new List<Control>();


        /// <summary>
        /// Defines the background color drawn behind the whole control. 
        /// </summary>
        public Color BackColor { get; set; } = Color.Transparent;

        /// <summary>
        /// Gets or sets whether the control is draggable. 
        /// </summary>
        public bool CanDrag { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the control responds to mouse events. 
        /// </summary>
        public bool ClickThrough { get; set; }

        /// <summary>
        /// Gets or sets whether this control can be moved. 
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
        public event Action MouseEnter;

        /// <summary>
        /// Raised whenever the mouse leaves the control's boundary. 
        /// </summary>
        public event Action MouseLeave;

        /// <summary>
        /// Raised whenever a mouse button is pressed on this control. 
        /// </summary>
        public event Action<Control, Vector> MouseDown;

        /// <summary>
        /// Raised whenever the mouse moves over the control's boundary. 
        /// </summary>
        public event Action<Control, Vector> MouseMove;

        /// <summary>
        /// Raised whenever a mouse button is released while previously pressed on the control. 
        /// </summary>
        public event Action<Control, Vector> MouseUp;

        /// <summary>
        /// Raised whenever the control's visibility changes. 
        /// </summary>
        public event Action<Control> VisibleChanged;
        #endregion


        public IEnumerable<Control> Controls
        {
            get { return controls; }
        }

        /// <summary>
        /// Gets or sets the absolute position of this control in UI coordinates. 
        /// </summary>
        public Vector AbsolutePosition
        {
            get
            {
                return ParentPosition + Location;
            }
            set
            {
                Location = value - ParentPosition;
            }
        }


        Vector _position;
        Vector _size;

        /// <summary>
        /// Gets the sticky sides in relation to the parent control. 
        /// </summary>
        public AnchorMode ParentAnchor { get; set; } = AnchorMode.Left | AnchorMode.Top;

        /// <summary>
        /// Gets or sets the position of this control in its parent's coordinate space. 
        /// </summary>
        public Vector Location
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the size of the control in UI coordinates. 
        /// </summary>
        public Vector Size
        {
            get { return _size; }
            set
            {
                if(_size != value)
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
            foreach(var c in controls)
            {
                var hasMin = c.ParentAnchor.HasFlag(min);
                var hasMax = c.ParentAnchor.HasFlag(max);

                if (hasMax && hasMin)    //anchor both sides -> modify size
                    c.Size += d;
                else if (hasMax && !hasMin)  // anchor at max (right/top) -> modify loc
                    c.Location += d;
                else if (!hasMin && !hasMax) // has no anchor -> float
                    c.Location += d / 2;
            }
        }

        public double Top
        {
            get { return _position.Y; }
            //set { Location = new Vector(Location.X, value); }
        }

        /// <summary>
        /// Gets the left X coordinate of this control relative to its parent. 
        /// </summary>
        public double Left
        {
            get { return _position.X; }
            //set { Location = new Vector(value, Location.Y); }
        }

        /// <summary>
        /// Gets the bottom Y coordinate of this control relative to its parent. 
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


        #region screen stuff
        /// <summary>
        /// Gets or sets the screen position of this control, in pixels. 
        /// </summary>
        public Vector ScreenPosition
        {
            get { return Screen.UiToScreen(_position); }
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
        /// Gets the absolute position of this control's parent. 
        /// </summary>
        /// <returns></returns>
        private Vector ParentPosition
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


        public Control()
        {
            Visible = true;
            MouseDown += UserControl_MouseDown;
        }

        #region Drag to move

        Vector dragLast = Vector.Zero;
        Vector dragStart = Vector.Zero;
        Keys dragToMoveKey = Keys.Q;

        void UserControl_MouseDown(Control sender, Vector p)
        {
            if (keyboardState.IsKeyDown(dragToMoveKey))
            {
                var cc = this;
                while (cc.Locked && cc.Parent != null)
                    cc = cc.Parent;

                if (!cc.Locked)
                {
                    cc.dragLast = p;
                    cc.dragStart = p;
                }
            }
        }

        void checkDragMove()
        {
            if (dragStart == Vector.Zero)
                return;

            //stop holding keyboard button -> reset control
            if (!keyboardState.IsKeyDown(dragToMoveKey))
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
        /// <param name="relativeAnchor">True to position the control according to its relative position, false to keep the absolute one. </param>
        public void Add(Control c, bool relativeAnchor = true)
        {
            var pos = c.Location;
            this.controls.Add(c);

            c.Parent = this;
            c.BringToFront();
        }

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

        public void DoDraw(SpriteBatch sb)
        {
            DoDraw(new Graphics(sb, Location, Size));
        }

        public void DoDraw(Graphics g)
        {
            if (Visible)
            {
                Draw(g);

                // draw controls
                // sort by z order - lower is first
                if (controls.Any())
                {
                    foreach (var c in controls.OrderBy(c => c.ZOrder))
                        c.DoDraw(new Graphics(g.SpriteBatch, g.Position + c.Location, c.Size));
                }
            }
        }

        /// <summary>
        /// Updates the control's state and fires the appropriate events in response to user input. 
        /// </summary>
        /// <param name="msElapsed"></param>
        public void DoUpdate(int msElapsed)
        {
            //update self
            this.Update(msElapsed);

            //update controls
            foreach (var c in this.controls)
                c.DoUpdate(msElapsed);

            checkDragMove();
        }

        static Control MoveControl = null;
        public static readonly Keys MoveButton = Keys.LeftAlt;

        /// <summary>
        /// Draws a background over the whole control. 
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(Graphics g)
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
        public virtual void Update(int msElapsed) { }


        /// <summary>
        /// Returns the control under the mouse pointer, or null if there is no such control. 
        /// </summary>
        /// <returns></returns>
        private Control GetHoverControl()
        {
            //order by reverse z - higher is first
            foreach (var c in controls.OrderBy(c => -c.ZOrder))
            {
                if (c.Visible)
                {
                    var hc = c.GetHoverControl();
                    if (hc != null)
                        return hc;
                }
            }

            if (this.Contains(Screen.ScreenToUi(mouseState.Position.ToPoint())))
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
            //span the whole window
            var min = Screen.ScreenToUi(Point.Zero);
            var max = Screen.ScreenToUi(new Point(Screen.Size.X, Screen.Size.Y));
            Location = min;   //use the lowercase field so we don't move children..
            Size = max - min;


            //update keyboard/mouse info
            oldMouseState = mouseState;
            oldKeyboardState = keyboardState;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

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
                HoverControl.MouseMove?.Invoke(HoverControl, mPos);

            if (HoverControl == oldHover)
            {
                //mouse down
                if (justPressedLeft)
                    HoverControl.MouseDown?.Invoke(HoverControl, mPos);

                //mouse up
                if (justReleasedLeft)
                    HoverControl.MouseUp?.Invoke(HoverControl, mPos);
            }
            else
            {
                //leave old control
                oldHover.MouseLeave?.Invoke();

                // if we also released the button this frame
                // and the source supports drag-drop
                // raise the events
                // hover is kept when mouse is down so that's what a drag-drop is
                if (justReleasedLeft && oldHover.CanDrag)
                {
                    oldHover.OnDrag?.Invoke(HoverControl);
                    HoverControl.OnDrop?.Invoke(oldHover);
                    Console.WriteLine("DRAG DROP?");
                }

                //enter new control
                HoverControl.MouseEnter?.Invoke();
            }
        }

    }
}
