using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Client.Sprites;
using IO;

namespace Client.UI
{
    /// <summary>
    /// Represents a user interface control. 
    /// </summary>
    abstract class Control
    {


        /// <summary>
        /// Specifies the default distance between elements in UI scale. 
        /// 
        /// </summary>
        public const float Anchor = 0.01f;

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


        private bool _visible;
        internal Vector2 _absolutePosition;
        internal Vector2 _size = new Vector2(0.1f, 0.1f);
        private Keys MoveAroundKey = Keys.LeftAlt;

        readonly List<Control> controls = new List<Control>();


        /// <summary>
        /// Defines the background color drawn behind the whole control. 
        /// </summary>
        public Color BackColor = Color.Transparent;

        /// <summary>
        /// Gets or sets whether the control is draggable. 
        /// </summary>
        public bool CanDrag = false;

        /// <summary>
        /// Gets or sets whether the control can accept dragged items. 
        /// </summary>
        public bool CanDrop = false;

        /// <summary>
        /// Gets or sets whether the control responds to mouse events. 
        /// </summary>
        public bool ClickThrough { get; set; }

        //unused
        public Keys DragDropKey = Keys.LeftAlt;

        /// <summary>
        /// Gets or sets whether this control can be moved. 
        /// </summary>
        public bool Locked = true;

        public IEnumerable<Control> Controls
        {
            get { return controls; }
        }

        public Control()
        {
            Visible = true;
            MouseDown += UserControl_MouseDown;
        }

        /// <summary>
        /// Occurs when the control receives a drop from another control. 
        /// </summary>
        public event Action<Control, Control> DragDrop;

        /// <summary>
        /// Raised whenever a mouse button is pressed on this control. 
        /// </summary>
        public event Action<Control, Vector2> MouseDown;

        /// <summary>
        /// Raised whenever the mouse enters the control's boundary. 
        /// </summary>
        public event Action MouseEnter;

        /// <summary>
        /// Raised whenever the mouse leaves the control's boundary. 
        /// </summary>
        public event Action MouseLeave;

        /// <summary>
        /// Raised whenever the mouse moves over the control's boundary. 
        /// </summary>
        public event Action<Control, Vector2> MouseMove;

        /// <summary>
        /// Raised whenever a mouse button is released while previously pressed on the control. 
        /// </summary>
        public event Action<Control, Vector2> MouseUp;

        /// <summary>
        /// Raised whenever the control's visibility changes. 
        /// </summary>
        public event Action<Control> VisibleChanged;

