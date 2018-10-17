using System;

namespace Shanism.Client.UI
{
    partial class Control
    {

        #region Drag & Drop

        /// <summary>
        /// Executed when the user starts dragging this control.
        /// <para>Raises the <see cref="DragBegin"/> event.</para>
        /// </summary>
        protected virtual void OnDragBegin(DragArgs e) => DragBegin?.Invoke(this, e);
        /// <summary>
        /// Executed when the user drag-and-drops this control onto another control.
        /// <para>Raises the <see cref="DragEnd"/> event.</para>
        /// </summary>
        protected virtual void OnDragEnd(DragDropArgs e) => DragEnd?.Invoke(this, e);
        /// <summary>
        /// Executed when the user drag-and-drops another control onto this control.
        /// <para>Raises the <see cref="DropEnd"/> event.</para>
        /// </summary>
        protected virtual void OnDropEnd(DragDropArgs e) => DropEnd?.Invoke(this, e);

        /// <summary>
        /// Raised whenever this control is drag-dropped onto another control. 
        /// </summary>
        public event UiEventHandler<DragArgs> DragBegin;
        /// <summary>
        /// Raised whenever this control is drag-dropped onto another control. 
        /// </summary>
        public event UiEventHandler<DragDropArgs> DragEnd;
        /// <summary>
        /// Raised whenever this control receives a drag-drop from another control. 
        /// </summary>
        public event UiEventHandler<DragDropArgs> DropEnd;

        #endregion

        #region Focus

        protected virtual void OnFocusGained(EventArgs e) => FocusGained?.Invoke(this, e);
        protected virtual void OnFocusLost(EventArgs e) => FocusLost?.Invoke(this, e);

        /// <summary>
        /// Raised whenever this control becomes the <see cref="FocusControl"/>.
        /// </summary>
        public event UiEventHandler<EventArgs> FocusGained;

        /// <summary>
        /// Raised whenever this control stops being the <see cref="FocusControl"/>.
        /// </summary>
        public event UiEventHandler<EventArgs> FocusLost;

        #endregion

        #region Mouse

        protected virtual void OnMouseEnter(MouseArgs e) => MouseEnter?.Invoke(this, e);
        protected virtual void OnMouseLeave(MouseArgs e) => MouseLeave?.Invoke(this, e);
        protected virtual void OnMouseMove(MouseArgs e) => MouseMove?.Invoke(this, e);
        protected virtual void OnMouseDown(MouseButtonArgs e) => MouseDown?.Invoke(this, e);
        protected virtual void OnMouseUp(MouseButtonArgs e) => MouseUp?.Invoke(this, e);
        protected virtual void OnMouseClick(MouseButtonArgs e) => MouseClick?.Invoke(this, e);
        protected virtual void OnMouseDoubleClick(MouseButtonArgs e) => MouseDoubleClick?.Invoke(this, e);
        protected virtual void OnMouseScroll(MouseScrollArgs e) => MouseScroll?.Invoke(this, e);


        /// <summary>
        /// Raised whenever the mouse enters the control's boundary. 
        /// </summary>
        public event UiEventHandler<MouseArgs> MouseEnter;
        /// <summary>
        /// Raised whenever the mouse leaves the control's boundary. 
        /// </summary>
        public event UiEventHandler<MouseArgs> MouseLeave;
        /// <summary>
        /// Raised whenever the mouse moves while the control is on focus. 
        /// </summary>
        public event UiEventHandler<MouseArgs> MouseMove;

        /// <summary>
        /// Raised whenever a mouse button is pressed while the control is on focus. 
        /// </summary>
        public event UiEventHandler<MouseButtonArgs> MouseDown;
        /// <summary>
        /// Raised whenever a mouse button is released while the control is on focus.
        /// </summary>
        public event UiEventHandler<MouseButtonArgs> MouseUp;

        /// <summary>
        /// Raised whenever a mouse button is released while the control is on focus 
        /// and the mouse cursor is inside the control's bounds.
        /// </summary>
        public event UiEventHandler<MouseButtonArgs> MouseClick;
        public event UiEventHandler<MouseButtonArgs> MouseDoubleClick;
        public event UiEventHandler<MouseScrollArgs> MouseScroll;

        #endregion

        #region Keyboard

