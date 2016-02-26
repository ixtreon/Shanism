using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Input;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;
using Microsoft.Xna.Framework.Input;

namespace Client.UI.Common
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
        public static readonly Vector DefaultSize = new Vector(1.0, 1.2);



        /// <summary>
        /// Gets or sets the game action that toggles this window. 
        /// </summary>
        public GameAction? ToggleAction { get; set; }

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

        ///// <summary>
        /// Gets or sets whether this window has a title bar. 
        /// </summary>
        public bool HasTitleBar
        {
            get { return titleBar.IsVisible; }
            set { titleBar.IsVisible = value; }
        }

        /// <summary>
        /// The event raised whenever the close button is clicked, 
        /// right before the window is hidden. 
        /// </summary>
        public event Action CloseButtonClicked;

        readonly Label titleBar;
        readonly Button CloseButton;

        /// <summary>
        /// Creates a new window, optionally specifying the side it is anchored to. 
        /// </summary>
        /// <param name="anchor"></param>
        protected Window(AnchorMode anchor = AnchorMode.None)
        {
            IsVisible = false;
            //CanFocus = true;
            Size = DefaultSize;
            BackColor = Color.SaddleBrown;
            VisibleChanged += onVisibleChanged;
            GameActionActivated += onActionActivated;

            switch (anchor)
            {
                case AnchorMode.Left:
                    ParentAnchor = AnchorMode.Left;
                    Location = new Vector(0, 0.25);
                    Locked = true;
                    break;

                case AnchorMode.Right:
                    ParentAnchor = AnchorMode.Right;
                    Location = new Vector(2 - Size.X, 0.25);
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

            CloseButton = new Button
            {
                ParentAnchor = AnchorMode.Top | AnchorMode.Right,
                Size = new Vector(0.1, 0.07),
                Location = new Vector(DefaultSize.X - 0.1, 0),

                Texture = Content.Textures.TryGetUI("close"),
                TextureColor = Color.Black,

                BackColor = Color.Red.SetAlpha(150),
            };
            CloseButton.MouseUp += CloseButton_MouseUp;

            Add(titleBar);
            titleBar.Add(CloseButton);

        }

        #region Drag to move

        Vector? dragLoc = null;

        void onTitleBarMouseUp(MouseButtonArgs ev)
        {
            dragLoc = null;
            Console.WriteLine("UP!");
        }

        void onTitleBarMouseMove(MouseArgs ev)
        {
            if (dragLoc != null)
            {
                Location += ev.RelativePosition - dragLoc.Value;
                Console.WriteLine("MOVE!");
            }
        }

        void onTitleBarMouseDown(MouseButtonArgs ev)
        {
            if (!Locked)
            {
                dragLoc = ev.RelativePosition;
                Console.WriteLine("DOWN!");
            }
        }
        #endregion

        void onVisibleChanged(Control obj)
        {
            if (IsVisible)
                SetFocus();
            else
                ClearFocus();
        }

        void onActionActivated(GameAction ga)
        {
            if (ga == ToggleAction || ga == GameAction.ToggleMenus)
                IsVisible = !IsVisible;
        }

        void CloseButton_MouseUp(MouseButtonArgs e)
        {
            CloseButtonClicked?.Invoke();
            IsVisible = false;
        }
    }
}
