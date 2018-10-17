using Ix.Math;
using Shanism.Common;
using System;
using System.Numerics;

namespace Shanism.Client.UI.Containers
{
    /// <summary>
    /// A basic window-like control with a title, close box and a hotkey. 
    /// </summary>
    public class Window : Control
    {
        /// <summary>
        /// Gets the default size of the title bar of a window. 
        /// </summary>
        public const float DefaultTitleHeight = 0.08f;

        /// <summary>
        /// Gets the default size of a window. 
        /// </summary>
        public static readonly Vector2 DefaultSize
            = new Vector2(1.0f, 1.3f);

        public static readonly Vector2 DefaultCloseButtonSize
            = new Vector2(0.1f, 0.07f);


        readonly Label titleBar;
        readonly Button closeButton;

        /// <summary>
        /// Gets or sets the game action that toggles this window. 
        /// </summary>
        public ClientAction? ToggleAction { get; set; }

        /// <summary>
        /// Gets or sets whether this window can be moved around. 
        /// </summary>
        public bool CanMove { get; set; } = true;

        /// <summary>
        /// Gets or sets whether this window can be resized.
        /// </summary>
        public bool CanResize { get; set; } = true;

        public float TitleHeight => HasTitleBar ? DefaultTitleHeight : 0;

        public Vector2 ClientOffset => new Vector2(Padding, Padding + TitleHeight);
        public Vector2 ClientSize => Size - (new Vector2(0, TitleHeight) + new Vector2(2 * Padding));

        public override RectangleF ClientBounds => new RectangleF(ClientOffset, ClientSize);

        /// <summary>
        /// Gets or sets the window of this title. 
        /// </summary>
        public string TitleText
        {
            get => titleBar.Text;
            set => titleBar.Text = value;
        }

        /// <summary>
        /// Gets or sets whether this window has a title bar. 
        /// </summary>
        public bool HasTitleBar
        {
            get => titleBar.IsVisible;
            set => titleBar.IsVisible = value;
        }

        public Window()
            : this(AnchorMode.None)
        {
            MinimumSize = new Vector2(0.3f, 0.1f);
        }

        /// <summary>
        /// Creates a new window, optionally specifying the side it is anchored to. 
        /// </summary>
        /// <param name="anchor"></param>
        protected Window(AnchorMode anchor)
        {
            CanFocus = true;
            Size = DefaultSize;
            Padding = LargePadding;
            BackColor = UiColors.WindowBackground;
            Hide();

            switch(anchor)
            {
                case AnchorMode.Left:
                    ParentAnchor = AnchorMode.Left | AnchorMode.Bottom;
                    Location = new Vector2(0, Screen.UiSize.Y - Size.Y);
                    CanMove = false;
                    break;

                case AnchorMode.Right:
                    ParentAnchor = AnchorMode.Right | AnchorMode.Bottom;
                    Location = Screen.UiSize - Size;
                    CanMove = false;
                    break;

                default:
                    ParentAnchor = AnchorMode.None;
                    Location = (Screen.UiSize - Size) / 2;
                    CanMove = true;
                    break;
            }

            titleBar = new Label
            {
                TextAlign = AnchorPoint.Center,

                ParentAnchor = AnchorMode.Horizontal | AnchorMode.Top,
                Location = Vector2.Zero,
                Size = new Vector2(Size.X, DefaultTitleHeight),
            };

            closeButton = new Button
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,
                Size = DefaultCloseButtonSize,
                Top = 0,
                Left = Size.X - DefaultCloseButtonSize.X,
                
                Cursor = GameCursor.Default,

                IconName = "cancel",
                Padding = 0,
                //Sprite = Content.UI.MenuClose,
                Tint = Color.Black,
                SpriteSizeMode = TextureSizeMode.FitZoom,

                BackColor = UiColors.ButtonDanger,
            };

            //drag to move
            titleBar.MouseDown += startMouseMove;
            titleBar.MouseMove += doMouseMove;
            titleBar.MouseUp += endMouseMove;

            //drag to resize
            MouseDown += onMouseDown;
            MouseMove += onMouseMove;
            MouseUp += onMouseUp;

            //close
            closeButton.MouseClick += clickCloseButton;

            Add(titleBar);
            titleBar.Add(closeButton);
        }