        /// <summary>
        /// Executed when a key is pressed while this control is the <see cref="FocusControl"/>.
        /// <para> Raises the <see cref="KeyPress"/> event. </para>
        /// </summary>
        protected virtual void OnKeyPress(KeyboardArgs e) => KeyPress?.Invoke(this, e);
        /// <summary>
        /// Executed when a key is released while this control is the <see cref="FocusControl"/>.
        /// <para> Raises the <see cref="KeyRelease"/> event. </para>
        /// </summary>
        protected virtual void OnKeyRelease(KeyboardArgs e) => KeyRelease?.Invoke(this, e);
        /// <summary>
        /// Executed when a character is repeated while this control is the <see cref="FocusControl"/>.
        /// <para> Raises the <see cref="CharacterInput"/> event. </para>
        /// </summary>
        protected virtual void OnCharInput(KeyboardArgs e) => CharacterInput?.Invoke(this, e);
        /// <summary>
        /// Executed when a key is pressed while this control is the <see cref="FocusControl"/>.
        /// <para> Raises the <see cref="KeyPress"/> event. </para>
        /// </summary>
        protected virtual void OnActionActivated(ClientActionArgs e) => ActionActivated?.Invoke(this, e);

        public event UiEventHandler<KeyboardArgs> KeyPress;
        public event UiEventHandler<KeyboardArgs> KeyRelease;
        public event UiEventHandler<KeyboardArgs> CharacterInput;
        /// <summary>
        /// Raised whenever a game UiEventHandler (a key plus/minus some modifier keys) 
        /// is activated while this control has focus (see <see cref="IsFocusControl"/>. 
        /// </summary>
        public event UiEventHandler<ClientActionArgs> ActionActivated;

        #endregion

        #region Children

        /// <summary>
        /// Executed when a child control is added to this object's child controls.
        /// <para>Raises the <see cref="ControlAdded"/> event.</para>
        /// </summary>
        protected virtual void OnControlAdded(ControlChildArgs e) => ControlAdded?.Invoke(this, e);
        /// <summary>
        /// Executed when a child control is removed from this object's child controls.
        /// <para>Raises the <see cref="ControlRemoved"/> event.</para>
        /// </summary>
        protected virtual void OnControlRemoved(ControlChildArgs e) => ControlRemoved?.Invoke(this, e);
        /// <summary>
        /// Executed when the parent of this control changes.
        /// <para>Raises the <see cref="ParentChanged"/> event.</para>
        /// </summary>
        protected virtual void OnParentChanged(ParentChangeArgs e) => ParentChanged?.Invoke(this, e);
        protected virtual void OnViewChanged(ViewChangeArgs e) => ViewChanged?.Invoke(this, e);

        /// <summary>
        /// Occurs when another control is added as a child of this control.
        /// </summary>
        public event UiEventHandler<ControlChildArgs> ControlAdded;
        /// <summary>
        /// Occurs when another control is removed from the child controls of this control.
        /// </summary>
        public event UiEventHandler<ControlChildArgs> ControlRemoved;
        /// <summary>
        /// Occurs when this control's parent changes.
        /// </summary>
        public event UiEventHandler<ParentChangeArgs> ParentChanged;
        public event UiEventHandler<ViewChangeArgs> ViewChanged;

        #endregion

        /// <summary>
        /// Executed when a control has finished loading.
        /// <para>Raises the <see cref="Initialized"/> event.</para>
        /// </summary>
        protected virtual void OnInitialized(EventArgs e) => Initialized?.Invoke(this, e);
        protected virtual void OnSizeChanged(EventArgs e) => SizeChanged?.Invoke(this, e);

        protected virtual void OnShown(EventArgs e) => Shown?.Invoke(this, e);
        protected virtual void OnHidden(EventArgs e) => Hidden?.Invoke(this, e);
        protected virtual void OnVisibleChanged(EventArgs e) => VisibleChanged?.Invoke(this, e);

        public event UiEventHandler<EventArgs> Initialized;
        /// <summary>
        /// Raised whenever the control's size changes. 
        /// </summary>
        public event UiEventHandler<EventArgs> SizeChanged;
        /// <summary>
        /// Raised whenever the control's visibility changes. 
        /// </summary>
        public event UiEventHandler<EventArgs> VisibleChanged;
        public event UiEventHandler<EventArgs> Shown;
        public event UiEventHandler<EventArgs> Hidden;

    }
}
