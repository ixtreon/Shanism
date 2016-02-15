using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Input;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

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
        /// Gets the close button of this window. 
        /// </summary>
        readonly Button CloseButton = new Button
        {
            ParentAnchor = AnchorMode.Top | AnchorMode.Right,
            Size = new Vector(0.1, 0.07),
            Location = new Vector(0.9, 0),

            Texture = Content.Textures.TryGetUI("close"),
            TextureColor = Color.Black,

            BackColor = Color.Red.SetAlpha(100),
            BackHoverColor = Color.Red.SetAlpha(200),
        };

        /// <summary>
        /// Gets or sets the game action that toggles this window. 
        /// </summary>
        public GameAction? Action { get; set; } 

        /// <summary>
        /// Gets or sets the window of this title. 
        /// </summary>
        public string TitleText { get; set; }

        /// <summary>
        /// Gets or sets whether this window has a title bar. 
        /// </summary>
        public bool HasTitleBar { get; set; } = true;

        /// <summary>
        /// The event raised whenever the window is closed using the titlebar button. 
        /// </summary>
        public event Action<Window> WindowClosed;

        /// <summary>
        /// Creates a new window, optionally specifying the side it is anchored to. 
        /// </summary>
        /// <param name="anchor"></param>
        protected Window(AnchorMode anchor = AnchorMode.None)
        {
            Visible = false;
            Size = DefaultSize;
            BackColor = Color.SaddleBrown;

            switch (anchor)
            {
                case AnchorMode.Left:
                    ParentAnchor = AnchorMode.Left;
                    Location = new Vector(0, 0.25);
                    break;

                case AnchorMode.Right:
                    ParentAnchor = AnchorMode.Right;
                    Location = new Vector(2 - Size.X, 0.25);
                    break;

                default:
                    Location = Vector.Zero + 1 - Size / 2;
                    break;
            }

            CloseButton.MouseUp += CloseButton_MouseUp;
            Add(CloseButton);
        }



        void CloseButton_MouseUp(MouseButtonEvent e)
        {
            Visible = false;
            WindowClosed?.Invoke(this);
        }


        protected override void OnUpdate(int msElapsed)
        {
            CloseButton.Visible = HasTitleBar;

            if (Action.HasValue && KeyboardInfo.IsActivated(Action.Value))
                Visible = !Visible;
        }

        /// <summary>
        /// Draws the background and title-bar of this window. 
        /// </summary>
        /// <param name="sb"></param>
        public override void OnDraw(Graphics g)
        {
            base.OnDraw(g);

            if(HasTitleBar)
            {
                g.Draw(Content.Textures.Blank, Vector.Zero, new Vector(Size.X, TitleHeight), Color.Black.SetAlpha(100));
                g.DrawString(Content.Fonts.FancyFont, TitleText, Color.White, new Vector(Size.X / 2, 0), xAnchor: 0.5f, yAnchor: 0f);
            }
        }
    }
}
