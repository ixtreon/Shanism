using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Textures;
using Client.Controls;
using Color = Microsoft.Xna.Framework.Color;
using IO.Common;

namespace Client.UI.Common
{
    abstract class Window : Control
    {
        public const double TitleHeight = 0.08;

        public static readonly Vector DefaultSize = new Vector(1.0, 1.2);

        public readonly Button CloseButton = new Button
        {
            ParentAnchor = AnchorMode.Top | AnchorMode.Right,
            Size = new Vector(0.1, 0.07),
            Location = new Vector(0.9, 0),

            Texture = Content.Textures.TryGetUI("close"),
            TextureColor = Color.Black,

            BackColor = Color.Red.SetAlpha(50),
            BackHoverColor = Color.Red.SetAlpha(150),
        };

        public Keybind? Key { get; set; } 

        public string Title { get; set; }

        public Window(AnchorMode anchor = AnchorMode.None)
        {
            this.Size = DefaultSize;

            switch(anchor)
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
                    Location = new Vector(0.25, 0.25);
                    break;
            }

            this.BackColor = Color.SaddleBrown;

            CloseButton.MouseUp += CloseButton_MouseUp;
            this.VisibleChanged += Window_VisibleChanged;
            Add(CloseButton);
        }

        void Window_VisibleChanged(Control sender)
        {
            if(this.Visible)
            {
                
            }
        }

        private void CloseButton_MouseUp(Control sender, Vector obj)
        {
            this.Visible = false;
        }


        public override void Update(int msElapsed)
        {
            if (KeyManager.IsActivated(Key.Value))
                Visible = !Visible;
        }

        /// <summary>
        /// Draws the background and title-bar of this window. 
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(Graphics g)
        {
            base.Draw(g);

            g.Draw(Content.Textures.Blank, Vector.Zero, new Vector(Size.X, TitleHeight), Color.Black.SetAlpha(100));
            g.DrawString(Content.Fonts.FancyFont, Title, Color.White, new Vector(Size.X / 2, 0), xAnchor: 0.5f, yAnchor: 0f);
        }
    }
}
