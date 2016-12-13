using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shanism.Client.Drawing;
using Shanism.Client.Input;
using Shanism.Client.UI;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.GameScreens
{
    abstract class UiScreen : GameScreen
    {
        static readonly Color BackColor = new Color(48, 24, 48);

        static readonly TextureFont TitleFont = Control.Content.Fonts.ShanoFont;
        static readonly TextureFont SubTitleFont = Control.Content.Fonts.FancyFont;


        public virtual Control Root { get; } = new Control
        {
            BackColor = BackColor,
        };

        UiScreen subScreen;

        readonly Label title;
        readonly Label subTitle;

        protected readonly GraphicsDevice device;
        readonly Graphics g;

        Control actualRoot
            => subScreen?.actualRoot ?? Root;

        public event Action Closed;

        public event Action<IShanoEngine> GameStarted;



        public string SubTitle
        {
            get { return subTitle.Text; }
            set { subTitle.Text = value; }
        }

        public UiScreen(GraphicsDevice device)
        {
            this.device = device;
            g = new Graphics(device);

            Root.Maximize();
            var titleFont = Control.Content.Fonts.ShanoFont;
            Root.Add(title = new Label
            {
                Font = titleFont,
                Text = "Shanism",

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Size = new Vector(Screen.UiSize.X, titleFont.HeightUi),
                Location = new Vector(0, 0.3),
                TextXAlign = 0.5f,

            });

            Root.Add(subTitle = new Label
            {
                Text = string.Empty,
                Font = SubTitleFont,

                ParentAnchor = AnchorMode.Left | AnchorMode.Right,

                Size = new Vector(Screen.UiSize.X, SubTitleFont.HeightUi),
                Location = new Vector(0, title.Bottom + Control.LargePadding),
                TextXAlign = 0.5f,

                AutoSize = false,
            });

            Root.Add(new UI.Tooltips.SimpleTip());

            Root.KeyPressed += root_KeyActivated;
        }

        public void StartGame(IShanoEngine game) => GameStarted?.Invoke(game);

        void root_KeyActivated(Keybind k)
        {
            if (k.Key == Keys.Escape)
            {
                Closed?.Invoke();
            }
        }

        public override void Draw()
        {
            g.Begin();

            actualRoot.Draw(g);

            g.End();
        }

        public override void Update(int msElapsed)
        {
            g.Bounds = new RectangleF(Vector.Zero, Screen.UiSize);

            actualRoot.Maximize();
            actualRoot.UpdateMain(msElapsed);
            actualRoot.Update(msElapsed);
        }

        /// <summary>
        /// Sets the child screen.
        /// </summary>
        protected virtual void SetScreen(UiScreen newScreen)
        {
            subScreen = newScreen;
            subScreen.Closed += ResetSubScreen;
        }

        /// <summary>
        /// Resets the child screen.
        /// </summary>
        protected void ResetSubScreen()
            => subScreen = null;
    }
}
