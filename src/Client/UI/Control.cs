using Ix.Math;
using Shanism.Client.Assets;
using Shanism.Client.IO;
using Shanism.Client.Views;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{

    static class ControlContext
    {
        public static ScreenSystem Screen { get; private set; }
        public static ContentList Content { get; private set; }

        public static void Init(ScreenSystem screen, ContentList content)
        {
            Screen = screen ?? throw new ArgumentNullException(nameof(screen));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }

    /// <summary>
    /// A user interface control. 
    /// </summary>
    public partial class Control
    {

        /// <summary>
        /// Specifies the default distance between elements in UI scale. 
        /// </summary>
        public const float DefaultPadding = 0.01f;

        public const float LargePadding = 0.02f;


        /// <summary>
        /// The list of child controls in a back-to-front order. 
        /// </summary>
        readonly List<Control> controls = new List<Control>();

        bool isInitialized;
        bool _isVisible = true;
        Vector2 _size = new Vector2(DefaultPadding);
        Vector2 _location;
        Vector2 _minimumSize;
        Vector2? _maximumSize;
        View _view;
        Control _parent;


        /// <summary>
        /// Gets the first control that is under the mouse pointer
        /// and has <see cref="CanHover"/> set to <c>true</c>. 
        /// </summary>
        public Control HoverControl => View?.ViewHoverControl;

        /// <summary>
        /// Gets the control that currently has keyboard focus. 
        /// A control must have its <see cref="CanFocus"/> property set to <c>true</c> in order to become the <see cref="View.ViewFocusControl"/>.
        /// </summary>
        public Control FocusControl => View?.ViewFocusControl;



        protected static ScreenSystem Screen => ControlContext.Screen;
        protected static ContentList Content => ControlContext.Content;


        #region Public Properties

        /// <summary>
        /// Gets the inner padding of the control
        /// which determines its <see cref="ClientBounds"/>.
        /// </summary>
        public float Padding { get; set; } = DefaultPadding;

        /// <summary>
        /// Gets the parent of this control. 
        /// </summary>
        public Control Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                View = _parent?.View;
            }
        }

        /// <summary>
        /// Defines the background color drawn behind the whole control. 
        /// </summary>
        public virtual Color BackColor { get; set; } = Color.Transparent;

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
        public virtual object ToolTip { get; set; }

        /// <summary>
        /// The cursor displayed when the mouse is over this control.
        /// Only used if <see cref="CanHover"/> is set to true.
        /// </summary>
        public GameCursor Cursor { get; set; } = GameCursor.Default;

        /// <summary>
        /// Gets or sets the sticky sides in relation to the parent control. 
        /// </summary>
        public AnchorMode ParentAnchor { get; set; } = AnchorMode.Top | AnchorMode.Left;

        public AnchorMode ResizeAnchor { get; set; } = AnchorMode.Top | AnchorMode.Left;

        /// <summary>
        /// Gets or recursively sets the root view
        /// of this control and all its child controls.
        /// </summary>
        public View View
        {
            get => _view;
            set
            {
                if (_view == value)
                    return;

                var oldView = _view;
                _view = value;

                OnViewChanged(new ViewChangeArgs(oldView, value));

                for (int i = 0; i < controls.Count; i++)
                    controls[i].View = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the control is visible. 
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible)
                    return;

                _isVisible = value;

                OnVisibleChanged(EventArgs.Empty);
                if (value)
                    OnShown(EventArgs.Empty);
                else
                    OnHidden(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the position of this control in its parent's coordinate space. 
        /// </summary>
        public Vector2 Location
        {
            get => _location;
            set => _location = value;
        }

        /// <summary>
        /// Clamps the given size between <see cref="MinimumSize"/> and <see cref="MaximumSize"/>.
        /// </summary>
        protected void ClampSize(ref Vector2 value)
        {
            if (_maximumSize != null)
                value = Vector2.Clamp(value, _minimumSize, _maximumSize.Value);
            else
                value = Vector2.Max(value, _minimumSize);
        }

        void SetSize(Vector2 newSize)
        {
            if (_size == newSize) return;
            ClampSize(ref newSize);
            if (_size == newSize) return;

            var dSize = newSize - _size;

            _size = newSize;
            Location -= ResizeAnchor.GetOffset() * dSize;

            // handle custom anchors/offsets on resize
            resizeChildren(AnchorMode.Left, AnchorMode.Right, new Vector2(dSize.X, 0));
            resizeChildren(AnchorMode.Top, AnchorMode.Bottom, new Vector2(0, dSize.Y));

            // raise an event
            OnSizeChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets the size of the control in UI units. 
        /// </summary>
        public Vector2 Size
        {
            get => _size;
            set => SetSize(value);
        }

        public Vector2? MaximumSize
        {
            get => _maximumSize;
            set
            {
                _maximumSize = value;
                if (value != null)
                    _minimumSize = Vector2.Min(_minimumSize, value.Value);

                SetSize(Size);
            }
        }

        /// <summary>
        /// Gets or sets the minimum size of the control.
        /// Zero by default. Cannot become negative.
        /// </summary>
        public Vector2 MinimumSize
        {
            get => _minimumSize;
            set
            {
                _minimumSize = Vector2.Max(value, Vector2.Zero);
                if (_maximumSize != null)
                    _maximumSize = Vector2.Max(_maximumSize.Value, value);

                SetSize(Size);
            }
        }

        /// <summary>
        /// Gets the bounds of this control relative to its parent's coordinate space.
        /// </summary>
        public RectangleF Bounds
        {
            get => new RectangleF(Location, _size);
            set
            {
                Location = value.Position;
                Size = value.Size;
            }
        }

        public virtual RectangleF ClientBounds
        {
            get => new RectangleF(Vector2.Zero, _size).Deflate(Padding);
        }

        #endregion

        #region Property Shortcuts


        /// <summary>
        /// Calculates the absolute position of this control in UI coordinates. 
        /// </summary>
        public Vector2 AbsoluteLocation => Location + (Parent?.AbsoluteLocation ?? Vector2.Zero);

        /// <summary>
        /// Gets all the children of this control. 
        /// </summary>
        public IReadOnlyList<Control> Controls => controls;

        /// <summary>
        /// Gets the currently set UI color scheme.
        /// </summary>
        public ColorScheme UiColors => ColorScheme.Current;

        /// <summary>
        /// Gets or sets the width of this control.
        /// </summary>
        public float Width
        {
            get => _size.X;
            set => Size = new Vector2(value, _size.Y);
        }

        /// <summary>
        /// Gets or sets the height of this control.
        /// </summary>
        public float Height
        {
            get => _size.Y;
            set => Size = new Vector2(_size.X, value);
        }

        /// <summary>
        /// Gets the top (low Y) coordinate of this control relative to its parent. 
        /// </summary>
        public float Top
        {
            get => Location.Y;
            set => Location = new Vector2(Location.X, value);
        }

        /// <summary>
        /// Gets the left X coordinate of this control relative to its parent. 
        /// </summary>
        public float Left
        {
            get => Location.X;
            set => Location = new Vector2(value, Location.Y);
        }

        /// <summary>
        /// Gets the bottom (high Y) coordinate of this control relative to its parent. 
        /// </summary>
        public float Bottom
        {
            get => Location.Y + Size.Y;
            set => Location = new Vector2(Location.X, value - Size.Y);
        }

        /// <summary>
        /// Gets the right X coordinate of this control relative to its parent. 
        /// </summary>
        public float Right
        {
            get => Location.X + Size.X;
            set => Location = new Vector2(value - Size.X, Location.Y);
        }

        /// <summary>
        /// Gets whether this control is the currently focused control (see <see cref="View.ViewFocusControl"/>).
        /// </summary>
        public bool IsFocusControl => View.ViewFocusControl == this;

        /// <summary>
        /// Gets whether the mouse is currently over this control (see <see cref="View.ViewHoverControl"/>).
        /// </summary>
        public bool IsHoverControl => (HoverControl == this);

        /// <summary>
        /// Gets whether this control is a root (top-level) control. 
        /// </summary>
        public bool IsRootControl => Parent == null;

        #endregion


        #region Virtual Methods

        /// <summary>
        /// Override in derived classes to implement custom update logic. 
        /// </summary>
        public virtual void Update(int msElapsed) { }

        /// <summary>
        /// Draws a background over the whole control. 
        /// </summary>
        public virtual void Draw(Canvas c)
        {
            DrawBackground(c, BackColor);
        }

        protected void DrawBackground(Canvas canvas, Color c)
        {
            if (c.A > 0)
                canvas.FillRectangle(Vector2.Zero, Size, c);
        }

        #endregion

        #region Collection

        /// <summary>
        /// Adds the specified control as a child of this control.
        /// </summary>
        /// <param name="c">The control that is to be added. Cannot be null.</param>
        /// <returns>The same control. Just in case. </returns>
        public Control Add(Control c)
        {
            if (c == null) throw new ArgumentNullException(nameof(c));
            if (c.Parent != null) throw new ArgumentException("This control is already a child of another control.");

            c.Parent = this;
            c.View = View;

            controls.Add(c);

            c.tryInitialize();
            c.SetFocus();

            OnControlAdded(new ControlChildArgs(this, c));
            c.OnParentChanged(new ParentChangeArgs(c, null, this));

            return c;
        }

        public void AddRange(params Control[] controls) => AddRange((IEnumerable<Control>)controls);

        public void AddRange(IEnumerable<Control> controls)
        {
            foreach (var c in controls ?? throw new ArgumentNullException(nameof(controls)))
                Add(c);
        }


        /// <summary>
        /// Removes the given child control. 
        /// </summary>
        /// <param name="c"></param>
        public bool Remove(Control c)
        {
            var ans = controls.Remove(c);

            if (ans)
                onRemove(c);

            return ans;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= controls.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var c = controls[index];
            controls.RemoveAt(index);

            onRemove(c);
        }

        public void RemoveRange(IEnumerable<Control> controls)
        {
            foreach (var c in controls)
                Remove(c);
        }

        /// <summary>
        /// Removes all child controls.
        /// </summary>
        public void RemoveAll()
        {
            for (int i = controls.Count - 1; i >= 0; i--)
                RemoveAt(i);
        }

        void onRemove(Control c)
        {
            c.Parent = null;
            c.View = null;

            OnControlRemoved(new ControlChildArgs(this, c));
            c.OnParentChanged(new ParentChangeArgs(c, this, null));
        }

        /// <summary>
        /// Removes all child controls of the specified type.
        /// </summary>
        public void RemoveAll<T>()
            => RemoveWhere(c => c is T);

        public void RemoveWhere(Predicate<Control> f)
        {
            for (int i = controls.Count - 1; i >= 0; i--)
                if (f(controls[i]))
                    RemoveAt(i);
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
            while (p != parent && (p = p.Parent) != null) { }
            return p == parent;
        }

        public bool IsFirstChild
            => Parent?.controls.FirstOrDefault() == this;

        public bool IsLastChild
            => Parent?.controls.LastOrDefault() == this;

        #endregion

        #region Focus

        /// <summary>
        /// Makes this control the currently focused control, only if <see cref="CanFocus"/> is true. 
        /// </summary>
        public void SetFocus()
        {
            if (View != null && IsVisible && (CanFocus || IsRootControl))
                View.SetFocus(this);
        }

        internal void InvokeFocusGained() => OnFocusGained(EventArgs.Empty);
        internal void InvokeFocusLost() => OnFocusLost(EventArgs.Empty);

        /// <summary>
        /// Clears focus from the current control, if it is focused (see <see cref="IsFocusControl"/>).
        /// </summary>
        public void ClearFocus()
        {
            if (FocusControl != this || Parent == null)
                return;

            //find control to give the focus to
            var c = Parent;
            while (!c.CanFocus && c.Parent != null)
                c = c.Parent;

            c.SetFocus();
        }

        #endregion

        #region Z-Order

        /// <summary>
        /// Brings this control to the front of the Z-order.
        /// </summary>
        public void BringToFront()
            => Parent.controls.MoveToLast(this);

        /// <summary>
        /// Sends this control to the back of the Z-order.
        /// </summary>
        public void SendToBack()
            => Parent.controls.MoveToFirst(this);

        #endregion

        #region Visibility & Bounds

        public void Show() => IsVisible = true;

        public void Hide() => IsVisible = false;


        Vector2 _parentCenter => ((Parent?.Size ?? Screen.UI.Size) - Size) / 2;

        /// <summary>
        /// If set to true, centers the control along the X dimension, 
        /// so middle of left-right.
        /// </summary>
        public bool CenterX
        {
            set => Left = value ? _parentCenter.X : Left;
        }
        /// <summary>
        /// If set to true, centers the control along the Y dimension,
        /// so middle of top-bottom.
        /// </summary>
        public bool CenterY
        {
            set => Top = value ? _parentCenter.Y : Top;
        }

        /// <summary>
        /// If set to true, centers the control along both dimensions,
        /// so middle of parent.
        /// </summary>
        public bool CenterBoth
        {
            set => Location = value ? _parentCenter : Location;
        }

        #endregion


        /// <summary>
        /// A hacky method to walk the <see cref="Parent"/> chain up to the top.
        /// </summary>
        public Control FindRoot()
        {
            var c = this;
            while (c.Parent != null)
                c = c.Parent;
            return c;
        }


        void tryInitialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;
            OnInitialized(EventArgs.Empty);
        }


        void resizeChildren(AnchorMode min, AnchorMode max, Vector2 d)
        {
            foreach (var c in controls)
            {
                var hasMin = (c.ParentAnchor & min) != 0;
                var hasMax = (c.ParentAnchor & max) != 0;

                if (hasMax && hasMin)           //both anchors -> modify size
                    c.Size += d;
                else if (hasMax && !hasMin)     //2nd anchor (right/top) -> modify loc
                    c.Location += d;
                else if (!hasMin && !hasMax)    //no anchor -> float in center
                    c.Location += d / 2;
                //else if                       //1st anchor (left/bottom) -> don't touch
            }
        }

        protected Control getHover(Vector2 mousePos)
        {

            //search child controls first
            //order by descending zorder
            Control childHover = null;
            for (int i = controls.Count - 1; i >= 0; i--)
            {
                var c = controls[i];

                if (!c.IsVisible)
                    continue;

                if (!mousePos.IsInside(c.Bounds))
                    continue;

                childHover = c.getHover(mousePos - c.Location);
                if (childHover != null)
                    return childHover;

            }

            if (CanHover)
                return this;

            return null;
        }


        public void ForChildrenRecursed(Action<Control> func)
        {
            if (!controls.Any())
                return;

            var s = new Stack<Control>(controls);
            do
            {
                var cur = s.Pop();

                func(cur);

                foreach (var c in cur.controls)
                    s.Push(c);
            }
            while (s.Any());
        }
    }
}