        public override void Update(int msElapsed)
        {
            titleBar.BackColor = FocusControl.IsChildOf(this) ? UiColors.WindowActiveTitle : UiColors.WindowInactiveTitle;
        }

        #region Drag to resize

        const float resizeDist = 0.03f;

        bool isResizing;
        Vector2 resizeStartLoc;
        AnchorMode _hoverResize;

        void onMouseDown(object o, MouseButtonArgs ev)
        {
            if(_hoverResize != AnchorMode.None)
            {
                resizeStartLoc = ev.Position;
                isResizing = true;
            }
        }

        void onMouseMove(object o, MouseArgs ev)
        {
            //update mouse cursor
            if(!CanResize)
                return;

            if(!isResizing)
            {
                var dNear = ev.Position;
                var dFar = Size - ev.Position;

                _hoverResize = AnchorMode.None;

                if(dNear.X < resizeDist)
                    _hoverResize |= AnchorMode.Left;
                else if(dFar.X < resizeDist)
                    _hoverResize |= AnchorMode.Right;

                if(dNear.Y < resizeDist)
                    _hoverResize |= AnchorMode.Top;
                else if(dFar.Y < resizeDist)
                    _hoverResize |= AnchorMode.Bottom;

                _hoverResize &= ~ParentAnchor;


                switch(_hoverResize)
                {
                    case AnchorMode.Left:
                    case AnchorMode.Right:
                        Cursor = GameCursor.SizeH;
                        break;

                    case AnchorMode.Top:
                    case AnchorMode.Bottom:
                        Cursor = GameCursor.SizeV;
                        break;

                    case AnchorMode.Top | AnchorMode.Left:
                    case AnchorMode.Bottom | AnchorMode.Right:
                        Cursor = GameCursor.SizeNWSE;
                        break;

                    case AnchorMode.Top | AnchorMode.Right:
                    case AnchorMode.Bottom | AnchorMode.Left:
                        Cursor = GameCursor.SizeNESW;
                        break;

                    default:
                        Cursor = GameCursor.Default;
                        break;
                }
            }
            else
            {
                var newSize = Size + ev.Position - resizeStartLoc;
                ClampSize(ref newSize);

                var dSz = newSize - Size;
                var newLoc = Location;
                var newSz = Size;

                switch(_hoverResize & AnchorMode.Horizontal)
                {
                    case AnchorMode.Left:
                        newLoc.X += dSz.X;
                        newSz.X -= dSz.X;
                        break;
                    case AnchorMode.Right:
                        newSz.X += dSz.X;
                        resizeStartLoc.X += (newSz - Size).X;
                        break;
                }

                switch(_hoverResize & AnchorMode.Vertical)
                {
                    case AnchorMode.Top:
                        newLoc.Y += dSz.Y;
                        newSz.Y -= dSz.Y;
                        break;

                    case AnchorMode.Bottom:
                        newSz.Y += dSz.Y;
                        resizeStartLoc.Y += (newSz - Size).Y;
                        break;
                }

                Size = newSz;
                Location = newLoc;
            }
        }

        void onMouseUp(object o, MouseButtonArgs ev)
        {
            isResizing = false;
        }
        #endregion

        #region Drag to move

        Vector2? dragLoc;
        void startMouseMove(object o, MouseButtonArgs ev)
        {
            if(CanMove)
            {
                dragLoc = ev.Position;
            }
        }

        void doMouseMove(object o, MouseArgs ev)
        {
            if(dragLoc != null)
                Location = (Location + ev.Position - dragLoc.Value)
                    .Clamp(Vector2.Zero, Screen.UiSize - Size);
        }

        void endMouseMove(object o, MouseButtonArgs ev)
        {
            dragLoc = null;
        }

        #endregion


        protected override void OnShown(EventArgs e)
        {
            SetFocus();
            base.OnShown(e);
        }

        protected override void OnHidden(EventArgs e)
        {
            ClearFocus();
            base.OnHidden(e);
        }


        protected override void OnActionActivated(ClientActionArgs e)
        {
            if(e.Action == ToggleAction || e.Action == ClientAction.HideMenus)
                IsVisible = !IsVisible;
            base.OnActionActivated(e);
        }

        void clickCloseButton(object _, MouseButtonArgs e)
        {
            Hide();
            OnClosed();
        }

        protected virtual void OnClosed() { }
    }
}
