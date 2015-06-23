using Microsoft.Xna.Framework;
using Client.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Client.Textures;
using Client.Controls;

namespace Client.UI
{
    abstract class Window : Control
    {
        const float TitleHeight = 0.08f;

        public static readonly Vector2 DefaultSize = new Vector2(1.0f, 1.2f);

        public WindowAlign Align { get; set; }

        public readonly Button CloseButton = new Button()
        {
            Size = new Vector2(0.05f),
            RelativePosition = new Vector2(0.85f, 0.02f),
            Texture = TextureCache.Get(@"UI\close"),
            BackColor = Color.Black.SetAlpha(50),
        };

        public Keybind? Key { get; set; } 

        public string Title { get; set; }

        public Window(WindowAlign align = WindowAlign.None)
        {
            this.Size = DefaultSize;
            this.AbsolutePosition = new Vector2(-1, (2 - DefaultSize.Y) / 2 - 1);
            this.BackColor = Color.SaddleBrown;
            this.Align = align;

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

        private void CloseButton_MouseUp(Control sender, Vector2 obj)
        {
            this.Visible = false;
        }

        private void updatePosition()
        {
            const int anchor = 3;

            switch(Align)
            {
                case WindowAlign.Left:
                    ScreenPosition = new Point(anchor, Screen.ScreenHalfSize.Y - ScreenSize.Y / 2);
                    break;
                case WindowAlign.Right:
                    ScreenPosition = new Point(Screen.Size.X - ScreenSize.X - anchor, Screen.ScreenHalfSize.Y - ScreenSize.Y / 2);
                    break;
                case WindowAlign.None:
                    break;
            }
        }

        public override void Update(int msElapsed)
        {
            updatePosition();
            CloseButton.AbsolutePosition = AbsolutePosition + new Vector2(Size.X - CloseButton.Size.X - 0.01f, 0.01f);
            if (KeyManager.IsActivated(Key.Value))
                this.Visible = !this.Visible;
            base.Update(msElapsed);
        }

        /// <summary>
        /// Draws the background and title of the window. 
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            SpriteFactory.Blank.Draw(sb, AbsolutePosition, new Vector2(Size.X, TitleHeight), Color.Black.SetAlpha(100));
            TextureCache.FancyFont.DrawStringScreen(sb, Title, Color.White, ScreenPosition + new Point(ScreenSize.X / 2, 0), xAnchor: 0.5f, yAnchor: 0f);
        }
    }

    enum WindowAlign
    {
        Left, Right, None
    }
}