        /// <summary>
        /// Gets or sets the absolute position of this control in UI coordinates. 
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                return _absolutePosition;
            }
            set
            {
                if (value != _absolutePosition)
                {
                    var d = value - _absolutePosition;
                    foreach (var c in this.controls)
                        c.AbsolutePosition += d;
                    this._absolutePosition = value;
                }
            }
        }

        /// <summary>
        /// Gets the top Y coordinate of this control relative to its parent. 
        /// </summary>
        public float Top
        {
            get { return RelativePosition.Y; }
            set { RelativePosition = new Vector2(RelativePosition.X, value); }
        }

        /// <summary>
        /// Gets the left X coordinate of this control relative to its parent. 
        /// </summary>
        public float Left
        {
            get { return RelativePosition.X; }
            set { RelativePosition = new Vector2(value, RelativePosition.Y); }
        }

        /// <summary>
        /// Gets the bottom Y coordinate of this control relative to its parent. 
        /// </summary>
        public float Bottom
        {
            get { return RelativePosition.Y + Size.Y; }
        }

        /// <summary>
        /// Gets the right X coordinate of this control relative to its parent. 
        /// </summary>
        public float Right
        {
            get { return RelativePosition.X + Size.X; }
        }

        public bool MouseOver
        {
            get { return HoverControl == this; }
        }

        public Control Parent { get; private set; }

        /// <summary>
        /// Gets or sets the position of this control in its parent's coordinate space. 
        /// </summary>
        public Vector2 RelativePosition
        {
            get
            {
                return (AbsolutePosition - ParentPosition);
            }
            set
            {
                this.AbsolutePosition = ParentPosition + value;
            }
        }

        public int ScreenAnchor
        {
            get { return Screen.UiToScreen(Anchor); }
        }
        /// <summary>
        /// Gets or sets the screen position of this control, in pixels. 
        /// </summary>
        public Point ScreenPosition
        {
            get { return Screen.UiToScreen(_absolutePosition); }
            set { AbsolutePosition = Screen.ScreenToUi(value); }
        }

        /// <summary>
        /// Gets the screen size of this control, in pixels. 
        /// </summary>
        public Point ScreenSize
        {
            get
            {
                return new Point(Screen.UiToScreen(_size.X), Screen.UiToScreen(_size.Y));
            }
        }

        /// <summary>
        /// Gets or sets the size of the control. 
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the tooltip text of this control. 
        /// </summary>
        public string TooltipText { get; set; }


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
                    if (VisibleChanged != null)
                        VisibleChanged(this);
                }
            }
        }

        /// <summary>
        /// Gets the Z-order of the control. 
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// Gets the absolute position of this control's parent. 
        /// </summary>
        /// <returns></returns>
        private Vector2 ParentPosition
        {
            get { return Parent != null ? Parent._absolutePosition : Vector2.Zero; }
        }

        /// <summary>
        /// Gets the size of this control's parent. 
        /// </summary>
        /// <returns></returns>
        private Vector2 ParentSize
        {
            get { return Parent != null ? Parent.Size : Vector2.Zero; }
        }

        #region Drag to move

        Vector2 dragPoint = Vector2.Zero;
        void UserControl_MouseDown(Control sender, Vector2 p)
        {
            if (!Locked && oldKeyboardState.IsKeyDown(Keys.Q))
            {
                dragPoint = p;
            }
        }
        #endregion

        /// <summary>
        /// Adds the specified control as a child of this control. 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="relativeAnchor">True to position the control according to its relative position, false to keep the absolute one. </param>
        public void Add(Control c, bool relativeAnchor = true)
        {
            var pos = c.RelativePosition;
            this.controls.Add(c);
            c.Parent = this;
            if (relativeAnchor)
                c.RelativePosition = pos;
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
        public bool Contains(Vector2 p)
        {
            var localP = p - AbsolutePosition;
            var inside = localP.X >= 0 && localP.Y >= 0 && localP.X < Size.X && localP.Y < Size.Y;
            return inside;
        }

        public void DoDraw(SpriteBatch sb)
        {
            if (this.Visible)
                this.Draw(sb);

            // draw controls
            // sort by z order - lower is first
            if(this.Visible && this.controls.Any())
                foreach (var c in this.controls.OrderBy(c => c.ZOrder))
                    c.DoDraw(sb);
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

            //mouse dragging
            if (mouseState.LeftButton == ButtonState.Released)
                dragPoint = Vector2.Zero;
            else if (dragPoint != Vector2.Zero)  //the control is being moved around by the user. 
            {
                var d = Screen.ScreenToUi(mouseState.Position) - dragPoint;
                this.RelativePosition += d;
                dragPoint += d;
            }
        }

        static Control MoveControl = null;
        public static readonly Keys MoveButton = Keys.LeftAlt;

        /// <summary>
        /// Draws a background over the whole control. 
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.BackColor.A > 0)
                SpriteFactory.Blank.DrawScreen(sb, ScreenPosition, ScreenSize, this.BackColor);
        }

        /// <summary>
        /// Removes the given child control. 
        /// </summary>
        /// <param name="c"></param>
        public void Remove(Control c)
        {
            this.controls.Remove(c);
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

            if (this.Contains(Screen.ScreenToUi(mouseState.Position)))
                return this;

            return null;
        }

        internal void UpdateMain(int msElapsed)
        {
            //span the whole window
            var min = Screen.ScreenToUi(Point.Zero);
            var max = Screen.ScreenToUi(new Point(Screen.Size.X, Screen.Size.Y));
            this._absolutePosition = min;   //use the lowercase field so we don't move children..
            this.Size = max - min;

            //update keyboard/mouse info
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            //get hover control (the one under the mouse pointer)
            var oldHover = HoverControl ?? this;
            var newHover = GetHoverControl() ?? this;

            //get the mouse position
            var mPos = Screen.ScreenToUi(mouseState.Position);
            var oldMPos = Screen.ScreenToUi(oldMouseState.Position);

            var btnHeldDown =
                (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed) ||
                (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed);


            if (MoveControl != null)
            {
                MoveControl.ScreenPosition += (mPos - oldMPos).ToXnaPoint();
            }

            //fire events for the hover controls
            else if (!btnHeldDown)
            {
                HoverControl = newHover;
                if (newHover != null)
                {

                    if (mPos != oldMPos && newHover.MouseMove != null)
                        newHover.MouseMove(newHover, mPos);

                    Func<ButtonState, ButtonState, bool> justPressed = (now, old) =>
                        (now == ButtonState.Pressed && old == ButtonState.Released);

                    Func<ButtonState, ButtonState, bool> justReleased = (now, old) =>
                        (now == ButtonState.Released && old == ButtonState.Pressed);
                    if (newHover == oldHover)
                    {
                        if (justPressed(mouseState.LeftButton, oldMouseState.LeftButton))
                        {
                            if (newHover.MouseDown != null)  // if we *just* pressed the mouse button, fire the MouseDown event. 
                                newHover.MouseDown(newHover, mPos);
                        }
                        else if (justReleased(mouseState.LeftButton, oldMouseState.LeftButton))
                        {
                            if (newHover.MouseUp != null)    // if we *just* released the mouse button, fire the MouseUp event. 
                                newHover.MouseUp(newHover, mPos);
                        }
                    }
                    else    // hover control changed
                    {
                        //leave old control
                        if (oldHover.MouseLeave != null)
                            oldHover.MouseLeave();
                        // if we also released the button this frame, do a shano-drag-drop event. 
                        if (justReleased(mouseState.LeftButton, oldMouseState.LeftButton))
                        {
                            if (newHover.DragDrop != null && oldHover.CanDrag)    // if we *just* released the mouse button, fire the MouseUp event. 
                                newHover.DragDrop(newHover, oldHover);
                        }
                        //enter new control
                        if (newHover.MouseEnter != null)
                            newHover.MouseEnter();
                    }
                }
            }
            oldMouseState = mouseState;
            oldKeyboardState = keyboardState;
        }
    }
}
