using System;
using System.Collections.Generic;
using System.Linq;
using Shanism.Client.Input;
using Shanism.Common;

namespace Shanism.Client.UI.Common
{
    /// <summary>
    /// A basic window-like control with a title, close box and a hotkey. 
    /// </summary>
    abstract class Window : Control
    {
        /// <summary>
        /// Gets the default size of the title bar of a window. 
        /// </summary>
        public const double TitleHeight = 0.08;

        /// <summary>
        /// Gets the default size of a window. 
        /// </summary>
        public static readonly Vector DefaultSize = new Vector(1.0, 1.3);



        /// <summary>
        /// Gets or sets the game action that toggles this window. 
        /// </summary>
        public ClientAction? ToggleAction { get; set; }

        /// <summary>
        /// Gets or sets whether this window can be moved around. 
        /// </summary>
        public bool Locked { get; set; } = true;

        /// <summary>
        /// Gets or sets the window of this title. 
        /// </summary>
        public string TitleText
        {
            get { return titleBar.Text; }
            set { titleBar.Text = value; }
        }

        /// <summary>
        /// Gets or sets whether this window has a title bar. 
        /// </summary>
        public bool HasTitleBar
        {
            get { return titleBar.IsVisible; }
            set { titleBar.IsVisible = value; }
        }

        readonly Label titleBar;
        readonly Button CloseButton;

        public Vector MinimumSize { get; set; } = new Vector(0.3, TitleHeight + 2 * Padding);

        public Vector MaximumSize { get; set; } = Vector.MaxValue;

        /// <summary>
        /// Creates a new window, optionally specifying the side it is anchored to. 
        /// </summary>
        /// <param name="anchor"></param>
        protected Window(AnchorMode anchor = AnchorMode.None)
        {
            IsVisible = false;
            CanFocus = true;
            Size = DefaultSize;
            BackColor = Color.SaddleBrown;
            VisibleChanged += onVisibleChanged;
            GameActionActivated += onActionActivated;

            switch (anchor)
            {
                case AnchorMode.Left:
                    ParentAnchor = AnchorMode.Left | AnchorMode.Bottom;
                    Location = new Vector(0, 1 - Size.Y);
                    Locked = true;
                    break;

                case AnchorMode.Right:
                    ParentAnchor = AnchorMode.Right | AnchorMode.Bottom;
                    Location = new Vector(2 - Size.X, 1 - Size.Y);
                    Locked = true;
                    break;

                default:
                    Location = Vector.One - Size / 2;
                    Locked = false;
                    break;
            }

            titleBar = new Label
            {
                IsVisible = true,
                AutoSize = false,
                TextXAlign = 0.5f,

                Location = Vector.Zero,
                Size = new Vector(Size.X, TitleHeight),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                BackColor = Color.Black.SetAlpha(100),
            };
            titleBar.MouseDown += onTitleBarMouseDown;
            titleBar.MouseMove += onTitleBarMouseMove;
            titleBar.MouseUp += onTitleBarMouseUp;

            MouseDown += onMouseDown;
            MouseMove += onMouseMove;
            MouseUp += onMouseUp;

            CloseButton = new Button
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,
                Size = new Vector(0.1, 0.07),
                Location = new Vector(DefaultSize.X - 0.1, 0),

                Texture = Content.UI.MenuClose,
                TextureColor = Color.Black,

                BackColor = Color.Red.SetAlpha(150),
            };
            CloseButton.MouseUp += CloseButton_MouseUp;

            Add(titleBar);
            titleBar.Add(CloseButton);

        }

        #region Drag to resize
        Vector? sizeLoc;

        void onMouseDown(MouseButtonArgs ev)
        {
            var dFromCorner = Size - ev.Position;
            if (dFromCorner.X + dFromCorner.Y < 3 * Padding)
            {
                sizeLoc = dFromCorner;
            }
        }

        void onMouseMove(MouseArgs ev)
        {
            if (sizeLoc != null)
            {
                Size = (ev.Position + sizeLoc.Value)
                    .Clamp(MinimumSize, MaximumSize);
            }
        }

        void onMouseUp(MouseButtonArgs ev)
        {
            sizeLoc = null;
        }
        #endregion

        #region Drag to move
        Vector? dragLoc;

        void onTitleBarMouseMove(MouseArgs ev)
        {
            if (dragLoc != null)
                Location += ev.Position - dragLoc.Value;
        }

        void onTitleBarMouseDown(MouseButtonArgs ev)
        {
            if (!Locked)
            {
                dragLoc = ev.Position;
            }
        }
        void onTitleBarMouseUp(MouseButtonArgs ev)
        {
            dragLoc = null;
        }
        #endregion

        void onVisibleChanged(Control obj)
        {
            if (IsVisible)
                SetFocus();
            else
                ClearFocus();
        }

        void onActionActivated(ClientAction ga)
        {
            if (ga == ToggleAction || ga == ClientAction.ToggleMenus)
                IsVisible = !IsVisible;
        }

        void CloseButton_MouseUp(MouseButtonArgs e)
        {
            OnCloseButtonClicked();
            IsVisible = false;
        }

        protected virtual void OnCloseButtonClicked() { }
    }
}
